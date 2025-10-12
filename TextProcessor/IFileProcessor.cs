namespace TextProcessor.Interfaces
{
	public interface IFileProcessor
	{
		public Task<IReadOnlyDictionary<string, int>> ProcessFileAsync(
			string filePath,
			IProgress<(long, long)> progress,
			CancellationToken token
		);
	}
}
