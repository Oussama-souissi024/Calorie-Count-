using CalorieCount.Core.Entites;
using CalorieCount.Core.Interfaces;
using CalorieCount.Infrastructur.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Services
{
	public class DailyMealServices : IDailyMealServices
	{
		private readonly IRepository<DailyMeal> _repository;
		private readonly ApplicationbDbContext _context;

		public DailyMealServices(IRepository<DailyMeal> repository, ApplicationbDbContext context)
        {
			_repository = repository;
			_context = context;
		}
        public async ValueTask<DailyMeal> AddDailyMealAsync(string userId, DateOnly date)
		{
			var dailyMeal = new DailyMeal
			{
				UserID = userId,
				Date = date,
				Proteins = 0,
				Calories = 0,
				Carbohydrates = 0,
				Fats = 0,
				Fiber = 0
			};
			dailyMeal = await _repository.AddAsync(dailyMeal);
			_repository.SaveChanges();
			return dailyMeal;
		}

		public async ValueTask<DailyMeal> GetByUserIDandDateAsync(string userId, DateOnly date)
		{
			// Convert DateOnly to DateTime for compatibility
			var dateTime = date.ToDateTime(TimeOnly.MinValue);

			// Call TVF with AsNoTracking to avoid entity tracking
			var result = await _context.GetDailyMealByUserIDandDate(userId, dateTime)
									   .AsNoTracking() // Avoid entity tracking to prevent infinite loop
									   .FirstOrDefaultAsync();

			return result;
		}


		public async ValueTask<IEnumerable<DailyMeal>> GetAllByUserIDAsync(string userId)
		{
			var result = await _context.GetAllDailyMealByUserID(userId)
									   .AsNoTracking() //  this line to avoid tracking
									   .ToListAsync();
			return result;
		}

	}
}
