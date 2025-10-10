namespace TextProcessor.Interfaces
{
	public interface IFileReader
	{
		IAsyncEnumerable<string> ReadLinesAsync(string filePath, CancellationToken token = default);
	}
}
