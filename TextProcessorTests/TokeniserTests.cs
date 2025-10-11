using TextProcessor.Services;

namespace TextProcessor.Testing
{
	[TestClass]
	public class TokeniserTests
	{
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
				new[] { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy" },
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
	}
}
