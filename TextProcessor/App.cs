using System.Windows;
using TextProcessor.Services;
using TextProcessorViews;

namespace TextProcessor
{
	public partial class App : Application
	{
		private void ApplicationStartup(object sender, StartupEventArgs args)
		{
			IFileReader service = new FileReader();
			MainWindow window = new MainWindow(service);
			window.Show();
		}
	}
}
