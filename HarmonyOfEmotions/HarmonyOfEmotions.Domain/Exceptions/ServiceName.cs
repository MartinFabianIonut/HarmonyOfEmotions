using System.ComponentModel;

namespace HarmonyOfEmotions.Domain.Exceptions
{
	public enum ServiceName
	{
		[Description("Error retrieving audio preview from Spotify.")]
		SpotifyAudioService,

		[Description("Error saving audio file.")]
		AudioFileService,

		[Description("Error creating spectrogram.")]
		SpectrogramService,

		[Description("Error predicting emotion.")]
		EmotionPredictionService,

		[Description("Error creating memory stream from spectrogram.")]
		MemoryStreamService,

		[Description("Error retrieving data from Firebase.")]
		FirebaseRepositoryService,

		[Description("Error saving data to Firebase.")]
		FirebaseSaveService,

		[Description("Error retrieving data from SQL repository.")]
		SQLRepositoryService,

		[Description("Error saving data to SQL repository.")]
		SQLSaveService,

		[Description("Error retrieving data from LastFM API.")]
		LastFMApiService,

		[Description("Error retrieving data from Spotify API.")]
		SpotifyApiService,

		[Description("Error retrieving data from MusicBrainz API.")]
		MusicBrainzApiService,

		[Description("Error generating recommendations.")]
		RecommendationService
	}
}
