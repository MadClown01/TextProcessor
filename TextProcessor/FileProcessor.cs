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
		private readonly ITokenizer _tokenizer;
		private const int REPORT_INTERVAL_MILLISECONDS = 15; // report progress at most every 15ms

		public FileProcessor(IFileReader fileReader, ITokenizer tokeniser)
		{
			_fileReader = fileReader;
			_tokenizer = tokeniser;
		}

		public async Task<(IReadOnlyDictionary<string, int>, Double)> ProcessFileAsync(
			string filePath,
			IProgress<(long, long)> progress,
			CancellationToken token)
		{
			var wordCounter = new WordCounter();
			var totalBytes = new FileInfo(filePath).Length;

			var stopwatch = new Stopwatch();
			stopwatch.Start();
			var lastReportTime = stopwatch.Elapsed;

			await foreach ((string line, long bytesRead) in _fileReader.ReadLinesAsync(filePath, token))
			{
				var words = _tokenizer.TokenizeLine(line);
				wordCounter.CountWords(words);

				var elapsed = stopwatch.Elapsed;
				if (elapsed - lastReportTime >= TimeSpan.FromMilliseconds(REPORT_INTERVAL_MILLISECONDS))
				{
					progress?.Report((bytesRead, totalBytes));
					lastReportTime = elapsed;
				}
			}
			stopwatch.Stop();
			return (wordCounter.GetCounts(), stopwatch.Elapsed.TotalSeconds);
		}
	}
}
