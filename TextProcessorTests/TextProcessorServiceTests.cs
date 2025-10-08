using TextProcessor.Services;

namespace TextProcessorTesting
{
	[TestClass]
	public class TextProcessorServiceTests
	{
		[TestMethod]
		public void Processor_ShouldReturnString()
		{
			// Arrange
			var service = new TextProcessingService();
			string input = "hello";

			// Act
			string result = service.ProcessText(input);

			// Assert
			Assert.AreEqual("hello", result);
		}
	}
}
