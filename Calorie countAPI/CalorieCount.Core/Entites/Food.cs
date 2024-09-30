using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CalorieCount.Core.Entites
{
	public class Food
	{
		[Key]
		public int Food_ID { get; set; }
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
