using System.Windows;

namespace TextProcessor.Views
{
    public partial class ProcessingDialog : Window
    {
        private readonly CancellationTokenSource _cts = new();

        public CancellationToken CancellationToken => _cts.Token;
        public IProgress<double> Progress { get; }

        public ProcessingDialog()
        {
            InitializeComponent();
            Progress = new Progress<double>(value => ProgressBar.Value = value);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();
        }
    }
}
