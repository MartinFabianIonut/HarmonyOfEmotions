using HarmonyOfEmotions.Domain.RecommenderSystem;

namespace HarmonyOfEmotions.ApiService.Interfaces
{
	public interface IRepository
	{
		Task<Centroid[]> GetCentroids();
		Task<Centroid[]> GetClusteredData();
		Task<Track[]> GetTracks();
		Task<Track> GetTrackById(string id);
		Task<Track> GetTrackByIndex(int index);
		Task<Track[]> GetTracksByArtist(string artist);
	}
}
