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
			// For simplicity, services are manually wired here.
			// In a more complex application, Microsoft.Extensions.DependencyInjection could be used.

			IFileReader fileReader = new FileReader();
			ITokenizer tokeniser = new Tokenizer();
			IFileProcessor fileProcessor = new FileProcessor(fileReader, tokeniser);
			MainWindow window = new MainWindow(fileProcessor);
			window.Show();
		}
	}
}
