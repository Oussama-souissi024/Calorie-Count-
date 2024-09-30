using CalorieCount.Core.Entites;
using CalorieCount.Core.Interfaces;
using CalorieCount.Core.Mapping.DailyMealMapping;
using CalorieCount.Core.Mapping.FoodMapping;
using CalorieCount.Core.Mapping.MealMapping;
using CalorieCount.Core.Mapping.TunisianFoodMapping;
using CalorieCount.Core.Mapping.UserRecipeMapping;
using CalorieCount.Infrastructur.Data;
using CalorieCount.Services.BackgroundServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CalorieCount.Services
{
	public static class ServicesRegistration
	{
		public static void AddServiceRegistraion(this WebApplicationBuilder builder)
		{
			builder.Services.AddTransient<IFoodSevices, FoodServices>();
			builder.Services.AddTransient<ITunisianFoodServices, TunisianFoodServices>();
			builder.Services.AddTransient<IDailyMealServices, DailyMealServices > ();
			builder.Services.AddTransient<IMealServices, MealServices>();
			builder.Services.AddTransient<IUserRecipe, UserRecipeService>();
			builder.Services.AddTransient<IEmailSender, EmailSenderServices>();

			// Using Scoped for Daily Meal Services
			builder.Services.AddScoped<IDailyMealServices, DailyMealServices>(); 

			builder.Services.AddAutoMapper(typeof(FoodMappingProfile));
			builder.Services.AddAutoMapper(typeof(TunisianFoodMappingProfile));
			builder.Services.AddAutoMapper(typeof(DailyMealMappingProfile));
			builder.Services.AddAutoMapper(typeof(MealMappingProfile));
			builder.Services.AddAutoMapper(typeof(UserRecipeMappingProfile));

			// Save background service
			builder.Services.AddHostedService<DailyMealBackgroundService>();

			builder.Services.AddIdentity<IdentityUser, IdentityRole>()
			.AddEntityFrameworkStores<ApplicationbDbContext>()
			.AddDefaultTokenProviders();

			// Configure authentication and authorization
			builder.Services.AddAuthentication();
			builder.Services.AddAuthorization();
		}
	}

}
