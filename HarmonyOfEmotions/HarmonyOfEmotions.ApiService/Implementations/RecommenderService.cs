using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain;
using HarmonyOfEmotions.ServiceDefaults.Utils;
using HarmonyOfEmotions_ApiService;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class RecommenderService(IRepository repository, 
		HttpClient httpClient, 
		ILogger<RecommenderService> logger) : IRecommenderService
	{
		private readonly IRepository _repository = repository;
		private readonly HttpClient _httpClient = httpClient;
		private readonly ILogger<RecommenderService> _logger = logger;

		public async Task<string> GetEmotionForTrack(string previewUrl)
		{
			var audio = await _httpClient.GetByteArrayAsync(previewUrl);
			// save audio to file
			File.WriteAllBytes("audio-wow.mp3", audio);
			var spectrogramBitmap = AudioUtils.CreateGraySpetrogram(audio);
			var imageMemoryStream = AudioUtils.GetMemoryStreamFromSpetrogram(spectrogramBitmap);
			imageMemoryStream.Position = 0;

			var imageBytes = imageMemoryStream.ToArray();

			ClassifierModel.ModelInput sampleData = new ClassifierModel.ModelInput()
			{
				ImageSource = imageBytes,
			};

			_logger.LogInformation("Start processing image...");

			// Measure execution time.
			var watch = System.Diagnostics.Stopwatch.StartNew();

			var result = ClassifierModel.Predict(sampleData);

			// Stop measuring time.
			watch.Stop();
			var elapsedMs = watch.ElapsedMilliseconds;

			_logger.LogInformation($"Image processed in {elapsedMs} miliseconds");

			return result.PredictedLabel;
		}

		public async Task<Track[]> GetRecommendedTraks(float x, float y, float z)
		{
			var centroids = await _repository.GetCentroids();
			var clusteredData = await _repository.GetClusteredData();

			var tracks = await _repository.GetTracks();
			// sort the tracks by the distance to the given point, x for Energy, y for Valence and z for Danceability
			var sortedTracks = tracks
				.Select((t, index) => new { Track = t, Index = index })
				.OrderBy(t => Math.Sqrt(Math.Pow(t.Track.Energy - x, 2) + Math.Pow(t.Track.Valence - y, 2) + Math.Pow(t.Track.Danceability - z, 2)))
				.Select(t => t.Index);

			var nearestCentroidsWithIndices = clusteredData
				.Select((c, index) => new { Data = c, Index = index })
				.OrderBy(c => Math.Sqrt(Math.Pow(c.Data.PC1 - x, 2) + Math.Pow(c.Data.PC2 - y, 2) + Math.Pow(c.Data.PC3 - z, 2)))
				.Select(c => c.Index);

			var firstFiveTracksIndices = nearestCentroidsWithIndices
				.Where(n => sortedTracks.Contains(n)) 
				.Take(5) 
				.ToList();

			var recommendedTracks = new List<Track>();

			foreach (var index in firstFiveTracksIndices)
			{
				var track = await _repository.GetTrackByIndex(index);
				if (track != null)
				{
					recommendedTracks.Add(track);
				}
			}

			return [.. recommendedTracks];
		}
	}
}