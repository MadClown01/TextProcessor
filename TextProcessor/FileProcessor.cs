using System.IO;
using TextProcessor.Interfaces;

namespace TextProcessor.Services
{
	/// <summary>
	/// Responsible for reading, tokenising, counting.
	/// Accepts a file path, tokeniser, word counter.
	/// Reports progress via IProgress<double>.
	/// Supports cancellation via CancellationToken.
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
			IProgress<double> progress,
			CancellationToken token)
		{
			var wordCounter = new WordCounter();
			var totalBytes = new FileInfo(filePath).Length; // Does this need to be async?

			//var bytesRead = reader.BaseStream.Position;
			//progress = (double)bytesRead / totalBytes;

			await foreach (var line in _fileReader.ReadLinesAsync(filePath).WithCancellation(token))
			{
				var words = _tokeniser.TokeniseLine(line);
				wordCounter.CountWords(words);

				var bytesRead = 1;//reader.BaseStream.Position;
				progress?.Report((double)bytesRead / totalBytes * 100);
			}

			return wordCounter.GetCounts();
		}
	}
}
