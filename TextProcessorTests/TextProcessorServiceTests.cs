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
		[DataRow("lorem ipsum")]
		[DataRow("")]
		[DataRow("line1\nline2\nline3")]
		[DataRow("single line")]
		public async Task FileReader_ShouldReadStringFromFile(string content)
		{
			// Arrange
			await File.WriteAllTextAsync(_tempFilePath, content);
			var reader = new FileReader();

			// Act
			string lines = reader.ReadFile(_tempFilePath);

			// Assert
			Assert.AreEqual(content, lines);
		}

		[TestMethod]
		public void TextParser_ShouldReturnWordsFromLines()
		{
			// Arrange
			var parser = new TextParser();
			var lines = new List<string> { "asdf\nlorem\nipsum" };

			// Act
			IEnumerable<string> words = parser.ParseLines(lines);

			// Assert
			CollectionAssert.AreEqual(
				words.ToList(),
				new List<string> { "asdf", "lorem", "ipsum" }
			);
		}
		[TestMethod]
		public void TextParser_ShouldReturnSeperatedWords()
		{
			// Arrange
			var parser = new TextParser();
			var lines = new List<string> { "asdf\nlorem\nipsum" };

			// Act
			IEnumerable<string> words = parser.ParseLines(lines);

			// Assert
			CollectionAssert.AreEqual(
				words.ToList(),
				new List<string> { "asdf", "lorem", "ipsum" }
			);
		}

	}

	/// <summary>
	/// Confirm that usage of files behaves in the current environment as expected.
	/// </summary>
	[TestClass]
	public class FileUtilityTests
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
		public async Task TempFileReadsAndWrites()
		{
			// Arrange
			var content = "lorem ipsum";
			await File.WriteAllTextAsync(_tempFilePath, content);

			// Act
			string? lines;
			using (var reader = new StreamReader(_tempFilePath))
			{
				lines = await reader.ReadLineAsync();
			}

			// Assert
			Assert.IsTrue(File.Exists(_tempFilePath));
			Assert.AreEqual(lines, content);
		}
	}
}
