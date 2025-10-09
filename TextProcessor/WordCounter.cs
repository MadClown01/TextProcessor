using TextProcessor.Interfaces;

namespace TextProcessor.Services
{
	public class WordCounter : IWordCounter
	{
		private Dictionary<string, int> _wordCounts = new Dictionary<string, int>();
		public void CountWords(IEnumerable<string> tokens)
		{
			foreach (var token in tokens)
			{
				_wordCounts[token] = _wordCounts.GetValueOrDefault(token) + 1; // avoids branching, slightly faster than GetValue
			}
		}
		public IReadOnlyDictionary<string, int> GetCounts()
		{
			return _wordCounts;
		}
	}
}
