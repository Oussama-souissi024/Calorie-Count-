using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Mapping.FoodMapping
{
	public class CreateFoodRequest
	{
		public string Name { get; set; }
		public string? Description { get; set; }
		public double? Proteins { get; set; }
		public double? Calories { get; set; }
		public double? Carbohydrates { get; set; }
		public double? Fats { get; set; }
		public double? Fiber { get; set; }
		public string Unity { get; set; }
		public double Quantity { get; set; }
		public string Category { get; set; }
	}
}
