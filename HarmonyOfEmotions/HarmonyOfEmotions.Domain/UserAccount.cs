using System.ComponentModel.DataAnnotations;

namespace HarmonyOfEmotions.Domain
{
	public class UserAccount
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a valid email address.")]
		[EmailAddress]
		public string? Email { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password.")]
		[MinLength(6)]
		public string? Password { get; set; }

		// ConfirmPassword should be conditionally required
		public string? ConfirmPassword { get; set; }

		public bool IsRegistering { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var validationResults = new List<ValidationResult>();

			if (IsRegistering)
			{
				if (string.IsNullOrWhiteSpace(ConfirmPassword))
				{
					validationResults.Add(new ValidationResult(
						"Please confirm your password.",
						[nameof(ConfirmPassword)]));
				}
				else if (ConfirmPassword != Password)
				{
					validationResults.Add(new ValidationResult(
						"Passwords do not match.",
						[nameof(ConfirmPassword)]));
				}
			}

			return validationResults;
		}
	}
}
