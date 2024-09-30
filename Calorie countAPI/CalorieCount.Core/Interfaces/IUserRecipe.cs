using CalorieCount.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Interfaces
{
	public interface IUserRecipe
	{
		ValueTask<UserRecipe> Add(UserRecipe userRecipe, string UserID);
		ValueTask<UserRecipe> GetByID(int ID);
		ValueTask<IEnumerable<UserRecipe>> GetAllUserRecipeByUSerID(string UserID);
		ValueTask<IEnumerable<UserRecipe>> GetAllUserRecipeByUSerIDandDate(string UserID, DateOnly Date);
		void Update(UserRecipe userRecipe);
		ValueTask Delete(int ID);
	}
}
