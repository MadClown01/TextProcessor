using System.IO;
using System.Windows;
using TextProcessor.Interfaces;

namespace TextProcessor.Views
{
	public partial class ProcessingDialog : Window
	{
		private readonly CancellationTokenSource _cts = new();
		private readonly IFileProcessor _fileProcessor;
		private readonly string _filePath;
		public IReadOnlyDictionary<string, int> Counts { get; private set; } = new Dictionary<string, int>();
		public double TotalElapsed { get; private set; } = 0.0;

		public ProcessingDialog(IFileProcessor fileProcessor, string filePath)
		{
			_fileProcessor = fileProcessor;
			_filePath = filePath;
			InitializeComponent();
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			_ = ProcessAsync();
		}

		private async Task ProcessAsync()
		{
			if (!File.Exists(_filePath))
			{
				MessageBox.Show(
					$"File {_filePath} no longer exists or is inaccessible.",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
				return;
			}

			var progress = new Progress<(long bytesRead, long bytesTotal)>(p =>
			{
				double percent = p.bytesTotal > 0 ? (p.bytesRead / (double)p.bytesTotal) * 100 : 0;
				ProgressBar.Value = percent;

				string displayBytesRead = FormatBytes(p.bytesRead);
				string displayBytesTotal = FormatBytes(p.bytesTotal);
				StatusText.Text = $"{displayBytesRead} / {displayBytesTotal} processed ({percent:F1}%)";
			});

			try
			{
				// No background thread call needed here because ProcessFileAsync doesn't block
				(Counts, var totalElapsed) = await _fileProcessor.ProcessFileAsync(
					_filePath,
					progress,
					_cts.Token
				);

				if (IsLoaded)
				{
					// Only set DialogResult if the window is still open
					DialogResult = true;
				}
			}
			catch (OperationCanceledException)
			{
				if (IsLoaded)
				{
					// Only set DialogResult if the window is still open
					DialogResult = false;
				}

				MessageBox.Show(
					"Processing canceled.",
					"Canceled",
					MessageBoxButton.OK,
					MessageBoxImage.Information
				);
			}
			catch (Exception ex)
			{
				if (IsLoaded)
				{
					// Only set DialogResult if the window is still open
					DialogResult = false;
				}

				MessageBox.Show(
					$"Failed to read file:\n{ex.Message}",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			_cts.Cancel();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			_cts.Dispose();
			Loaded -= OnLoaded;
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			// If the user clicked X, cancel processing
			if (!_cts.IsCancellationRequested)
			{
				_cts.Cancel();
			}

			base.OnClosing(e);
		}

		private string FormatBytes(long bytes)
		{
			if (bytes == 0) return "0 B";
			string[] sizes = { "B", "KB", "MB", "GB", "TB" };
			int order = (int)Math.Floor(Math.Log(bytes, 1024));
			double len = bytes / Math.Pow(1024, order);
			return $"{len:F2} {sizes[order]}";
		}
	}
}
