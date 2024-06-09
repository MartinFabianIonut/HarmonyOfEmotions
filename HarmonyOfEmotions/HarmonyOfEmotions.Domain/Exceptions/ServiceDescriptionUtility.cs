using System.ComponentModel;
using System.Reflection;

namespace HarmonyOfEmotions.Domain.Exceptions
{
	public static class ServiceDescriptionUtility
	{
		public static string GetServiceDescription(Enum serviceName)
		{
			FieldInfo field = serviceName.GetType()?.GetField(serviceName.ToString())!;
			DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

			return attributes.Length > 0 ? attributes[0].Description : serviceName.ToString();
		}
	}
}
