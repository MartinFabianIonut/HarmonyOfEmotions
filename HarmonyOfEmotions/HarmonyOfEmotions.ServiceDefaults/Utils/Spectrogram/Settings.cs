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

namespace HarmonyOfEmotions.ServiceDefaults.Utils.Spectrogram
{
	public class Settings
	{
		// vertical information
		public readonly int FftSize;
		public readonly double FreqNyquist;
		public readonly double HzPerPixel;
		public readonly double PxPerHz;
		public readonly int FftIndex1;
		public readonly int FftIndex2;
		public readonly int Height;

		// horizontal information
		public readonly double[] Window;
		public readonly int StepSize;

		public Settings(int sampleRate, int fftSize, int stepSize, double minFreq, double maxFreq, int offsetHz)
		{
			// FFT info
			FftSize = fftSize;
			StepSize = stepSize;

			// vertical
			minFreq = Math.Max(minFreq, 0);
			FreqNyquist = sampleRate / 2;
			HzPerPixel = (double)sampleRate / fftSize;
			PxPerHz = (double)fftSize / sampleRate;
			FftIndex1 = (minFreq == 0) ? 0 : (int)(minFreq / HzPerPixel);
			FftIndex2 = (maxFreq >= FreqNyquist) ? fftSize / 2 : (int)(maxFreq / HzPerPixel);
			Height = FftIndex2 - FftIndex1;

			// horizontal
			var window = new FftSharp.Windows.Hanning();
			Window = window.Create(fftSize);
		}
	}
}
