using CalorieCount.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Interfaces
{
	public interface ITunisianFoodServices
	{
		ValueTask<TunisianFood> Add(TunisianFood TnFood);
		ValueTask<TunisianFood> GetByID(int tunisianFoodId);
		ValueTask<IEnumerable<TunisianFood>> GetAll();
		void Update(TunisianFood TnFood);
		ValueTask Delete(int ID);
	}
}
