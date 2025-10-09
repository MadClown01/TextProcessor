using System.Windows;
using TextProcessor.Services;
using TextProcessorViews;

namespace TextProcessor
{
	public partial class App : Application
	{
		private void ApplicationStartup(object sender, StartupEventArgs args)
		{
			ITextProcessingService service = new TextProcessingService();
			MainWindow window = new MainWindow(service);
			window.Show();
		}
	}
}
