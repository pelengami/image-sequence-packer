using Microsoft.Win32;

namespace ImageSequencePacker.Service
{
	internal sealed class SaveFileDialogService
	{
		public bool SaveFileDialog(out string fileName)
		{
			fileName = string.Empty;

			var saveFileDialog = new SaveFileDialog
			{
				Title = "Save Texture",
				Filter = "png files (*.png)|*.png|All files (*.*)|*.*",
				FilterIndex = 1,
				AddExtension = true,
				RestoreDirectory = true
			};

			var result = saveFileDialog.ShowDialog();

			if (!result.HasValue)
				return false;

			if (result.Value)
				fileName = saveFileDialog.FileName;

			return result.Value;
		}
	}
}
