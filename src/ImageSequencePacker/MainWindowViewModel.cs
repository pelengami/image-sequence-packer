﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.CommandWpf;
using ImageSequencePacker.Extensions;
using ImageSequencePacker.Model;
using ImageSequencePacker.Service;
using ImageSequencePacker.Util;

namespace ImageSequencePacker
{
	public sealed class MainWindowViewModel : INotifyPropertyChanged
	{
		private const int DefaultColumnsCount = 2;
		private const int DefaultRowsCount = 2;
		private const int DefaultPadding = 20;
		private const bool DefaultIsCropAutoSizeEnabled = true;
		private const int DefaultAlphaThreshold = 100;
		private readonly Size _defaultSize = new Size(2048, 2048);

		private BitmapSource _packedImagePreview;
		private readonly IPacker _packer;
		private readonly SaveFileDialogService _saveFileDialogService;
		private bool _isPacking;

		public MainWindowViewModel()
		{
			ImagesPath = new ObservableCollection<string>();

			_saveFileDialogService = new SaveFileDialogService();

			_packer = new Packer();

			PackCommand = new RelayCommand(OnPackCommand);
			PreviewCommand = new RelayCommand(OnPreviewCommand);
			DropCommand = new RelayCommand<DragEventArgs>(OnDropCommand);

			RowsCount = DefaultRowsCount;
			ColumnsCount = DefaultColumnsCount;
			Padding = DefaultPadding;
			AlphaThreshold = DefaultAlphaThreshold;

			OutputTextureSizes = new List<Size>
			{
				new Size(512, 512),
				new Size(1024, 1024),
				new Size(2048, 2048),
				new Size(4096, 4096)
			};

			IsCropAutoSizeEnabled = DefaultIsCropAutoSizeEnabled;

			SelectedOutputTextureSize = _defaultSize;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ICommand PackCommand { get; set; }

		public ICommand PreviewCommand { get; set; }

		public ICommand DropCommand { get; set; }

		public int ColumnsCount { get; set; }

		public int RowsCount { get; set; }

		public int AlphaThreshold { get; set; }

		public int Padding { get; set; }

		public int CropSizeWidth { get; set; }

		public int CropSizeHeight { get; set; }

		public bool IsCropAutoSizeEnabled { get; set; }

		public List<Size> OutputTextureSizes { get; }

		public Size SelectedOutputTextureSize { get; set; }

		public BitmapSource PackedImagePreview
		{
			get => _packedImagePreview;
			set
			{
				if (Equals(value, _packedImagePreview))
					return;
				_packedImagePreview = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<string> ImagesPath { get; }

		public bool IsPacking
		{
			get => _isPacking;
			set
			{
				if (value == _isPacking)
					return;
				_isPacking = value;
				OnPropertyChanged();
			}
		}

		private async Task<bool> PackImages()
		{
			IsPacking = true;

			var packParameters = new PackParameters(ColumnsCount, RowsCount, AlphaThreshold, Padding, SelectedOutputTextureSize,
				ImagesPath.ToList(), IsCropAutoSizeEnabled, CropSizeWidth, CropSizeHeight);

			var packedImage = await _packer.PackAsync(packParameters);

			IsPacking = false;

			if (packedImage == null)
				return false;

			PackedImagePreview = packedImage.ToBitmapSource();

			return true;
		}

		private async void OnPackCommand()
		{
			if (!await PackImages())
				return;

			string saveFilePath;
			if (!_saveFileDialogService.SaveFileDialog(out saveFilePath))
				return;

			var bitmapWriter = new BitmapReaderWriter();
			bitmapWriter.Write(saveFilePath, PackedImagePreview);
		}

		private async void OnPreviewCommand()
		{
			await PackImages();
		}

		private void OnDropCommand(DragEventArgs obj)
		{
			if (!obj.Data.GetDataPresent(DataFormats.FileDrop))
				return;

			string[] files = (string[])obj.Data.GetData(DataFormats.FileDrop);
			if (files == null)
				return;

			foreach (var file in files)
			{
				var extension = System.IO.Path.GetExtension(file);
				if (extension == null)
					continue;

				if (extension.Equals(".png", StringComparison.CurrentCultureIgnoreCase) ||
					extension.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase))
					ImagesPath.Add(file);
			}
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
