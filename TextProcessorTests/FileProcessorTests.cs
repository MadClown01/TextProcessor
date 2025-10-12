using TextProcessor.Interfaces;
using TextProcessor.Services;

namespace TextProcessorTesting
{
	[TestClass]
	public class FileProcessorTests
	{
		private string _tempFilePath = string.Empty;

		[TestInitialize]
		public void Setup()
		{
			_tempFilePath = Path.GetTempFileName();
		}

		[TestCleanup]
		public void Cleanup()
		{
			if (File.Exists(_tempFilePath))
			{
				File.Delete(_tempFilePath);
			}
		}

		[TestMethod]
		public async Task FileProcessor_CountsWordsInFile()
		{
			// Arrange
			string content = "1:1 Adam Seth Enos\r\n1:2 Cainan Adam Seth Iared";
			await File.WriteAllTextAsync(_tempFilePath, content);
			var processor = new FileProcessor(new FileReader(), new Tokenizer());
			var progressReporter = new TestProgressReporter();
			IReadOnlyDictionary<string, int> counts = new Dictionary<string, int>();

			// Act
			counts = await processor.ProcessFileAsync(
				_tempFilePath,
				progressReporter,
				CancellationToken.None
				);

			// Assert
			Assert.AreEqual(counts["1:1"], 1);
			Assert.AreEqual(counts["Adam"], 2);
			Assert.AreEqual(counts["Seth"], 2);
			Assert.AreEqual(counts["Enos"], 1);
			Assert.AreEqual(counts["1:2"], 1);
			Assert.AreEqual(counts["Cainan"], 1);
			Assert.AreEqual(counts["Iared"], 1);
		}

		[TestMethod]
		public async Task FileProcessor_RespectsCancellationToken()
		{
			// Arrange
			string content = string.Join(
				Environment.NewLine,
				Enumerable.Repeat("The quick brown fox jumped over the lazy dog", 1000)
			);
			await File.WriteAllTextAsync(_tempFilePath, content);

			var processor = new FileProcessor(new FileReader(), new Tokenizer());
			var progressReporter = new TestProgressReporter();
			using var cts = new CancellationTokenSource();

			// Act
			var processingTask = processor.ProcessFileAsync(
				_tempFilePath,
				progressReporter,
				cts.Token
			);

			// Cancel almost immediately
			cts.Cancel();

			// Assert
			await Assert.ThrowsExceptionAsync<TaskCanceledException>(
				async () => await processingTask
			);
		}
	}
	public class TestProgressReporter : IProgressReporter
	{
		public void Report(long bytesRead, long totalBytes) { }
	}
}
