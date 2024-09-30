namespace CalorieCount.Core.Mapping.UserRecipeMapping
{
	public class GetUserRecipeResponce
	{
		public int UserRecipeID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double? Proteins { get; set; }
		public double? Calories { get; set; }
		public double? Carbohydrates { get; set; }
		public double? Fats { get; set; }
		public double? Fiber { get; set; }
		public DateOnly Date { get; set; }
		public string Category { get; set; }
	}
}
