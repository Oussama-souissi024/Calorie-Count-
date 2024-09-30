using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Mapping.UserRecipeMapping
{
	public class CreateUserRecipeRequest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public double? Proteins { get; set; }
		public double? Calories { get; set; }
		public double? Carbohydrates { get; set; }
		public double? Fats { get; set; }
		public double? Fiber { get; set; }
		public string Category { get; set; }
	}
}
