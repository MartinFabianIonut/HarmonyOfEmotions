using HarmonyOfEmotions.ServiceDefaults.Utils.Spectrogram;
using MP3Sharp;
using NAudio.Wave;
using System.Drawing;
using System.Drawing.Imaging;

namespace HarmonyOfEmotions.ServiceDefaults.Utils
{
	public static class AudioUtils
	{
		private static double[] ReadMp3FromBytes(byte[] mp3Bytes)
		{
			using var mp3Stream = new MemoryStream(mp3Bytes);
			using var mp3Reader = new Mp3FileReader(mp3Stream);
			var sampleProvider = mp3Reader.ToSampleProvider();
			var buffer = new List<double>();

			var readBuffer = new float[mp3Reader.WaveFormat.SampleRate * mp3Reader.WaveFormat.Channels];
			int samplesRead;

			do
			{
				samplesRead = sampleProvider.Read(readBuffer, 0, readBuffer.Length);
				for (int i = 0; i < samplesRead; i++)
				{
					buffer.Add(readBuffer[i]);
				}
			} while (samplesRead > 0);

			return buffer.ToArray();
		}

		public static double[] ReadMP3(string filePath, int bufferSize = 4096)
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

		public static Bitmap CreateGraySpetrogram(byte[] mp3Bytes)
		{
			//var audio = ReadMp3FromBytes(mp3Bytes);
			var audio = ReadMP3("audio-wow.mp3");
			// delete file
			File.Delete("audio-wow.mp3");
			int sampleRate = 44100;

			int fftSize = 1024;
			int targetWidthPx = 775;
			int stepSize = audio.Length / targetWidthPx;

			var spectogramGenerator = new SpectrogramGenerator(sampleRate, fftSize, stepSize, maxFreq: sampleRate / 2);
			spectogramGenerator.Add(audio);
			var bmp = spectogramGenerator.GetBitmap();
			return bmp;
		}

		public static MemoryStream GetMemoryStreamFromSpetrogram(Bitmap bmp)
		{
			var ms = new MemoryStream();
			bmp.Save(ms, ImageFormat.Png);
			return ms;
		}
	}
}