using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
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

		private async void OnSelectFileClick(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
				Title = "Select a text file to process"
			};

			bool? result = dialog.ShowDialog();

			if (result == true) // user clicked OK
			{
				await ProcessFileAsync(dialog.FileName);
			}
		}

		private async Task ProcessFileAsync(string filePath)
		{
			var dialog = new ProcessingDialog { Owner = this };
			dialog.Show();
			try
			{
				var counts = await _fileProcessor.ProcessFileAsync(
					filePath,
					dialog,
					dialog.CancellationToken
					);

				await PrintCountsAsync(counts, dialog.CancellationToken);

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

		private async Task PrintCountsAsync(
			IReadOnlyDictionary<string, int> counts,
			CancellationToken token)
		{
			// Build the list in a background thread
			var list = await Task.Run(() =>
			{
				var tempList = new List<WordCountItem>();
				foreach (var kvp in counts)
				{
					token.ThrowIfCancellationRequested();
					tempList.Add(new WordCountItem {
						Word = kvp.Key,
						Count = kvp.Value
						});
				}
				return tempList;
			}, token);

			ResultsDataGrid.ItemsSource = list;

			var view = CollectionViewSource.GetDefaultView(ResultsDataGrid.ItemsSource);
			if (view != null)
			{
				view.SortDescriptions.Clear();
				view.SortDescriptions.Add(
					new SortDescription(
						nameof(WordCountItem.Count),
						ListSortDirection.Descending
						)
					);
			}
			ResultsDataGrid.Items.Refresh();
		}

	}
	public class WordCountItem
	{
		public string Word { get; set; } = string.Empty;
		public int Count { get; set; }
	}
}
