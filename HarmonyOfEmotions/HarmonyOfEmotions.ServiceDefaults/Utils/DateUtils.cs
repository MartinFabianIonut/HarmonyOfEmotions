namespace HarmonyOfEmotions.ServiceDefaults.Utils
{
	public static class DateUtils
	{
		public static DateTime ConvertStringToDateTime(string yearString)
		{
			if (string.IsNullOrEmpty(yearString))
			{
				return DateTime.MinValue;
			}
			else
			{
				if (yearString.Length == 4 && int.TryParse(yearString, out int year))
				{
					return new DateTime(year, 1, 1);
				}
				else if (yearString.Length == 10 &&
						 int.TryParse(yearString.AsSpan(0, 4), out year) &&
						 int.TryParse(yearString.AsSpan(5, 2), out int month) &&
						 int.TryParse(yearString.AsSpan(8, 2), out int day))
				{
					return new DateTime(year, month, day);
				}
				else
				{
					throw new FormatException("The year is not in a valid format.");
				}
			}
		}

		public static string FormatDate(DateTime date)
		{
			if (date == DateTime.MinValue)
			{
				return "Still alive";
			}
			return $"{date.Day} {GetMonthName(date.Month)} {date.Year}";
		}

		private static string GetMonthName(int month)
		{
			return month switch
			{
				1 => "January",
				2 => "February",
				3 => "March",
				4 => "April",
				5 => "May",
				6 => "June",
				7 => "July",
				8 => "August",
				9 => "September",
				10 => "October",
				11 => "November",
				12 => "December",
				_ => string.Empty,
			};
		}
	}
}
