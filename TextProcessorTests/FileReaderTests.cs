using TextProcessor.Services;

namespace TextProcessor.Testing
{
	[TestClass]
	public class FileReaderTests
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
			await foreach ((string line, long bytesRead) in reader.ReadLinesAsync(_tempFilePath))
			{
				linesList.Add(line);
			}
			string lines = string.Join("\n", linesList);

			// Assert
			Assert.AreEqual(content, lines);
		}
	}
}
