using HarmonyOfEmotions.Domain;
using SpotifyAPI.Web;

namespace HarmonyOfEmotions.ServiceDefaults.Utils
{
	public static class TrackUtils
	{
		public static Track ConvertToTrack(FullTrack track)
		{
			return new Track
			{
				Id = track.Id,
				Name = track.Name,
				Artist = track.Artists.First().Name,
				ArtistId = track.Artists.First().Id,
				Album = track.Album.Name,
				OtherArtists = track.Artists.Skip(1).Select(a => a.Name).ToArray(),
				OtherArtistIds = track.Artists.Skip(1).Select(a => a.Id).ToArray(),
				PreviewUrl = track.PreviewUrl,
				ImageUrl = track.Album.Images.First().Url,
				TrackNumber = track.TrackNumber,
				Year = DateUtils.ConvertStringToDateTime(track.Album.ReleaseDate)
			};
		}
		public static Track[] ConvertToTracks(List<FullTrack> tracks)
		{
			return tracks.Select(t => ConvertToTrack(t)).ToArray();
		}
	}
}
