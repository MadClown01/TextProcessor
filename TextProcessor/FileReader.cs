using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using TextProcessor.Interfaces;
using TextProcessor.Services;

namespace TextProcessor.Services
{
	public class FileReader : IFileReader
	{
		public async IAsyncEnumerable<string> ReadLinesAsync(
			string filePath,
			[EnumeratorCancellation] CancellationToken token = default)
		{
			using var stream = new FileStream(
				filePath,
				FileMode.Open,
				FileAccess.Read,
				FileShare.Read,
				4096,
				FileOptions.Asynchronous);
			using var reader = new StreamReader(stream);
			string? line;
			while ((line = await reader.ReadLineAsync()) != null)
			{
				token.ThrowIfCancellationRequested();
				yield return line;
			}
		}
	}
}
