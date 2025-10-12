using TextProcessor.Services;

namespace TextProcessorTesting
{
	[TestClass]
	public class TokenizerTests
	{
		[TestMethod]
		public void Tokenizer_SplitsWordsCorrectly()
		{
			// Arrange
			string line = "The quick brown fox";
			var tokenizer = new Tokenizer();

			// Act
			var tokens = tokenizer.TokenizeLine(line).ToArray();

			// Assert
			CollectionAssert.AreEqual(
				new[] { "The", "quick", "brown", "fox" },
				tokens
			);
		}

		[TestMethod]
		public void Tokenizer_HandlesExoticWhitespaceCharactersCorrectly()
		{
			// Arrange
			// Testing with space, tab, LF, CR, vertical tab, form feed, double space
			string line = "The quick\tbrown\nfox\rjumps\vover\fthe  lazy";
			var tokenizer = new Tokenizer();

			// Act
			var tokens = tokenizer.TokenizeLine(line).ToArray();

			// Assert
			CollectionAssert.AreEqual(
				new[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy" },
				tokens
			);
		}

		[TestMethod]
		public void Tokenizer_HandlesTaskExample()
		{
			// Arrange
			string line = "1:1 Adam Seth Enos\r\n1:2 Cainan Adam Seth Iared";
			var tokenizer = new Tokenizer();

			// Act
			var tokens = tokenizer.TokenizeLine(line).ToArray();

			// Assert
			CollectionAssert.AreEqual(
				new[] { "1:1", "Adam", "Seth", "Enos", "1:2", "Cainan", "Adam", "Seth", "Iared" },
				tokens
			);
		}
	}
}
