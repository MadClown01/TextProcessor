namespace TextProcessor.Interfaces
{
	public interface ITokenizer
	{
		IEnumerable<string> TokenizeLine(string line);
	}
}
