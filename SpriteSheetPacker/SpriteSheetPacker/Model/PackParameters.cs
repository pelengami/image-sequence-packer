using System.Windows;

namespace SpriteSheetPacker.Model
{
	internal sealed class PackParameters
	{
		public PackParameters(int tilesX, int tilesY, int alphaTreshold, int padding, Size outputTextureSize)
		{
			TilesX = tilesX;
			TilesY = tilesY;
			AlphaTreshold = alphaTreshold;
			Padding = padding;
			OutputTextureSize = outputTextureSize;
		}

		public int TilesX { get; }
		public int TilesY { get; }
		public int AlphaTreshold { get; }
		public int Padding { get; }
		public Size OutputTextureSize { get; }
	}
}
