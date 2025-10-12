using System.Diagnostics;
using System.IO;
using TextProcessor.Interfaces;

namespace TextProcessor.Services
{
	/// <summary>
	/// Responsible for reading, tokenising, counting.
	/// </summary>
	public class FileProcessor : IFileProcessor
	{
		private readonly IFileReader _fileReader;
		private readonly ITokeniser _tokeniser;
		private const int REPORT_INTERVAL_MILLISECONDS = 15; // report progress at most every 15ms

		public FileProcessor(IFileReader fileReader, ITokeniser tokeniser)
		{
			_fileReader = fileReader;
			_tokeniser = tokeniser;
		}

		public async Task<IReadOnlyDictionary<string, int>> ProcessFileAsync(
			string filePath,
			IProgressReporter progressReporter,
			CancellationToken token)
		{
			var wordCounter = new WordCounter();
			var totalBytes = new FileInfo(filePath).Length;

			var stopwatch = new Stopwatch();
			stopwatch.Start();
			var lastReportTime = stopwatch.Elapsed;

			await foreach ((string line, long bytesRead) in _fileReader.ReadLinesAsync(filePath, token))
			{
				var words = _tokeniser.TokeniseLine(line);
				wordCounter.CountWords(words);

				var elapsed = stopwatch.Elapsed;
				if (elapsed - lastReportTime >= TimeSpan.FromMilliseconds(REPORT_INTERVAL_MILLISECONDS))
				{
					progressReporter.Report(bytesRead, totalBytes);
					lastReportTime = elapsed;
				}
			}
			stopwatch.Stop();
			return wordCounter.GetCounts();
		}
	}
}
