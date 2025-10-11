using System.Windows;
using TextProcessor.Interfaces;

namespace TextProcessor.Views
{
	public partial class ProcessingDialog : Window, IProgressReporter
	{
		private readonly CancellationTokenSource _cts = new();
		public CancellationToken CancellationToken => _cts.Token;
		public IProgress<double> Progress { get; }

		public ProcessingDialog()
		{
			InitializeComponent();
			Progress = new Progress<double>(value => ProgressBar.Value = value);
		}

		public void Report(long bytesRead, long totalBytes)
		{
			double percent = totalBytes == 0 ? 0 : (bytesRead / (double)totalBytes) * 100;

			ProgressBar.Value = percent;
			StatusText.Text = $"{bytesRead:N0} / {totalBytes:N0} bytes processed ({percent:F1}%)";
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
