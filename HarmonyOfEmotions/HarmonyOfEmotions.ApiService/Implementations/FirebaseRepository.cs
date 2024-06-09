using FireSharp.Interfaces;
using System.Collections.Concurrent;
using HarmonyOfEmotions.Domain;
using HarmonyOfEmotions.ApiService.Interfaces;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class FirebaseRepository(IFirebaseClient client, ILogger<FirebaseRepository> logger) : IRepository
	{
		private readonly IFirebaseClient _client = client;
		private readonly ILogger<FirebaseRepository> _logger = logger;
		private readonly ConcurrentDictionary<string, object> _cache = new();

		public void AddTrackInDatabase(Track track)
		{
			_cache.Clear();
			_client.Set($"Tracks/{track.Id}", track);
		}

		public async Task<Centroid[]> GetCentroids()
		{
			const string cacheKey = "Centroids";
			if (_cache.TryGetValue(cacheKey, out var cachedData))
			{
				_logger.LogInformation("Centroids found in cache");
				return (Centroid[])cachedData;
			}

			var response = await _client.GetAsync(cacheKey);
			_logger.LogInformation("Centroids found");
			var data = response.ResultAs<Centroid[]>();
			_cache.TryAdd(cacheKey, data);
			return data;
		}

		public async Task<Centroid[]> GetClusteredData()
		{
			const string cacheKey = "ClusteredSongs";
			if (_cache.TryGetValue(cacheKey, out var cachedData))
			{
				_logger.LogInformation("Clustered data found in cache");
				return (Centroid[])cachedData;
			}

			var response = await _client.GetAsync(cacheKey);
			_logger.LogInformation("Clustered data found");
			var data = response.ResultAs<Centroid[]>();
			_cache.TryAdd(cacheKey, data);
			return data;
		}

		public async Task<Track> GetTrackById(string id)
		{
			var cacheKey = $"Tracks/{id}";
			if (_cache.TryGetValue(cacheKey, out var cachedData))
			{
				return (Track)cachedData;
			}

			var response = await _client.GetAsync(cacheKey);
			var data = response.ResultAs<Track>();
			_cache.TryAdd(cacheKey, data);
			return data;
		}

		public async Task<Track> GetTrackByIndex(int index)
		{
			const string cacheKey = "Tracks";
			if (_cache.TryGetValue(cacheKey, out var cachedData))
			{
				var tracks = (Track[])cachedData;
				_logger.LogInformation("Track found by index {Index} in cache", index);
				return tracks[index];
			}

			var response = await _client.GetAsync(cacheKey);
			_logger.LogInformation("Track found by index {Index}", index);
			var data = response.ResultAs<Dictionary<string, Track>>();
			_cache.TryAdd(cacheKey, data.Values.ToArray());
			return data.ElementAt(index).Value;
		}

		public async Task<Track[]> GetTracks()
		{
			const string cacheKey = "Tracks";
			if (_cache.TryGetValue(cacheKey, out var cachedData))
			{
				return (Track[])cachedData;
			}

			var response = await _client.GetAsync(cacheKey);
			var data = response.ResultAs<Dictionary<string, Track>>();
			_cache.TryAdd(cacheKey, data.Values.ToArray());
			return [.. data.Values];
		}

		public async Task<Track[]> GetTracksByArtist(string artist)
		{
			const string cacheKey = "Tracks";
			if (_cache.TryGetValue(cacheKey, out var cachedData))
			{
				var tracks = (Track[])cachedData;
				return tracks.Where(t => t.Artist!.Contains(artist)).ToArray();
			}

			var response = await _client.GetAsync(cacheKey);
			var data = response.ResultAs<Track[]>();
			_cache.TryAdd(cacheKey, data);
			return data.Where(t => t.Artist!.Contains(artist)).ToArray();
		}
	}
}
