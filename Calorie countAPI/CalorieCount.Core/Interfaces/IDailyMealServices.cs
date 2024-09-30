using CalorieCount.Core.Entites;

namespace CalorieCount.Core.Interfaces
{
	public interface IDailyMealServices
	{
		ValueTask<DailyMeal> AddDailyMealAsync(string userId, DateOnly date);
		ValueTask<DailyMeal> GetByUserIDandDateAsync(string userId, DateOnly date);
		ValueTask<IEnumerable<DailyMeal>> GetAllByUserIDAsync(string userId);
	}
}
