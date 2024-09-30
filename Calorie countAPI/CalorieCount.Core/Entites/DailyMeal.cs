using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CalorieCount.Core.Entites
{
	public class DailyMeal
	{
		[Key]
		public int DailyMealID { get; set; }

		[Required]
		public string UserID { get; set; }

		// Navigation property to AspNetUsers
		public virtual IdentityUser User { get; set; } // Marqué comme virtual

		[Required]
		public DateOnly Date { get; set; }

		public double? Proteins { get; set; }
		public double? Calories { get; set; }
		public double? Carbohydrates { get; set; }
		public double? Fats { get; set; }
		public double? Fiber { get; set; }

		public virtual ICollection<Meal> MealFoods { get; set; } = new List<Meal>();
	}
}
