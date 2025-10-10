namespace TextProcessor.Interfaces
{
	public interface IFileProcessor
	{
		public Task<IReadOnlyDictionary<string, int>> ProcessFileAsync(string filePath, IProgress<double> progress, CancellationToken token);
	}
}
