// This code includes portions from a project by Scott W Harden.
// Used under the MIT License.

// MIT License
// 
// Copyright (c) 2019 Scott W Harden
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.


using FftSharp;
using SkiaSharp;

namespace HarmonyOfEmotions.ServiceDefaults.Utils.Spectrogram
{
	/// <summary>
	/// Instantiate a spectrogram generator.
	/// This module calculates the FFT over a moving window as data comes in.
	/// Using the Add() method to load new data and process it as it arrives.
	/// </summary>
	/// <param name="sampleRate">Number of samples per second (Hz)</param>
	/// <param name="fftSize">Number of samples to use for each FFT operation. This value must be a power of 2.</param>
	/// <param name="stepSize">Number of samples to step forward</param>
	/// <param name="minFreq">Frequency data lower than this value (Hz) will not be stored</param>
	/// <param name="maxFreq">Frequency data higher than this value (Hz) will not be stored</param>
	/// <param name="offsetHz">This value will be added to displayed frequency axis tick labels</param>
	public class SpectrogramGenerator(
		int sampleRate,
		int fftSize,
		int stepSize,
		double minFreq = 0,
		double maxFreq = double.PositiveInfinity,
		int offsetHz = 0)
	{
		/// <summary>
		/// Number of FFTs that remain to be processed for data which has been added but not yet analuyzed
		/// </summary>
		public int FftsToProcess { get => (_unprocessedData.Count - _settings.FftSize) / _settings.StepSize; }

		/// <summary>
		/// Total number of FFT steps processed
		/// </summary>
		public int FftsProcessed { get; private set; }

		/// <summary>
		/// This module contains detailed FFT/Spectrogram settings
		/// </summary>
		private readonly Settings _settings = new(sampleRate, fftSize, stepSize, minFreq, maxFreq, offsetHz);

		/// <summary>
		/// This is the list of FFTs which is translated to the spectrogram image when it is requested.
		/// The length of this list is the spectrogram width.
		/// The length of the arrays in this list is the spectrogram height.
		/// </summary>
		private readonly List<double[]> _fFTs = [];

		/// <summary>
		/// This list contains data values which have not yet been processed.
		/// Process() processes all unprocessed data.
		/// This list may not be empty after processing if there aren't enough values to fill a full FFT (FftSize).
		/// </summary>
		private readonly List<double> _unprocessedData = [];


		/// <summary>
		/// Load new data into the spectrogram generator
		/// </summary>
		public void Add(IEnumerable<double> audio, bool process = true)
		{
			_unprocessedData.AddRange(audio);
			if (process)
				Process();
		}

		/// <summary>
		/// Perform FFT analysis on all unprocessed data
		/// </summary>
		public double[][]? Process()
		{
			if (FftsToProcess < 1)
				return null;

			int newFftCount = FftsToProcess;
			double[][] newFfts = new double[newFftCount][];

			Parallel.For(0, newFftCount, newFftIndex =>
			{
				var buffer = new Complex[_settings.FftSize];
				int sourceIndex = newFftIndex * _settings.StepSize;
				for (int i = 0; i < _settings.FftSize; i++)
					buffer[i].Real = _unprocessedData[sourceIndex + i] * _settings.Window[i];

				Transform.FFT(buffer);

				newFfts[newFftIndex] = new double[_settings.Height];
				for (int i = 0; i < _settings.Height; i++)
					newFfts[newFftIndex][i] = buffer[_settings.FftIndex1 + i].Magnitude / _settings.FftSize;
			});

			foreach (var newFft in newFfts)
				_fFTs.Add(newFft);
			FftsProcessed += newFfts.Length;

			_unprocessedData.RemoveRange(0, newFftCount * _settings.StepSize);

			return newFfts;
		}

		public SKBitmap GetBitmap()
		{
			if (_fFTs.Count == 0)
				throw new ArgumentException("Not enough data in FFTs to generate an image yet.");

			int width = _fFTs.Count;
			int height = _fFTs[0].Length;

			var bitmap = new SKBitmap(width, height, SKColorType.Gray8, SKAlphaType.Opaque);
			var pixels = new byte[width * height];

			Parallel.For(0, width, col =>
			{
				int sourceCol = col;
				for (int row = 0; row < height; row++)
				{
					double value = _fFTs[sourceCol][row];
					value = 20 * Math.Log10(value + 1);
					value *= 3;
					value = Math.Min(value, 255);

					int bytePosition = (height - 1 - row) * width + col;
					pixels[bytePosition] = (byte)value;
				}
			});

			using (var pixmap = bitmap.PeekPixels())
			{
				if (pixmap != null)
				{
					var pixelSpan = pixmap.GetPixelSpan<byte>();
					pixels.CopyTo(pixelSpan);
				}
			}

			return bitmap;
		}

		/// <summary>
		/// Generate the spectrogram and save it as an image file.
		/// </summary>
		/// <param name="fileName">Path of the file to save.</param>
		public void SaveImage(string fileName)
		{
			var bitmap = GetBitmap();
			using var image = SKImage.FromBitmap(bitmap);
			using var data = image.Encode(SKEncodedImageFormat.Png, 100);
			using var stream = File.OpenWrite(fileName);
			data.SaveTo(stream);
		}
	}
}
