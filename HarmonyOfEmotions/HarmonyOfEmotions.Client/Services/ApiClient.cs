namespace HarmonyOfEmotions.Client.Services
{
	public class ApiClient(HttpClient httpClient)
	{
		private readonly HttpClient _httpClient = httpClient;

		public async Task<T?> GetAsync<T>(string uri)
		{
			return await _httpClient.GetFromJsonAsync<T>(uri);
		}

		public async Task<HttpResponseMessage> PostAsync<T>(string uri, T content)
		{
			return await _httpClient.PostAsJsonAsync(uri, content);
		}

		public async Task DeleteAsync(string uri)
		{
			await _httpClient.DeleteAsync(uri);
		}
	}
}
