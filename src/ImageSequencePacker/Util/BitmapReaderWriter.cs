using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageSequencePacker.Util
{
	internal sealed class BitmapReaderWriter
	{
		public void Write(string filePath, BitmapSource bitmapSource)
		{
			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				BitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
				encoder.Save(fileStream);
			}
		}

		public Bitmap Read(string filePath)
		{
			var bitmap = (Bitmap)Image.FromFile(filePath);
			return bitmap;
		}
	}
}
