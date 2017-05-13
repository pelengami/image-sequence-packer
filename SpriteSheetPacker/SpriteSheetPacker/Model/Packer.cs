using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using SpriteSheetPacker.Extensions;
using SpriteSheetPacker.Util;

namespace SpriteSheetPacker.Model
{
	internal sealed class Packer : IPacker
	{
		public async Task<Bitmap> PackAsync(PackParameters packParameters)
		{
			return await Task.Run(() => PackSync(packParameters));
		}

		private Bitmap PackSync(PackParameters packParameters)
		{
			try
			{
				var originalBitmaps = new List<Bitmap>();

				foreach (var path in packParameters.ImagesPath)
				{
					var bitmap = (Bitmap)Image.FromFile(path);
					originalBitmaps.Add(bitmap);
				}

				if (!originalBitmaps.Any())
					return null;

				Rectangle avgCropSize;

				if (packParameters.IsAutoSizeEnabled)
					avgCropSize = CalcAvgCropSizeAlpha(originalBitmaps, packParameters.AlphaTreshold);
				else
				{
					var firstBitmap = originalBitmaps[0];

					avgCropSize = new Rectangle(firstBitmap.Width / 2 - packParameters.CropSizeWidth / 2,
						firstBitmap.Height / 2 - packParameters.CropSizeHeight / 2,
						packParameters.CropSizeWidth,
						packParameters.CropSizeHeight);
				}

				var eachTextureWidth = (int)packParameters.OutputTextureSize.Width / packParameters.ColumnsCount - packParameters.Padding * 2;
				var eachTextureHeight = (int)packParameters.OutputTextureSize.Height / packParameters.RowsCount - packParameters.Padding * 2;

				var resizedBitmaps = new List<Bitmap>();

				foreach (var bitmap in originalBitmaps)
				{
					var cropedBitmap = CropBitmap(bitmap, avgCropSize);
					var resizedBitmap = ImageResizer.CreateResizedBitmap(cropedBitmap.ToBitmapSource(), eachTextureWidth, eachTextureHeight);
					resizedBitmaps.Add(resizedBitmap);
				}

				var packedBitmap = Pack(resizedBitmaps, packParameters);

				return packedBitmap;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		private static Bitmap Pack(IEnumerable<Bitmap> bitmaps, PackParameters packParameters)
		{
			var packedBitmap = new Bitmap((int)packParameters.OutputTextureSize.Width, (int)packParameters.OutputTextureSize.Height);
			var lockPackedBitmap = new LockBitmap(packedBitmap);
			lockPackedBitmap.LockBits();

			var hOffset = packParameters.Padding;
			var vOffset = packParameters.Padding;

			foreach (var bitmap in bitmaps)
			{
				var lockBitmap = new LockBitmap(bitmap);
				lockBitmap.LockBits();

				for (int j = 0; j < bitmap.Height; j++)
				{
					for (int i = 0; i < bitmap.Width; i++)
					{
						var pixel = lockBitmap.GetPixel(i, j);
						lockPackedBitmap.SetPixel(hOffset + i, vOffset + j, pixel);
					}
				}

				lockBitmap.UnlockBits();

				hOffset += bitmap.Width + packParameters.Padding * 2;

				if (hOffset + bitmap.Width + packParameters.Padding > (int)packParameters.OutputTextureSize.Width)
				{
					hOffset = packParameters.Padding;
					vOffset += bitmap.Height + packParameters.Padding * 2;
				}
			}

			lockPackedBitmap.UnlockBits();

			return packedBitmap;
		}

		private Bitmap CropBitmap(Bitmap bitmap, Rectangle cropSize)
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

		private static Rectangle CalcAvgCropSizeAlpha(List<Bitmap> bitmaps, int alphaThreshold)
		{
			var minX = bitmaps[0].Width;
			var width = 0;

			var minY = bitmaps[0].Height;
			var height = 0;

			foreach (var bitmap in bitmaps)
			{
				var tempCropRc = CalcCropSizeAlpha(bitmap, alphaThreshold);

				minX = Math.Min(minX, tempCropRc.X);
				minY = Math.Min(minY, tempCropRc.Y);
				width = Math.Max(width, tempCropRc.Width);
				height = Math.Max(height, tempCropRc.Height);
			}

			return new Rectangle(minX, minY, width, height);
		}

		private static Rectangle CalcCropSizeAlpha(Bitmap bitmap, int alphaThreshold)
		{
			var minX = bitmap.Width;
			var maxX = 0;

			var minY = bitmap.Height;
			var maxY = 0;

			var lockBitmap = new LockBitmap(bitmap);
			lockBitmap.LockBits();

			for (int j = 0; j < lockBitmap.Height; j++)
			{
				for (int i = 0; i < lockBitmap.Width; i++)
				{
					if (lockBitmap.GetPixel(i, j).A >= alphaThreshold)
					{
						minX = Math.Min(minX, i);
						minY = Math.Min(minY, j);
						maxX = Math.Max(maxX, i);
						maxY = Math.Max(maxY, j);
					}
				}
			}

			lockBitmap.UnlockBits();

			return new Rectangle(minX, minY, maxX - minX, maxY - minY);
		}
	}
}