using System.ComponentModel.DataAnnotations;

namespace CalorieCount.Core.Entites.Account_Entity
{
	public class ResetPasswordModel
	{
		[Required]
		public string Token { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(6)]
		public string OldPassword { get; set; }

		[Required]
		[MinLength(6)]
		public string NewPassword { get; set; }
	}
}
