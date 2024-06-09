using System.ComponentModel;

namespace HarmonyOfEmotions.Domain.Exceptions
{
	public enum ServiceName
	{
		[Description("External error while getting audio from preview URL of Spotify")]
		SpotifyAudioService,
		[Description("Internal server error while saving audio to file")]
		AudioFileService,
		[Description("Internal server error while creating spectrogram")]
		SpectrogramService,
		[Description("Internal server error while creating emotion prediction")]
		EmotionPredictionService,
		[Description("Internal server error while creating memory stream from spectrogram")]
		MemoryStreamService,
		[Description("Internal server error while getting data from Firebase repository")]
		FirebaseRepositoryService,
		[Description("Internal server error while saving data to Firebase repository")]
		FirebaseSaveService,
		[Description("Internal server error while getting data from SQL repository")]
		SQLRepositoryService,
		[Description("Internal server error while saving data to SQL repository")]
		SQLSaveService,
		[Description("Internal server error while getting data from LastFM API")]
		LastFMApiService,
		[Description("Internal server error while getting data from Spotify API")]
		SpotifyApiService,
		[Description("Internal server error while getting data from MusicBrainz API")]
		MusicBrainzApiService,
		[Description("Internal server error while generating recommendations")]
		RecommendationService
	}
}
