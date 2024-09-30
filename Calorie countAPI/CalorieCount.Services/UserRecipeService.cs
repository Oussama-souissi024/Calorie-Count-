using CalorieCount.Core.Entites;
using CalorieCount.Core.Interfaces;
using CalorieCount.Infrastructur.Data;
using Microsoft.EntityFrameworkCore;

namespace CalorieCount.Services
{
	public class UserRecipeService : IUserRecipe
	{
		private readonly IRepository<UserRecipe> _repository;
		private readonly ApplicationbDbContext _context;

		public UserRecipeService(IRepository<UserRecipe> repository, ApplicationbDbContext context)
		{
			_repository = repository;
			_context = context;
		}

		// Add a new UserRecipe
		public async ValueTask<UserRecipe> Add(UserRecipe userRecipe, string UserID)
		{
			userRecipe.UserID = UserID;
			var addedUserRecipe = await _repository.AddAsync(userRecipe);
			_repository.SaveChanges(); // Save changes to the database
			return addedUserRecipe;
		}

		// Get a specific UserRecipe by its ID
		public async ValueTask<UserRecipe> GetByID(int ID) =>
			await _repository.Read(ID);

		// Get all UserRecipes by UserID
		public async ValueTask<IEnumerable<UserRecipe>> GetAllUserRecipeByUSerID(string UserID)
		{
			return await _context.UserRecipes
				.Where(ur => ur.UserID == UserID)
				.ToListAsync();
		}

		// Get all UserRecipes by UserID and Date
		public async ValueTask<IEnumerable<UserRecipe>> GetAllUserRecipeByUSerIDandDate(string UserID, DateOnly Date)
		{
			return await _context.UserRecipes
				.Where(ur => ur.UserID == UserID && ur.Date == Date)
				.ToListAsync();
		}

		// Update an existing UserRecipe
		public void Update(UserRecipe userRecipe)
		{
			_repository.Update(userRecipe);
			_repository.SaveChanges(); // Save changes to the database
		}

		// Delete a UserRecipe by its ID
		public async ValueTask Delete(int ID)
		{
			await _repository.DeleteAsync(ID);
			_repository.SaveChanges(); // Save changes to the database
		}
	}
}
