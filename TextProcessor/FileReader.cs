using System.IO;
using TextProcessor.Interfaces;

namespace TextProcessor.Services
{
	public class FileReader : IFileReader
	{
		public string ReadFile(string filePath)
		{
			//string lines = GetLines(filePath).GetAwaiter().GetResult();
			//return lines;
			return File.ReadAllText(filePath); // simple and synchronous
		}
		private async Task<string> GetLines(string filePath)
		{
			using (var reader = new StreamReader(filePath))
			{
				return await reader.ReadToEndAsync();
			}
		}
	}
}
