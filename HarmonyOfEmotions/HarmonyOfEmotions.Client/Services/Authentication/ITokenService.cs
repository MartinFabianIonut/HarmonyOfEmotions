namespace HarmonyOfEmotions.Client.Services.Authentication
{
	public interface ITokenService
	{
		Task<string?> GetAccessTokenAsync();
		Task<string?> GetRefreshTokenAsync();
		Task SetTokens(string? accessToken, string? refreshToken, int expiresIn);
	}
}
