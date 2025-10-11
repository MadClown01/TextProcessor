using TextProcessor.Services;

namespace TextProcessor.Testing
{
	[TestClass]
	public class WordCounterTests
	{
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
	}
}
