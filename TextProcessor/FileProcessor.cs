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
			var totalBytes = new FileInfo(filePath).Length; // Does this need to be async?

			//var bytesRead = reader.BaseStream.Position;
			//progress = (double)bytesRead / totalBytes;

			await foreach ((string line, long bytesRead) in _fileReader.ReadLinesAsync(filePath, token))
			{
				var words = _tokeniser.TokeniseLine(line);
				wordCounter.CountWords(words);
				progressReporter.Report(bytesRead, totalBytes);
			}

			return wordCounter.GetCounts();
		}
	}
}
