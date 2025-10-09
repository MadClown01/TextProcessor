using System.Windows;
using TextProcessor.Interfaces;
using TextProcessor.Services;
using TextProcessor.Views;

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
