using HarmonyOfEmotions.Client.Services.ErrorHandling;
using Newtonsoft.Json.Linq;

namespace HarmonyOfEmotions.Client.Services.ApiServices
{
	public class GoogleSearchAutocompleteService(HttpClient httpClient, IErrorHandlingService errorHandlingService)
	{
		private readonly HttpClient _httpClient = httpClient;
		private readonly IErrorHandlingService _errorHandlingService = errorHandlingService;

		public async Task<string[]> FetchSearchKeywords(string startOfKeyword)
		{
			string encodedKeyword = Uri.EscapeDataString(startOfKeyword);
			var response = await _httpClient.GetStringAsync($"https://www.google.com/complete/search?client=spotify&client=firefox&q={encodedKeyword}");

			try
			{
				var jsonResponse = JArray.Parse(response);

				var suggestions = jsonResponse[1].ToObject<string[]>();

				if (suggestions == null)
				{
					return [];
				}
				return suggestions.Take(5).ToArray();
			}
			catch (Exception)
			{
				_errorHandlingService.HandleError(new Exception("An error occurred while fetching the search keywords."));
				return [];
			}
		}
	}
}
