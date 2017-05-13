using System.Drawing;
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
				graphics.DrawImage(bitmap, new Rectangle(0, 0, target.Width, target.Height),
					cropSize,
					GraphicsUnit.Pixel);
			}

			return target;
		}

		public static Bitmap CreateResizedBitmap(ImageSource source, int width, int height)
		{
			var rect = new Rect(0, 0, width, height);

			var drawingVisual = new DrawingVisual();
			using (var drawingContext = drawingVisual.RenderOpen())
				drawingContext.DrawImage(source, rect);

			var resizedImage = new RenderTargetBitmap((int)rect.Width, (int)rect.Height,
				96, 96,
				PixelFormats.Default);
			resizedImage.Render(drawingVisual);

			Bitmap bitmap;
			using (var stream = new MemoryStream())
			{
				BitmapEncoder encoder = new BmpBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(resizedImage));
				encoder.Save(stream);
				bitmap = new Bitmap(stream);
			}

			return bitmap;
		}
	}
}
