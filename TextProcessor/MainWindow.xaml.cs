using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using TextProcessor.Interfaces;

namespace TextProcessor.Views
{
	public partial class MainWindow : Window
	{
		private readonly IFileReader _fileReader;

		public MainWindow(IFileReader fileReader)
		{
			InitializeComponent();
			_fileReader = fileReader;
		}

		private void OnSelectFileClick(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
				Title = "Select a text file to process"
			};

			bool? result = dialog.ShowDialog();

			if (result == true) // user clicked OK
			{
				ProcessFile(dialog.FileName);
			}
		}

		private async void ProcessFile(string filePath)
		{
			OutputTextBox.Text = string.Empty;
			try
			{
				await foreach (var line in _fileReader.ReadLinesAsync(filePath))
				{
					OutputTextBox.AppendText(line + "\n");
				}

				MessageBox.Show(
					$"Counted the words in the file",
					"Success",
					MessageBoxButton.OK,
					MessageBoxImage.Information
					);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"Failed to read file:\n{ex.Message}",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
					);
			}
		}
	}
}
