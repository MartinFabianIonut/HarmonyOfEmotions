namespace HarmonyOfEmotions.Client.Services.ErrorHandling
{
	public interface IErrorHandlingService
	{
		event Action<string>? OnError;

		void HandleError(Exception ex);
	}

	public class ErrorHandlingService : IErrorHandlingService
	{
		public event Action<string>? OnError;

		public void HandleError(Exception ex)
		{
			OnError?.Invoke(ex.Message);
		}
	}
}
