using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain;
using HarmonyOfEmotions.Domain.Exceptions;
using HarmonyOfEmotions.ServiceDefaults.Utils;
using HarmonyOfEmotions_ApiService;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class RecommenderService(IRepository repository,
		IUserTrackService userTrackService,
		HttpClient httpClient,
		ILogger<RecommenderService> logger) : IRecommenderService
	{
		private readonly IRepository _repository = repository;
		private readonly IUserTrackService _userTrackService = userTrackService;
		private readonly HttpClient _httpClient = httpClient;
		private readonly ILogger<RecommenderService> _logger = logger;

		public async Task<string> GetEmotionForTrack(string previewUrl)
		{
			try
			{
				_logger.LogInformation("Start getting audio from preview url...");
				byte[] audio;
				try
				{
					audio = await _httpClient.GetByteArrayAsync(previewUrl);
					_logger.LogInformation("Audio received");
				}
				catch (HttpRequestException httpRequestException)
				{
					throw new ExternalServiceException(ServiceName.SpotifyAudioService, httpRequestException);
				}

				double[] audioAsDouble = audio.Select(b => (double)b).ToArray();

				// Save audio to file
				try
				{
					File.WriteAllBytes("../tmp/audio-wow.mp3", audio);
				}
				catch (IOException ioException)
				{
					throw new InternalServerErrorException(ServiceName.AudioFileService, ioException);
				}

				var spectrogramBitmap = AudioUtils.CreateGraySpetrogram(audio);


				var imageMemoryStream = AudioUtils.GetMemoryStreamFromSpetrogram(spectrogramBitmap);
				imageMemoryStream.Position = 0;

				var imageBytes = imageMemoryStream.ToArray();
				var sampleData = new MLModel.ModelInput()
				{
					ImageSource = imageBytes,
				};

				_logger.LogInformation("Start processing image...");

				var watch = System.Diagnostics.Stopwatch.StartNew();
				MLModel.ModelOutput result;
				try
				{
					result = MLModel.Predict(sampleData);
				}
				catch (Exception predictionException)
				{
					throw new InternalServerErrorException(ServiceName.EmotionPredictionService, predictionException);
				}

				watch.Stop();
				var elapsedMs = watch.ElapsedMilliseconds;
				_logger.LogInformation($"Image processed in {elapsedMs} milliseconds");

				return result.PredictedLabel;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "General error while getting emotion for track");
				throw new InternalServerErrorException(ServiceName.EmotionPredictionService, ex);
			}
		}

		public async Task<Track[]> GetRecommendedTraks(float x, float y, float z, string userId)
		{
			try
			{
				var centroids = await _repository.GetCentroids();
				var clusteredData = await _repository.GetClusteredData();
				var tracks = await _repository.GetTracks();

				var userTrackPreferences = await _userTrackService.GetUserTrackPreferences(userId);
				userTrackPreferences = userTrackPreferences.Where(t => !t.IsLiked).ToList();

				var sortedTracks = tracks
					.Select((t, index) => new { Track = t, Index = index })
					.OrderBy(t => Math.Sqrt(Math.Pow(t.Track.Energy - x, 2) + Math.Pow(t.Track.Valence - y, 2) + Math.Pow(t.Track.Danceability - z, 2)))
					.Select(t => t.Index);

				var nearestCentroidsWithIndices = clusteredData
					.Select((c, index) => new { Data = c, Index = index })
					.OrderBy(c => Math.Sqrt(Math.Pow(c.Data.PC1 - x, 2) + Math.Pow(c.Data.PC2 - y, 2) + Math.Pow(c.Data.PC3 - z, 2)))
					.Select(c => c.Index);

				var firstFiveTracksIndices = sortedTracks
					.Join(sortedTracks,
						n => n,
						s => s,
						(n, s) => n)
					.Where(n => !userTrackPreferences.Any(t => t.TrackId == tracks[n].Id))
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
			catch (Exception ex)
			{
				_logger.LogError(ex, "General error while getting recommended tracks");
				throw new InternalServerErrorException(ServiceName.RecommendationService, ex);
			}
		}
	}
}