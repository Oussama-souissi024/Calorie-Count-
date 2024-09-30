using CalorieCount.Core.Entites;
using CalorieCount.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CalorieCount.Infrastructur.Data;
using Microsoft.Data.SqlClient;

namespace CalorieCount.Services
{
	public class MealServices : IMealServices
	{
		private readonly IRepository<Meal> _repository;
		private readonly ApplicationbDbContext _context;

		public MealServices(IRepository<Meal> repository, ApplicationbDbContext context)
		{
			_repository = repository;
			_context = context;
		}


		public async ValueTask<Meal> Add(Meal meal, string UserID)
		{
			try
			{
				// Retrieve the DailyMeal ID for the user and the current date
				var DailyMealID = _context.GetDailyMealIdByUserIDandDate(UserID, DateTime.Now);

				if (DailyMealID.HasValue)
				{
					meal.DailyMealID = DailyMealID.Value;
				}
				else
				{
					// Handle the case when DailyMealID is not found or invalid
					throw new InvalidOperationException("DailyMealID not found for the specified UserID and Date.");
				}

				// Add the meal and save changes
				var addedMeal = await _repository.AddAsync(meal);
				_repository.SaveChanges();

				// Return the added meal
				return addedMeal;
			}
			catch (SqlException sqlEx)
			{
				// Specific handling of SQL-related errors
				Console.WriteLine($"Erreur SQL : {sqlEx.Message}");
				throw; // Rethrow the exception
			}
			catch (DbUpdateException dbEx)
			{
				// Errors related to database update
				Console.WriteLine($"Erreur lors de la mise à jour de la base de données : {dbEx.Message}");
				throw; // Rethrow the exception
			}
			catch (Exception ex)
			{
				// Generic handling of other exceptions
				Console.WriteLine($"Une erreur inattendue est survenue : {ex.Message}");
				throw; // Rethrow the exception
			}
		}


		public async ValueTask<Meal> GetByID(int ID) =>
			 await _repository.Read(ID);

		public async ValueTask<IEnumerable<Meal>> GetAllMealByUSerID(string userID)
		{
			try
			{
				// Retrieve meals for the given user
				return await _context.Meals
					.Where(m => m.DailyMeal.UserID == userID)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erreur lors de la récupération des repas : {ex.Message}");
				throw;
			}
		}



		public ValueTask<IEnumerable<Meal>> GetAllMealByUSerIDandDate(string UserID, DateOnly Date)
		{
			throw new NotImplementedException();
		}

		public ValueTask<IEnumerable<Meal>> GetAllMealByDailyMealID(int DailyMealID)
		{
			throw new NotImplementedException();
		}

		public void Update(Meal updatedMeal)
		{
			try
			{
				// Update meal via repository
				_repository.Update(updatedMeal);
				_repository.SaveChanges(); // Sauvegarder les modifications dans la base de données
			}
			catch (Exception ex)
			{
				// Error Handling
				Console.WriteLine($"Erreur lors de la mise à jour du repas : {ex.Message}");
				throw;
			}
		}

		public async ValueTask Delete(int mealID)
		{
			try
			{
				// Delete meal via repository
				await _repository.DeleteAsync(mealID);
				_repository.SaveChanges(); 
			}
			catch (Exception ex)
			{
				// Error Handling
				Console.WriteLine($"Erreur lors de la suppression du repas : {ex.Message}");
				throw;
			}
		}
	}
}
