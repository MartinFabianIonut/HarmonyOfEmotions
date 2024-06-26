using HarmonyOfEmotions.Client.Services.ErrorHandling;
using HarmonyOfEmotions.Domain.Exceptions;

namespace HarmonyOfEmotions.Client.Services.ApiServices
{
	public class ApiClient(HttpClient httpClient, IErrorHandlingService errorHandlingService)
	{
		private readonly HttpClient _httpClient = httpClient;
		private readonly IErrorHandlingService _errorHandlingService = errorHandlingService;

		public async Task<T?> GetAsync<T>(string uri)
		{
			var response = await _httpClient.GetAsync(uri);

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<T>();
			}
			else
			{
				try
				{
					var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
					_errorHandlingService.HandleError(new Exception(errorResponse?.Message ?? "An error occurred while processing the GET request."));
				}
				catch (Exception)
				{
					// do nothing
				}
			}

			return default;
		}

		public async Task<HttpResponseMessage> PostAsync<T>(string uri, T content)
		{
			var response = await _httpClient.PostAsJsonAsync(uri, content);

			if (!response.IsSuccessStatusCode)
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
				_errorHandlingService.HandleError(new Exception(errorResponse?.Message ?? "An error occurred while processing the POST request."));
			}

			return response;
		}

		public async Task DeleteAsync(string uri)
		{
			var response = await _httpClient.DeleteAsync(uri);

			if (!response.IsSuccessStatusCode)
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
				_errorHandlingService.HandleError(new Exception(errorResponse?.Message ?? "An error occurred while processing the DELETE request."));
			}
		}
	}
}
