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
			IFileReader fileReader = new FileReader();
			ITokeniser tokeniser = new Tokeniser();
			IWordCounter wordCounter = new WordCounter();
			MainWindow window = new MainWindow(fileReader, tokeniser, wordCounter);
			window.Show();
		}
	}
}
