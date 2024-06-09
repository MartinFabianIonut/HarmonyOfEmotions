using HarmonyOfEmotions.Domain;
using System.Collections;
using System.Globalization;
using System.Resources;

namespace HarmonyOfEmotions.ServiceDefaults.Utils
{
	public static class CountryUtils
	{
		private static readonly Dictionary<string, Country> _countries;

		static CountryUtils()
		{
			_countries = [];
			var resourceManager = new ResourceManager("HarmonyOfEmotions.ServiceDefaults.Resources.Countries", typeof(CountryUtils).Assembly);
			var resourceSet = resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

			if (resourceSet != null)
			{
				foreach (DictionaryEntry entry in resourceSet)
				{
					if (entry.Value != null)
					{
						_countries[entry.Value.ToString()!] = new Country
						{
							Name = entry.Key.ToString(),
							Code = entry.Value.ToString()
						};
					}
				}
			}
		}

		public static Country? GetCountryForCode(string code)
		{
			if (string.IsNullOrEmpty(code))
			{
				return null;
			}

			if (_countries.TryGetValue(code, out var country))
			{
				return country;
			}

			return null;
		}
	}
}
