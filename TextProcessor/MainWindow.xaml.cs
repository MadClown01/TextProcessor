using Microsoft.Win32;
using System.Windows;
using TextProcessor.Services;

namespace TextProcessorViews
{
	public partial class MainWindow : Window
	{
		private readonly ITextProcessingService _textService;

		public MainWindow(ITextProcessingService textService)
		{
			InitializeComponent();
			_textService = textService;
		}

		private void Process_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
				Title = "Select a text file to process"
			};

			bool? result = dialog.ShowDialog();

			if (result == true) // user clicked OK
			{
				string selectedFilePath = dialog.FileName;
				// pass this path to your TextProcessingService
			}
		}

		private void Clear_Click(object sender, RoutedEventArgs e)
		{
			OutputTextBox.Clear();
		}
	}
}
