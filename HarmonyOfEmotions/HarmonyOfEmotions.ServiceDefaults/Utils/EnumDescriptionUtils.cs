using System.ComponentModel;
using System.Reflection;

namespace HarmonyOfEmotions.ServiceDefaults.Utils
{
	public static class EnumDescriptionUtils
	{
		public static string GetEnumDescription(Enum serviceName)
		{
			FieldInfo field = serviceName.GetType()?.GetField(serviceName.ToString())!;
			DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

			return attributes.Length > 0 ? attributes[0].Description : serviceName.ToString();
		}
	}
}
