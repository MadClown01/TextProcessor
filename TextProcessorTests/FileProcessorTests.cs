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
			var progress = new Progress<(long, long)>(p => { });
			IReadOnlyDictionary<string, int> counts = new Dictionary<string, int>();

			// Act
			(counts, var totalElapsed) = await processor.ProcessFileAsync(
				_tempFilePath,
				progress,
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
			var progress = new Progress<(long, long)>(p => { });
			using var cts = new CancellationTokenSource();

			// Act
			var processingTask = processor.ProcessFileAsync(
				_tempFilePath,
				progress,
				cts.Token
			);

			// Cancel almost immediately
			cts.Cancel();

			// Assert
			await Assert.ThrowsExceptionAsync<TaskCanceledException>(
				async () => await processingTask
			);
		}

		[TestMethod]
		public async Task FileProcessor_CountsElapsedTime()
		{
			// Arrange
			string content = "1:1 Adam Seth Enos\r\n1:2 Cainan Adam Seth Iared";
			var processor = new FileProcessor(new FileReader(), new Tokenizer());
			var progress = new Progress<(long, long)>(p => { });
			IReadOnlyDictionary<string, int> counts = new Dictionary<string, int>();
			double totalElapsedTime = 0.0;

			await File.WriteAllTextAsync(_tempFilePath, content);

			// Act
			(counts, totalElapsedTime) = await processor.ProcessFileAsync(
				_tempFilePath,
				progress,
				CancellationToken.None
			);

			// Assert
			Assert.AreNotEqual(totalElapsedTime, 0);
		}
	}
}
