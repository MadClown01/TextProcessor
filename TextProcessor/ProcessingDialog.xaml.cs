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
		public CancellationToken CancellationToken => _cts.Token;
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
			double percent = 0;
			if (totalBytes > 0)
			{
				percent = (bytesRead / (double)totalBytes) * 100;
			}

			// Update UI on the main thread
			Dispatcher.BeginInvoke(() =>
			{
				ProgressBar.Value = percent;
				StatusText.Text = $"{bytesRead:N0} / {totalBytes:N0} bytes processed ({percent:F1}%)";
			});
		}

		private async void OnLoaded(object sender, RoutedEventArgs e)
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
				Counts = await _fileProcessor.ProcessFileAsync( // No background thread here, ProcessFileAsync doesn't block
					_filePath,
					this,
					_cts.Token
				);

				if (!IsLoaded) return; // User already closed window
				DialogResult = true;
			}
			catch (OperationCanceledException)
			{
				MessageBox.Show(
					"Processing canceled.",
					"Canceled",
					MessageBoxButton.OK,
					MessageBoxImage.Information
				);

				if (!IsLoaded) return; // User already closed window
				DialogResult = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"Failed to read file:\n{ex.Message}",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);

				if (!IsLoaded) return; // User already closed window
				DialogResult = false;
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
