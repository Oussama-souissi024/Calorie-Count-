using CalorieCount.Core.Entites;
using CalorieCount.Core.Mapping.FoodMapping;

namespace CalorieCount.Core.Interfaces
{
	public interface IFoodSevices
	{
		ValueTask<Food> Add(Food food);
		ValueTask<Food> GetByID (int ID);
		ValueTask<IEnumerable<Food>> GetAll();
		void Update(Food food);
		ValueTask Delete(int ID);
		
	}
}
