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
			Loaded += ProcessingDialog_Loaded;
		}

		public void Report(long bytesRead, long totalBytes)
		{
			double percent = 0;
			if (totalBytes > 0)
			{
				percent = (bytesRead / (double)totalBytes) * 100;
			}

			ProgressBar.Value = percent;
			StatusText.Text = $"{bytesRead:N0} / {totalBytes:N0} bytes processed ({percent:F1}%)";
		}

		private async void ProcessingDialog_Loaded(object sender, RoutedEventArgs e)
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
				Counts = await _fileProcessor.ProcessFileAsync(_filePath, this, _cts.Token);
				DialogResult = true;
			}
			catch (OperationCanceledException)
			{
				DialogResult = false;
				MessageBox.Show(
					"Processing canceled.",
					"Canceled",
					MessageBoxButton.OK,
					MessageBoxImage.Information
					);
			}
			catch (Exception ex)
			{
				DialogResult = false;
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
		}
	}
}
