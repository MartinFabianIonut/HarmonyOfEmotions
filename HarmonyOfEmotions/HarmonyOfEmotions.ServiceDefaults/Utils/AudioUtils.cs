using HarmonyOfEmotions.Domain.Exceptions;
using HarmonyOfEmotions.ServiceDefaults.Exceptions;
using HarmonyOfEmotions.ServiceDefaults.Utils.Spectrogram;
using MP3Sharp;
using SkiaSharp;

namespace HarmonyOfEmotions.ServiceDefaults.Utils
{
	public static class AudioUtils
	{
		private static double[] ReadMP3(string filePath, int bufferSize = 4096)
		{
			List<double> audio = [];
			MP3Stream stream = new(filePath);
			byte[] buffer = new byte[bufferSize];
			int bytesReturned = 1;
			while (bytesReturned > 0)
			{
				bytesReturned = stream.Read(buffer, 0, bufferSize);
				for (int i = 0; i < bytesReturned / 2 - 1; i += 2)
					audio.Add(BitConverter.ToInt16(buffer, i * 2));
			}
			stream.Close();
			return [.. audio];
		}

		public static SKBitmap CreateGraySpetrogram(byte[] mp3Bytes)
		{
			try
			{
				double[] audio;
				try
				{
					audio = ReadMP3("../tmp/audio-wow.mp3");
					// delete file
					File.Delete("../tmp/audio-wow.mp3");
				}
				catch (IOException ioException)
				{
					throw new InternalServerErrorException(ServiceName.AudioFileService, ioException);
				}
				int sampleRate = 44100;

				int fftSize = 1024;
				int targetWidthPx = 775;
				int stepSize = audio.Length / targetWidthPx;

				var spectogramGenerator = new SpectrogramGenerator(sampleRate, fftSize, stepSize, maxFreq: sampleRate / 2);
				spectogramGenerator.Add(audio);
				var bmp = spectogramGenerator.GetBitmap();
				return bmp;
			}
			catch (Exception spectrogramException)
			{
				throw new InternalServerErrorException(ServiceName.SpectrogramService, spectrogramException);
			}
		}

		public static MemoryStream GetMemoryStreamFromSpetrogram(SKBitmap bmp)
		{
			try
			{
				var ms = new MemoryStream();
				using (var image = SKImage.FromBitmap(bmp))
				{
					using var data = image.Encode(SKEncodedImageFormat.Png, 100);
					data.SaveTo(ms);
				}
				ms.Position = 0; // Reset the position of the stream to the beginning
				return ms;
			}
			catch (Exception memoryStreamException)
			{
				throw new InternalServerErrorException(ServiceName.MemoryStreamService, memoryStreamException);
			}
		}
	}
}