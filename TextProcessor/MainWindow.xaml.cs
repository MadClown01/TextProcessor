using Microsoft.Win32;
using System.Windows;
using TextProcessor.Interfaces;

namespace TextProcessor.Views
{
	public partial class MainWindow : Window
	{
		private readonly IFileProcessor _fileProcessor;

		public MainWindow(IFileProcessor fileProcessor)
		{
			_fileProcessor = fileProcessor;
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
			var dialog = new ProcessingDialog { Owner = this };
			dialog.Show();

			OutputTextBox.Text = string.Empty;
			try
			{
				var counts = await _fileProcessor.ProcessFileAsync(
					filePath,
					dialog.Progress,
					dialog.CancellationToken
					);

				// need to make this async
				foreach (var kvp in counts)
				{
					OutputTextBox.AppendText($"{kvp.Key}: {kvp.Value}\n");
				}
				dialog.Close();

				MessageBox.Show(
					$"Counted occurrences for {counts.Count} unique words",
					"Success",
					MessageBoxButton.OK,
					MessageBoxImage.Information
					);
			}
			catch (OperationCanceledException)
			{
				dialog.Close();
				MessageBox.Show(
					"Processing canceled.",
					"Canceled",
					MessageBoxButton.OK,
					MessageBoxImage.Information
					);
			}
			catch (Exception ex)
			{
				dialog.Close();
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
