using Microsoft.Win32;
using System.Windows;
using TextProcessor.Interfaces;

namespace TextProcessor.Views
{
	public partial class MainWindow : Window
	{
		private readonly IFileReader _fileReader;

		public MainWindow(IFileReader fileReader)
		{
			InitializeComponent();
			_fileReader = fileReader;
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
				OutputTextBox.Text += _fileReader.ReadLinesAsync(selectedFilePath);
			}
		}
	}
}
