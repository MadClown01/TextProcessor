namespace TextProcessor.Interfaces
{
	public interface IFileProcessor
	{
		public Task<IReadOnlyDictionary<string, int>> ProcessFileAsync(
			string filePath,
			Action<long, long> reportProgress,
			CancellationToken token);
	}
}
