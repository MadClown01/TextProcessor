using Microsoft.Win32;
using System.Windows;
using TextProcessor.Interfaces;
using TextProcessor.Services;

namespace TextProcessor.Views
{
	public partial class MainWindow : Window
	{
		private readonly IFileReader _fileReader;
		private readonly ITokeniser _tokeniser;
		private readonly IWordCounter _wordCounter;

		public MainWindow(IFileReader fileReader, ITokeniser tokeniser, IWordCounter wordCounter)
		{
			_fileReader = fileReader;
			_tokeniser = tokeniser;
			_wordCounter = wordCounter;

			InitializeComponent();
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
					var words = _tokeniser.TokeniseLine(line);
					_wordCounter.CountWords(words);
				}

				foreach (var kvp in _wordCounter.GetCounts())
				{
					OutputTextBox.AppendText($"{kvp.Key}: {kvp.Value}\n");
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
