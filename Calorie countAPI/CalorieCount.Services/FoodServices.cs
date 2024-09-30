using CalorieCount.Core.Entites;
using CalorieCount.Core.Interfaces;
using CalorieCount.Core.Mapping.FoodMapping;
using AutoMapper;
using CalorieCount.Infrastructur.Data;
using Microsoft.EntityFrameworkCore;


namespace CalorieCount.Services
{
	public class FoodServices : IFoodSevices
	{
		private readonly IRepository<Food> _repository;
		private readonly ApplicationbDbContext _context;
		public FoodServices(IRepository<Food> repository, ApplicationbDbContext context)
        {
			_repository = repository;
			_context = context;
		}

		public async ValueTask<Food> Add(Food food)
		{
			var addedFood = await _repository.AddAsync(food);
			_repository.SaveChanges(); // Save changes to the database
			return addedFood;
		}

		public async ValueTask<Food> GetByID(int ID) =>
			await _repository.Read(ID);
		
		public async ValueTask<IEnumerable<Food>> GetAll() =>
			await _context.GetAllFoods().ToListAsync();

		public void Update(Food food)
		{
			_repository.Update(food);
			_repository.SaveChanges(); // Save changes to the database
		}
		public async ValueTask Delete(int ID)
		{
			await _repository.DeleteAsync(ID);
			_repository.SaveChanges(); // Save changes to the database
		}

	}
}
