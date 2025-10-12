using System.IO;
using System.Windows;
using TextProcessor.Interfaces;

namespace TextProcessor.Views
{
	public partial class ProcessingDialog : Window, IProgressReporter
	{
		private readonly CancellationTokenSource _cts = new();
		private readonly IFileProcessor _fileProcessor;
		private readonly string _filePath;
		public IReadOnlyDictionary<string, int> Counts { get; private set; } = new Dictionary<string, int>();

		public ProcessingDialog(IFileProcessor fileProcessor, string filePath)
		{
			_fileProcessor = fileProcessor;
			_filePath = filePath;
			InitializeComponent();
			Loaded += OnLoaded;
		}

		public void Report(long bytesRead, long totalBytes)
		{
			double percent = totalBytes > 0 ? (bytesRead / (double)totalBytes) * 100 : 0;

			// Update UI on the main thread
			Dispatcher.BeginInvoke(() =>
			{
				ProgressBar.Value = percent;
				StatusText.Text = $"{bytesRead:N0} / {totalBytes:N0} bytes processed ({percent:F1}%)";
			});
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
			
			try
			{
				// No background thread call needed here because ProcessFileAsync doesn't block
				Counts = await _fileProcessor.ProcessFileAsync(
					_filePath,
					this,
					_cts.Token
				);
				
				DialogResult = true;
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
	}
}
