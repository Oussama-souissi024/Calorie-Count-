using CalorieCount.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Entites
{
	public class Meal
	{
		[Key]
		public int MealID { get; set; }

		public int DailyMealID { get; set; }
		public virtual DailyMeal DailyMeal { get; set; }

		public MealType Type { get; set; }  // Example: "Breakfast", "Lunch", "Dinner"

		public string Description { get; set; }
		public double? Proteins { get; set; }
		public double? Calories { get; set; }
		public double? Carbohydrates { get; set; }
		public double? Fats { get; set; }
		public double? Fiber { get; set; }
	}
}
