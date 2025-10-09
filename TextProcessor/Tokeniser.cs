using TextProcessor.Interfaces;

namespace TextProcessor.Services
{
	public class Tokeniser : ITokeniser
	{
		public IEnumerable<string> TokeniseLine(string line)
		{
			foreach (var word in line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
			{
				yield return word;
			}
		}
	}
}
