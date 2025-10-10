using TextProcessor.Services;

namespace TextProcessor.Testing
{
	[TestClass]
	public class TextProcessorServiceTests
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

		[DataTestMethod]
		[DataRow("")]
		[DataRow("single line")]
		[DataRow("line1\nline2\nline3")]
		[DataRow(
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua" +
			"\nLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua" +
			"\nLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua"
			)]
		public async Task FileReader_ReadsLinesFromFile(string content)
		{
			// Arrange
			await File.WriteAllTextAsync(_tempFilePath, content);
			var reader = new FileReader();

			// Act
			var linesList = new List<string>();
			await foreach (var line in reader.ReadLinesAsync(_tempFilePath))
			{
				linesList.Add(line);
			}
			string lines = string.Join("\n", linesList);

			// Assert
			Assert.AreEqual(content, lines);
		}

		[TestMethod]
		public void Tokeniser_SplitsWordsCorrectly()
		{
			// Arrange
			string line = "The quick brown fox";
			var tokeniser = new Tokeniser();

			// Act
			var tokens = tokeniser.TokeniseLine(line).ToArray();

			// Assert
			CollectionAssert.AreEqual(
				new[] { "The", "quick", "brown", "fox" },
				tokens
			);
		}

		[TestMethod]
		public void Tokeniser_HandlesExoticWhitespaceCharactersCorrectly()
		{
			// Arrange
			// Testing with space, tab, LF, CR, vertical tab, form feed, double space
			string line = "The quick\tbrown\nfox\rjumps\vover\fthe  lazy";
			var tokeniser = new Tokeniser();

			// Act
			var tokens = tokeniser.TokeniseLine(line).ToArray();

			// Assert
			CollectionAssert.AreEqual(
				new[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy"},
				tokens
			);
		}

		[TestMethod]
		public void Tokeniser_HandlesTaskExample()
		{
			// Arrange
			string line = "1:1 Adam Seth Enos\r\n1:2 Cainan Adam Seth Iared";
			var tokeniser = new Tokeniser();

			// Act
			var tokens = tokeniser.TokeniseLine(line).ToArray();

			// Assert
			CollectionAssert.AreEqual(
				new[] { "1:1", "Adam", "Seth", "Enos", "1:2", "Cainan", "Adam", "Seth", "Iared" },
				tokens
			);
		}

		[TestMethod]
		public void WordCounter_CountsTokens()
		{
			// Arrange
			string[] tokens = new string[] { "a", "a", "b", "a", "c", "c" };
			var counter = new WordCounter();

			// Act
			counter.CountWords(tokens);
			IReadOnlyDictionary<string, int> counts = counter.GetCounts();

			// Assert
			Assert.AreEqual(counts["a"], 3);
			Assert.AreEqual(counts["b"], 1);
			Assert.AreEqual(counts["c"], 2);
		}

		[TestMethod]
		public void WordCounter_HandlesTaskExample()
		{
			// Arrange
			string[] tokens = new string[] { "1:1", "Adam", "Seth", "Enos", "1:2", "Cainan", "Adam", "Seth", "Iared" };
			var counter = new WordCounter();

			// Act
			counter.CountWords(tokens);
			IReadOnlyDictionary<string, int> counts = counter.GetCounts();

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
		public async Task FileProcessor_CountsWordsInFile()
		{
			// Arrange
			string content = "1:1 Adam Seth Enos\r\n1:2 Cainan Adam Seth Iared";
			await File.WriteAllTextAsync(_tempFilePath, content);
			var processor = new FileProcessor(new FileReader(), new Tokeniser());

			// Act
			IReadOnlyDictionary<string, int> counts = processor.ProcessFileAsync(
				_tempFilePath,
				null,
				CancellationToken.None
				).Result;

			// Assert
			Assert.AreEqual(counts["1:1"], 1);
			Assert.AreEqual(counts["Adam"], 2);
			Assert.AreEqual(counts["Seth"], 2);
			Assert.AreEqual(counts["Enos"], 1);
			Assert.AreEqual(counts["1:2"], 1);
			Assert.AreEqual(counts["Cainan"], 1);
			Assert.AreEqual(counts["Iared"], 1);
		}
	}
}
