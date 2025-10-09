namespace TextProcessor.Interfaces
{
	public interface IWordCounter
	{
		void CountWords(IEnumerable<string> tokens);
		IReadOnlyDictionary<string, int> GetCounts();
	}
}
