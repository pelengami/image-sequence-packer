using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageSequencePacker.Util
{
	internal sealed class BitmapUitl
	{
		public static Bitmap CropBitmap(Bitmap bitmap, Rectangle cropSize)
		{
			var target = new Bitmap(cropSize.Width, cropSize.Height);

			using (var graphics = Graphics.FromImage(target))
			{
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(bitmap, new Rectangle(0, 0, target.Width, target.Height),
					cropSize, GraphicsUnit.Pixel);
			}

			return target;
		}

		public static Bitmap ResizeBitmap(Bitmap bitmapToResize, int width, int height)
		{
			var bitmap = new Bitmap(width, height);
			using (var graphics = Graphics.FromImage((Image)bitmap))
			{
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(bitmapToResize, 0, 0, width, height);
			}
			return bitmap;
		}
	}
}
