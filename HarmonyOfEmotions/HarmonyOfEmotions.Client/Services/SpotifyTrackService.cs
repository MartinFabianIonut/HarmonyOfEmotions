﻿using HarmonyOfEmotions.Domain;

namespace HarmonyOfEmotions.Client.Services
{
	public class SpotifyTrackService(ApiClient apiClient)
	{
		private readonly ApiClient _apiClient = apiClient;

		public async Task<IEnumerable<Track>?> GetTopTracksForArtist(string artistId)
		{
			return await _apiClient.GetAsync<IEnumerable<Track>>($"SpotifyTrack/GetTopTracksForArtist?artistId={artistId}");
		}

		public async Task<IEnumerable<Track>?> SearchTracksByKeyword(string keyword)
		{
			return await _apiClient.GetAsync<IEnumerable<Track>>($"SpotifyTrack/SearchTraksByKeyword?keyword={keyword}");
		}
	}
}
