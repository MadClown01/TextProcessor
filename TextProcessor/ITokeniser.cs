namespace TextProcessor.Interfaces
{
	public interface ITokeniser
	{
		IEnumerable<string> TokeniseLine(string line);
	}
}
