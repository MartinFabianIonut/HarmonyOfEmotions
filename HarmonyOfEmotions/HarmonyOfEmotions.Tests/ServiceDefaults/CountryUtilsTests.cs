using HarmonyOfEmotions.ServiceDefaults.Utils;

namespace HarmonyOfEmotions.Tests.ServiceDefaults
{
	[TestClass]
	public class CountryUtilsTests
	{
		[TestMethod]
		public void GetCountryForCode_WithValidCode_ReturnsCountry()
		{
			// Arrange
			string validCode = "US";

			// Act
			var result = CountryUtils.GetCountryForCode(validCode);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(validCode, result.Code);
			Assert.AreEqual("USA", result.Name);
		}

		[TestMethod]
		public void GetCountryForCode_WithInvalidCode_ReturnsNull()
		{
			// Arrange
			string invalidCode = "XX";

			// Act
			var result = CountryUtils.GetCountryForCode(invalidCode);

			// Assert
			Assert.IsNull(result);
		}
	}
}
