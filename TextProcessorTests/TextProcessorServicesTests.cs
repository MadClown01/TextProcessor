using TextProcessor.Services;

namespace TextProcessorTesting
{
	[TestClass]
	public class TextProcessorServicesTests
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
		public async Task FileReader_ShouldReturnString()
		{
			// Arrange
			var content = "lorem ipsum";
			await File.WriteAllTextAsync(_tempFilePath, content);
			var reader = new FileReader();

			// Act
			string lines = reader.ReadFile(_tempFilePath);

			// Assert
			Assert.AreEqual(lines, content);
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
