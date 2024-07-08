﻿namespace HarmonyOfEmotions.Domain.Authentication
{
	public class AuthResponse
	{
		public string? TokenType { get; set; }
		public string? AccessToken { get; set; }
		public string? RefreshToken { get; set; }
		public int ExpiresIn { get; set; }
	}
}
