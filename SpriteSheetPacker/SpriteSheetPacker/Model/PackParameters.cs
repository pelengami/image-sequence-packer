using System.Collections.Generic;
using System.Windows;

namespace SpriteSheetPacker.Model
{
	internal sealed class PackParameters
	{
		public PackParameters(int columnsCount, int rowsCount, int alphaTreshold, int padding,
			Size outputTextureSize, List<string> imagesPath,
			bool isAutoSizeEnabled, int cropSizeWidth, int cropSizeHeight)
		{
			ColumnsCount = columnsCount;
			RowsCount = rowsCount;
			AlphaTreshold = alphaTreshold;
			Padding = padding;
			OutputTextureSize = outputTextureSize;
			ImagesPath = imagesPath;
			IsAutoSizeEnabled = isAutoSizeEnabled;
			CropSizeWidth = cropSizeWidth;
			CropSizeHeight = cropSizeHeight;
		}

		public List<string> ImagesPath { get; }
		public int ColumnsCount { get; }
		public int RowsCount { get; }
		public int AlphaTreshold { get; }
		public int Padding { get; }
		public bool IsBlackTrimmingEnabled { get; set; }
		public Size OutputTextureSize { get; }
		public bool IsAutoSizeEnabled { get; }
		public int CropSizeWidth { get; }
		public int CropSizeHeight { get; }

	}
}
