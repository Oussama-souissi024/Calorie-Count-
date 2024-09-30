using CalorieCount.Core.Entites;
using CalorieCount.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using static CalorieCount.Infrastructur.Data.ApplicationbDbContext;

namespace CalorieCount.Services
{
	public class TunisianFoodServices : ITunisianFoodServices
	{
		private readonly IRepository<TunisianFood> _repository;

		public TunisianFoodServices(IRepository<TunisianFood> repository)
		{
			_repository = repository;
		}

		public async ValueTask<TunisianFood> Add(TunisianFood tunisianFood)
		{
			var addedTunisianFood = await _repository.AddAsync(tunisianFood);
		    _repository.SaveChanges(); 
			return addedTunisianFood;
		}

		public async ValueTask<TunisianFood> GetByID(int tunisianFoodId)
		{
			return await _repository.Read(tunisianFoodId);
		}
		public async ValueTask<IEnumerable<TunisianFood>> GetAll()
		{
			return await _repository.ReadAll();
		}

		public void Update(TunisianFood tunisianFood)
		{
			_repository.Update(tunisianFood);
			_repository.SaveChanges();
		}

		public async ValueTask Delete(int id)
		{
			await _repository.DeleteAsync(id);
		    _repository.SaveChanges(); 
		}

		
	}
}