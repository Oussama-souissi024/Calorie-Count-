using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CalorieCount.Core.Entites
{
	public class UserRecipe
	{
		[Key]
		public int UserRecipeID { get; set; }
		public string Name { get; set; }

		[Required]
		public string UserID { get; set; }

		// Navigation property to AspNetUsers
		public virtual IdentityUser User { get; set; } // Marqué comme virtual

		[Required]
		public string Description { get; set; }
		public double? Proteins { get; set; }
		public double? Calories { get; set; }
		public double? Carbohydrates { get; set; }
		public double? Fats { get; set; }
		public double? Fiber { get; set; }
		[Required]
		public DateOnly Date { get; set; }
		[Required]
		public string Category { get; set; }
	}
}
