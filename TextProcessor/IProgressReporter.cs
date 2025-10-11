namespace TextProcessor.Interfaces
{
	public interface IProgressReporter
	{
		void Report(long bytesRead, long totalBytes);
	}
}
