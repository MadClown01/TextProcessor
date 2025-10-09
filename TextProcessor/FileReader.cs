using System.IO;
using TextProcessor.Interfaces;

namespace TextProcessor.Services
{
	public class FileReader : IFileReader
	{
		public async IAsyncEnumerable<string> ReadLinesAsync(string filePath)
		{
			using var reader = new StreamReader(filePath);
			while (!reader.EndOfStream)
			{
				var line = await reader.ReadLineAsync();
				yield return line;
			}
		}
	}
}
