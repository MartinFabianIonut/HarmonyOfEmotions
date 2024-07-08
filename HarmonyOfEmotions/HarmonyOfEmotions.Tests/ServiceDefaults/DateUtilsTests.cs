using HarmonyOfEmotions.ServiceDefaults.Utils;

namespace HarmonyOfEmotions.Tests.ServiceDefaults
{
	[TestClass]
	public class DateUtilsTests
	{
		[TestMethod]
		public void ConvertStringToDateTime_WithValidYearString_ReturnsDateTime()
		{
			// Arrange
			string validYearString = "2024";

			// Act
			var result = DateUtils.ConvertStringToDateTime(validYearString);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(new DateTime(2024, 1, 1), result);
		}

		[TestMethod]
		public void ConvertStringToDateTime_WithDateInFormatYYYYMMDD_ReturnsDateTime()
		{
			// Arrange
			string validDateString = "2024-12-31";

			// Act
			var result = DateUtils.ConvertStringToDateTime(validDateString);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(new DateTime(2024, 12, 31), result);
		}

		[TestMethod]
		public void ConvertStringToDateTime_WithInvalidYearString_ThrowsFormatException()
		{
			// Arrange
			string invalidYearString = "abcd";

			// Act & Assert
			Assert.ThrowsException<FormatException>(() => DateUtils.ConvertStringToDateTime(invalidYearString));
		}

	}
}
