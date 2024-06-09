namespace HarmonyOfEmotions.Domain.Exceptions
{
	public class ExternalServiceException(ServiceName serviceName, 
		Exception innerException) : 
		Exception($"{ServiceDescriptionUtility.GetServiceDescription(serviceName)}", innerException)
	{
		public ServiceName ServiceName { get; } = serviceName;
	}
}
