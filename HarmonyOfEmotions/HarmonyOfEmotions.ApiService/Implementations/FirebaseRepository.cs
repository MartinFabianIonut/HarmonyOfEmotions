using FireSharp.Interfaces;
using HarmonyOfEmotions.ApiService.Interfaces;
using HarmonyOfEmotions.Domain.Exceptions;
using HarmonyOfEmotions.Domain.RecommenderSystem;
using HarmonyOfEmotions.ServiceDefaults.Exceptions;
using System.Collections.Concurrent;

namespace HarmonyOfEmotions.ApiService.Implementations
{
	public class FirebaseRepository(IFirebaseClient client, ILogger<FirebaseRepository> logger) : IRepository
	{
		private readonly IFirebaseClient _client = client;
		private readonly ILogger<FirebaseRepository> _logger = logger;
		private readonly ConcurrentDictionary<string, object> _cache = new();

		public async Task<Centroid[]> GetCentroids()
		{
			try
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
			catch (Exception repositoryException)
			{
				throw new InternalServerErrorException(ServiceName.FirebaseRepositoryService, repositoryException);
			}
		}

		public async Task<Centroid[]> GetClusteredData()
		{
			try
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
			catch (Exception repositoryException)
			{
				throw new InternalServerErrorException(ServiceName.FirebaseRepositoryService, repositoryException);
			}
		}

		public async Task<Track> GetTrackById(string id)
		{
			try
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
			catch (Exception repositoryException)
			{
				throw new InternalServerErrorException(ServiceName.FirebaseRepositoryService, repositoryException);
			}
		}

		public async Task<Track> GetTrackByIndex(int index)
		{
			try
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
			catch (Exception repositoryException)
			{
				throw new InternalServerErrorException(ServiceName.FirebaseRepositoryService, repositoryException);
			}
		}

		public async Task<Track[]> GetTracks()
		{
			try
			{
				const string cacheKey = "Tracks";
				if (_cache.TryGetValue(cacheKey, out var cachedData))
				{
					return (Track[])cachedData;
				}

				var response = await _client.GetAsync(cacheKey);
				var data = response.ResultAs<Dictionary<string, Track>>();
				foreach (var track in data.Values)
				{
					track.IsRecommended = true;
				}
				_cache.TryAdd(cacheKey, data.Values.ToArray());
				return [.. data.Values];
			}
			catch (Exception repositoryException)
			{
				throw new InternalServerErrorException(ServiceName.FirebaseRepositoryService, repositoryException);
			}
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
