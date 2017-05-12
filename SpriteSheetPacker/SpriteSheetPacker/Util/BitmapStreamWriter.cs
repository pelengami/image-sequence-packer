using System.IO;
using System.Windows.Media.Imaging;

namespace SpriteSheetPacker.Util
{
	internal sealed class BitmapStreamWriter
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
	}
}
