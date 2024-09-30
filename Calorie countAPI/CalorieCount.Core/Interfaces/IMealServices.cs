using CalorieCount.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Interfaces
{
	public interface IMealServices
	{
		ValueTask<Meal> Add(Meal meal, string UserID);
		ValueTask<Meal> GetByID(int ID);
		ValueTask<IEnumerable<Meal>> GetAllMealByUSerID(string UserID);
		ValueTask<IEnumerable<Meal>> GetAllMealByUSerIDandDate(string UserID, DateOnly Date);
		ValueTask<IEnumerable<Meal>> GetAllMealByDailyMealID(int DailyMealID);
		void Update(Meal meal);
		ValueTask Delete(int ID);
	}
}
