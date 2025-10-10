using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using TextProcessor.Interfaces;

namespace TextProcessor.Services
{
	public class FileReader : IFileReader
	{
		public async IAsyncEnumerable<(string line, long bytesRead)> ReadLinesAsync(
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
			string line;
			while ((line = await reader.ReadLineAsync(token)) != null)
			{
				long bytesRead = stream.Position;
				yield return (line, bytesRead);
			}

		}
	}
}
