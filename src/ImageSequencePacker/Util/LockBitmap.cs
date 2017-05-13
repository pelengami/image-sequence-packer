//https://www.codeproject.com/Tips/240428/Work-with-bitmap-faster-with-Csharp

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageSequencePacker.Util
{
	internal sealed class LockBitmap
	{
		private readonly Bitmap _source;
		private IntPtr _iptr = IntPtr.Zero;
		private BitmapData _bitmapData;

		public LockBitmap(Bitmap source)
		{
			_source = source;
		}

		public byte[] Pixels { get; set; }
		public int Depth { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public void LockBits()
		{
			try
			{
				Width = _source.Width;
				Height = _source.Height;

				int pixelCount = Width * Height;

				var rect = new Rectangle(0, 0, Width, Height);

				Depth = Image.GetPixelFormatSize(_source.PixelFormat);

				if (Depth != 8 && Depth != 24 && Depth != 32)
					throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");

				_bitmapData = _source.LockBits(rect, ImageLockMode.ReadWrite,
					_source.PixelFormat);

				int step = Depth / 8;
				Pixels = new byte[pixelCount * step];
				_iptr = _bitmapData.Scan0;

				Marshal.Copy(_iptr, Pixels, 0, Pixels.Length);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void UnlockBits()
		{
			try
			{
				Marshal.Copy(Pixels, 0, _iptr, Pixels.Length);

				_source.UnlockBits(_bitmapData);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Color GetPixel(int x, int y)
		{
			var clr = Color.Empty;
			int cCount = Depth / 8;

			int i = ((y * Width) + x) * cCount;

			if (i > Pixels.Length - cCount)
				throw new IndexOutOfRangeException();

			if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
			{
				byte b = Pixels[i];
				byte g = Pixels[i + 1];
				byte r = Pixels[i + 2];
				byte a = Pixels[i + 3]; // a
				clr = Color.FromArgb(a, r, g, b);
			}
			if (Depth == 24) // For 24 bpp get Red, Green and Blue
			{
				byte b = Pixels[i];
				byte g = Pixels[i + 1];
				byte r = Pixels[i + 2];
				clr = Color.FromArgb(r, g, b);
			}
			if (Depth == 8)
			// For 8 bpp get color value (Red, Green and Blue values are the same)
			{
				byte c = Pixels[i];
				clr = Color.FromArgb(c, c, c);
			}
			return clr;
		}

		public void SetPixel(int x, int y, Color color)
		{
			int cCount = Depth / 8;

			int i = ((y * Width) + x) * cCount;

			if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
			{
				Pixels[i] = color.B;
				Pixels[i + 1] = color.G;
				Pixels[i + 2] = color.R;
				Pixels[i + 3] = color.A;
			}
			if (Depth == 24) // For 24 bpp set Red, Green and Blue
			{
				Pixels[i] = color.B;
				Pixels[i + 1] = color.G;
				Pixels[i + 2] = color.R;
			}
			if (Depth == 8)
			// For 8 bpp set color value (Red, Green and Blue values are the same)
			{
				Pixels[i] = color.B;
			}
		}
	}
}
