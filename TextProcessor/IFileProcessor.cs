namespace TextProcessor.Interfaces
{
	public interface IFileProcessor
	{
		public Task<(IReadOnlyDictionary<string, int>, Double)> ProcessFileAsync(
			string filePath,
			IProgress<(long, long)> progress,
			CancellationToken token
		);
	}
}
