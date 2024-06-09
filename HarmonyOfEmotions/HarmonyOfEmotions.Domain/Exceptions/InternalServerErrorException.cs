namespace HarmonyOfEmotions.Domain.Exceptions
{
	public class InternalServerErrorException(ServiceName serviceName,
		Exception innerException) :
		Exception($"{ServiceDescriptionUtility.GetServiceDescription(serviceName)}", innerException)
	{
		public ServiceName ServiceName { get; } = serviceName;
	}
}
