using HarmonyOfEmotions.Client.Services.ErrorHandling;
using HarmonyOfEmotions.Domain.External;
using Newtonsoft.Json;

namespace HarmonyOfEmotions.Client.Services.ApiServices
{
	public class ZenQuotesService(HttpClient httpClient, IErrorHandlingService errorHandlingService)
	{
		private readonly HttpClient _httpClient = httpClient;
		private readonly IErrorHandlingService _errorHandlingService = errorHandlingService;

		public async Task<QuoteResponse?> FetchQuote()
		{
			var response = await _httpClient.GetStringAsync("https://zenquotes.io/api/today");
			var quoteResponse = JsonConvert.DeserializeObject<QuoteResponse[]>(response);

			if (quoteResponse != null && quoteResponse.Length > 0)
			{
				return quoteResponse[0];
			}
			else
			{
				_errorHandlingService.HandleError(new Exception("An error occurred while fetching the quote."));
				return null;
			}
		}
	}
}
