using HarmonyOfEmotions.Domain.Exceptions;
using HarmonyOfEmotions.ServiceDefaults.Utils;

namespace HarmonyOfEmotions.ServiceDefaults.Exceptions
{
	public class ExternalServiceException(ServiceName serviceName, 
		Exception innerException) : 
		Exception($"{EnumDescriptionUtils.GetEnumDescription(serviceName)}", innerException)
	{
		public ServiceName ServiceName { get; } = serviceName;
	}
}
