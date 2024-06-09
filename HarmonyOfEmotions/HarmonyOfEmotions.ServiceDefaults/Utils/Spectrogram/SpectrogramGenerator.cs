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


using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using FftSharp;

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
		public int FftsToProcess { get => (UnprocessedData.Count - Settings.FftSize) / Settings.StepSize; }

		/// <summary>
		/// Total number of FFT steps processed
		/// </summary>
		public int FftsProcessed { get; private set; }

		/// <summary>
		/// This module contains detailed FFT/Spectrogram settings
		/// </summary>
		private readonly Settings Settings = new(sampleRate, fftSize, stepSize, minFreq, maxFreq, offsetHz);

		/// <summary>
		/// This is the list of FFTs which is translated to the spectrogram image when it is requested.
		/// The length of this list is the spectrogram width.
		/// The length of the arrays in this list is the spectrogram height.
		/// </summary>
		private readonly List<double[]> FFTs = new();

		/// <summary>
		/// This list contains data values which have not yet been processed.
		/// Process() processes all unprocessed data.
		/// This list may not be empty after processing if there aren't enough values to fill a full FFT (FftSize).
		/// </summary>
		private readonly List<double> UnprocessedData = [];


		/// <summary>
		/// Load new data into the spectrogram generator
		/// </summary>
		public void Add(IEnumerable<double> audio, bool process = true)
		{
			UnprocessedData.AddRange(audio);
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
				var buffer = new Complex[Settings.FftSize];
				int sourceIndex = newFftIndex * Settings.StepSize;
				for (int i = 0; i < Settings.FftSize; i++)
					buffer[i].Real = UnprocessedData[sourceIndex + i] * Settings.Window[i];

				Transform.FFT(buffer);

				newFfts[newFftIndex] = new double[Settings.Height];
				for (int i = 0; i < Settings.Height; i++)
					newFfts[newFftIndex][i] = buffer[Settings.FftIndex1 + i].Magnitude / Settings.FftSize;
			});

			foreach (var newFft in newFfts)
				FFTs.Add(newFft);
			FftsProcessed += newFfts.Length;

			UnprocessedData.RemoveRange(0, newFftCount * Settings.StepSize);

			return newFfts;
		}

		private int GetInt32(byte r, byte g, byte b) => (255 << 24) | (r << 16) | (g << 8) | b;
#pragma warning disable CA1416 // Validate platform compatibility
		public Bitmap GetBitmap()
		{
			if (FFTs.Count == 0)
				throw new ArgumentException("Not enough data in FFTs to generate an image yet.");

			int Width = FFTs.Count;
			int Height = FFTs[0].Length;
			Bitmap bmp = new(Width, Height, PixelFormat.Format8bppIndexed);

			ColorPalette pal = bmp.Palette;
			for (int i = 0; i < 256; i++)
				pal.Entries[i] = Color.FromArgb(GetInt32((byte)i, (byte)i, (byte)i));
			bmp.Palette = pal;

			Rectangle lockRect = new(0, 0, Width, Height);
			BitmapData bitmapData = bmp.LockBits(lockRect, ImageLockMode.ReadOnly, bmp.PixelFormat);
			int stride = bitmapData.Stride;

			byte[] bytes = new byte[bitmapData.Stride * bmp.Height];
			Parallel.For(0, Width, col =>
			{
				int sourceCol = col;

				for (int row = 0; row < Height; row++)
				{
					double value = FFTs[sourceCol][row];

					value = 20 * Math.Log10(value + 1);

					value *= 3;
					value = Math.Min(value, 255);
					int bytePosition = (Height - 1 - row) * stride + col;
					bytes[bytePosition] = (byte)value;
				}
			});

			Marshal.Copy(bytes, 0, bitmapData.Scan0, bytes.Length);
			bmp.UnlockBits(bitmapData);

			return bmp;
		}

		/// <summary>
		/// Generate the spectrogram and save it as an image file.
		/// </summary>
		/// <param name="fileName">Path of the file to save.</param>
		public void SaveImage(string fileName)
		{
			if (FFTs.Count == 0)
				throw new InvalidOperationException("Spectrogram contains no data. Use Add() to add signal data.");

			ImageFormat fmt = ImageFormat.Png;

			GetBitmap().Save(fileName, fmt);
		}
#pragma warning restore CA1416 // Validate platform compatibility
	}
}
