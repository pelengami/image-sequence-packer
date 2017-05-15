using System;
using System.Drawing;
using System.Windows;

namespace ImageSequencePacker.Util
{
	internal sealed class BitmapReaderWriter
	{
		public void Write(string filePath, Bitmap bitmap)
		{
			try
			{
				bitmap.Save(filePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public Bitmap Read(string filePath)
		{
			try
			{
				var bitmap = (Bitmap)Image.FromFile(filePath);
				return bitmap;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
	}
}
