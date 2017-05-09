using System;
using System.Drawing;
using System.Threading.Tasks;
using SpriteSheetPacker.Service;

namespace SpriteSheetPacker.Model
{
	internal sealed class ForeignPacker : IPacker
	{
		private readonly ForeignFunctionCaller _foreignFunctionCaller;

		public ForeignPacker(ForeignFunctionCaller foreignFunctionCaller)
		{
			if (foreignFunctionCaller == null) throw new ArgumentNullException(nameof(foreignFunctionCaller));

			_foreignFunctionCaller = foreignFunctionCaller;
		}

		public async Task<Bitmap> PackAsync(PackParameters packParameters)
		{
			return await Task.Run(delegate
			{
				var bitmap = new Bitmap((int)packParameters.OutputTextureSize.Width,
					(int)packParameters.OutputTextureSize.Height);

				for (int i = 0; i < bitmap.Width; i++)
					for (int j = 0; j < bitmap.Height; j++)
						bitmap.SetPixel(i, j, Color.Black);

				return bitmap;
			});
		}
	}
}