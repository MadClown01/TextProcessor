namespace TextProcessor.Interfaces
{
	public interface IFileReader
	{
		IAsyncEnumerable<(string line, long bytesRead)> ReadLinesAsync(
			string filePath,
			CancellationToken token = default);
	}
}
