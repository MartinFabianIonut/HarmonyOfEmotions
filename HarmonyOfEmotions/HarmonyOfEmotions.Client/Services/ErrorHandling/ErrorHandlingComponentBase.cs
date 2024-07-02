using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HarmonyOfEmotions.Client.Services.ErrorHandling
{
	public class ErrorHandlingComponentBase : ComponentBase, IDisposable
	{
		[Inject]
		protected IErrorHandlingService? ErrorHandlingService { get; set; }

		[Inject]
		protected IJSRuntime? JSRuntime { get; set; }

		protected override void OnInitialized() => ErrorHandlingService!.OnError += HandleError;

		public void Dispose() => ErrorHandlingService!.OnError -= HandleError;

		protected virtual void HandleError(string errorMessage)
		{
			JSRuntime?.InvokeVoidAsync("showCustomAlert", errorMessage);
		}
	}
}
