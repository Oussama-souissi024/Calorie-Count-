using CalorieCount.Core.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CalorieCount.Infrastructur.Data
{
	public class ApplicationbDbContext : IdentityDbContext
	{
		public ApplicationbDbContext(DbContextOptions<ApplicationbDbContext> options) : base(options)
		{

		}

		// TVF configuration in OnModelCreating
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Meal>()
				 .Property(m => m.Type)
				 .HasConversion<int>();
			//  trigger for insert operation
			modelBuilder.Entity<Meal>()
				.ToTable(Tb => Tb.HasTrigger("trg_UpdateDailyMealAfterInsertMeal"));

			//  trigger for update operation
			modelBuilder.Entity<Meal>()
				.ToTable(tb => tb.HasTrigger("trg_UpdateDailyMealAfterUpdateMeal"));

			//  trigger for delete operation
			modelBuilder.Entity<Meal>()
				.ToTable(tb => tb.HasTrigger("trg_UpdateDailyMealAfterDeleteMeal"));

			modelBuilder
				.HasDbFunction(() => GetAllDailyMealByUserID(default))
				.HasName("GetAllDailyMealByUserID")
				.HasSchema("dbo");
			modelBuilder
				.HasDbFunction(() => GetDailyMealByUserIDandDate(default, default))
				.HasName("GetDailyMealByUserIDandDate")
				.HasSchema("dbo");
			// Configure the GetDailyMealIdByUserIDandDate scalar function
			modelBuilder
				.HasDbFunction(() => GetDailyMealIdByUserIDandDate(default, default))
				.HasName("GetDailyMealIdByUserIDandDate")
				.HasSchema("dbo");
			
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=.;Database=CalorieCount;User Id=sa;Password=sa123456;Encrypt=True;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=True");
			// Enable lazy loading
			optionsBuilder.UseLazyLoadingProxies();
		}

		public virtual DbSet<Food> Foods { get; set; }
		public virtual DbSet<TunisianFood> TunisianFoods { get; set; }
		public virtual DbSet<UserRecipe> UserRecipes { get; set; }
		public virtual DbSet<Meal> Meals { get; set; }
		public virtual DbSet<DailyMeal> DailyMeals { get; set; }

		// Map the TVF to retrieve all foods
		[DbFunction("GetAllFoods", "dbo")]
		public IQueryable<Food> GetAllFoods()
		{
			return FromExpression(() => GetAllFoods());
		}
		public IQueryable<DailyMeal> GetDailyMealByUserIDandDate(string userID, DateTime date)
		{
			return FromExpression(() => GetDailyMealByUserIDandDate(userID, date));
		}
		public IQueryable<DailyMeal> GetAllDailyMealByUserID(string userID)
		{
			return FromExpression(() => GetAllDailyMealByUserID(userID));
		}

		// Method to call scalar function
		public int? GetDailyMealIdByUserIDandDate(string userID, DateTime date)
		{
			var userIdParam = new SqlParameter("@UserID", userID);
			var dateParam = new SqlParameter("@Date", date.Date);
			var dailyMealIdParam = new SqlParameter
			{
				ParameterName = "@DailyMealID",
				SqlDbType = System.Data.SqlDbType.Int,
				Direction = System.Data.ParameterDirection.Output
			};

			this.Database.ExecuteSqlRaw(
				"EXEC @DailyMealID = dbo.GetDailyMealIdByUserIDandDate @UserID, @Date",
				userIdParam, dateParam, dailyMealIdParam);

			// Vérification si la valeur de @DailyMealID est DBNull
			if (dailyMealIdParam.Value == DBNull.Value)
			{
				return null;  // Renvoie null si aucun DailyMealID n'est trouvé
			}

			return (int)dailyMealIdParam.Value;
		}







	}
}
