using TextProcessor.Interfaces;

namespace TextProcessor.Services
{
	public class Tokenizer : ITokenizer
	{
		public IEnumerable<string> TokenizeLine(string line)
		{
			foreach (var word in line.Split(
				(char[])null, // null as char[] separator means split on any whitespace
				StringSplitOptions.RemoveEmptyEntries
				))
			{
				yield return word;
			}
		}
	}
}
