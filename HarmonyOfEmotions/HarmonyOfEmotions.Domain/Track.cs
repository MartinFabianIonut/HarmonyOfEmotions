using Newtonsoft.Json;

namespace HarmonyOfEmotions.Domain
{
	public class Track
	{
		[JsonProperty("id")]
		public string? Id { get; set; }

		[JsonProperty("song_name")]
		public string? Name { get; set; }

		[JsonProperty("artist")]
		public string? Artist { get; set; }

		[JsonProperty("other_artists")]
		private string? OtherArtistsRaw { get; set; }
		public string[]? OtherArtists
		{
			get
			{
				if (string.IsNullOrEmpty(OtherArtistsRaw))
					return null;

				return OtherArtistsRaw.Trim('[', ']').Split(",").Select(s => s.Trim('\'')).ToArray();
			}
			set => OtherArtistsRaw = $"[{string.Join(",", value.Select(s => $"'{s}'"))}]";
		}

		[JsonProperty("album")]
		public string? Album { get; set; }

		[JsonProperty("preview_url")]
		public string? PreviewUrl { get; set; }

		[JsonProperty("image_url")]
		public string? ImageUrl { get; set; }

		[JsonProperty("track_number")]
		public int TrackNumber { get; set; }

		public DateTime Year 
		{
			get 
			{
				if (string.IsNullOrEmpty(YearString))
				{
					return DateTime.MinValue;
				}
				else
				{
					if (YearString.Length == 4 && int.TryParse(YearString, out int year))
					{
						return new DateTime(year, 1, 1);
					}
					else if (YearString.Length == 10 &&
							 int.TryParse(YearString.AsSpan(0, 4), out year) &&
							 int.TryParse(YearString.AsSpan(5, 2), out int month) &&
							 int.TryParse(YearString.AsSpan(8, 2), out int day))
					{
						return new DateTime(year, month, day);
					}
					else
					{
						throw new FormatException("The year is not in a valid format.");
					}
				}
			}
			set => YearString = value.Year.ToString();
		}

		[JsonProperty("year")]
		private string? YearString { get; set; }

		[JsonProperty("danceability")]
		public float Danceability { get; set; }

		[JsonProperty("energy")]
		public float Energy { get; set; }

		[JsonProperty("key")]
		public float Key { get; set; }

		[JsonProperty("loudness")]
		public float Loudness { get; set; }

		[JsonProperty("speechiness")]
		public float Speechiness { get; set; }

		[JsonProperty("acousticness")]
		public float Acousticness { get; set; }

		[JsonProperty("instrumentalness")]
		public float Instrumentalness { get; set; }

		[JsonProperty("liveness")]
		public float Liveness { get; set; }

		[JsonProperty("valence")]
		public float Valence { get; set; }

		[JsonProperty("tempoid")]
		public float Tempo { get; set; }
	}
}
