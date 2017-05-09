using System;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SpriteSheetPacker.Extensions
{
	internal static class BitmapExtensions
	{
		public static BitmapSource ToBitmapSource(this Bitmap bitmap)
		{
			var bitmapSourceFromHBitmap = Imaging.CreateBitmapSourceFromHBitmap(
				bitmap.GetHbitmap(),
				IntPtr.Zero,
				System.Windows.Int32Rect.Empty,
				BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));

			return bitmapSourceFromHBitmap;
		}
	}
}
