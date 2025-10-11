namespace TextProcessor.Interfaces
{
	public interface IFileProcessor
	{
		public Task<IReadOnlyDictionary<string, int>> ProcessFileAsync(
			string filePath,
			IProgressReporter progressReporter,
			CancellationToken token);
	}
}
