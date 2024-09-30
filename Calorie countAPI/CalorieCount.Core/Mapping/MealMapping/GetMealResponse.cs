using CalorieCount.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Mapping.MealMapping
{
	public class GetMealResponse
	{
		public int MealID { get; set; }
		public DateOnly Date { get; set; }
		public MealType Type { get; set; }  // Example: "Breakfast", "Lunch", "Dinner", "lightMeal"
		public string? Description { get; set; }
		public double? Proteins { get; set; }
		public double? Calories { get; set; }
		public double? Carbohydrates { get; set; }
		public double? Fats { get; set; }
		public double? Fiber { get; set; }
	}
}
