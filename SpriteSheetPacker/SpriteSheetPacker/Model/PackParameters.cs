using System.Collections.Generic;
using System.Windows;

namespace SpriteSheetPacker.Model
{
	internal sealed class PackParameters
	{
		public PackParameters(int columnsCount, int rowsCount, int alphaTreshold, int padding, Size outputTextureSize, List<string> imagesPath)
		{
			ColumnsCount = columnsCount;
			RowsCount = rowsCount;
			AlphaTreshold = alphaTreshold;
			Padding = padding;
			OutputTextureSize = outputTextureSize;
			ImagesPath = imagesPath;
		}

		public List<string> ImagesPath { get; }
		public int ColumnsCount { get; }
		public int RowsCount { get; }
		public int AlphaTreshold { get; }
		public int Padding { get; }
		public bool IsBlackTrimmingEnabled { get; set; }
		public Size OutputTextureSize { get; }
	}
}
