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
			// Step 1: Show file open dialog
			var dialog = new OpenFileDialog
			{
				Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
				Title = "Select a text file to process"
			};

			bool? result = dialog.ShowDialog();

			if (result != true) // user didn't select a file
			{
				return;
			}

			string filePath = dialog.FileName;

			// Step 2: Show processing dialog
			var processingDialog = new ProcessingDialog(_fileProcessor, filePath)
			{
				Owner = this
			};
			bool? processed = processingDialog.ShowDialog(); // modal

			if (processed == true)
			{
				await DisplayCountsAsync(processingDialog.Counts, CancellationToken.None);
				MessageBox.Show(
					$"Counted occurrences for {processingDialog.Counts.Count} unique words.",
					"Success",
					MessageBoxButton.OK,
					MessageBoxImage.Information
					);
			}
		}

		private async Task DisplayCountsAsync(
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
					tempList.Add(
						new WordCountItem {
							Word = kvp.Key,
							Count = kvp.Value
							}
						);
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
		}

	}
	public class WordCountItem
	{
		public string Word { get; set; } = string.Empty;
		public int Count { get; set; }
	}
}
