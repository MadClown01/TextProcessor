using System.Windows;

namespace TextProcessorViews
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Process_Click(object sender, RoutedEventArgs e)
		{
			// Temporary stub logic
			OutputTextBox.Text = new string(InputTextBox.Text);
		}

		private void Clear_Click(object sender, RoutedEventArgs e)
		{
			InputTextBox.Clear();
			OutputTextBox.Clear();
		}
	}
}