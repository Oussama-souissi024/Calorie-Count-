using CalorieCount.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace CalorieCount.Services.BackgroundServices
{
	public class DailyMealBackgroundService : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly ILogger<DailyMealBackgroundService> _logger;

		public DailyMealBackgroundService(IServiceScopeFactory serviceScopeFactory,
			ILogger<DailyMealBackgroundService> logger)
		{
			_serviceScopeFactory = serviceScopeFactory;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("DailyMealBackgroundService started.");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					var currentTime = DateTime.Now;
					if (currentTime.Hour == 0 && currentTime.Minute == 0)
					{
						using (var scope = _serviceScopeFactory.CreateScope())
						{
							var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
							var dailyMealService = scope.ServiceProvider.GetRequiredService<IDailyMealServices>();

							var userIds = await GetAllUserIdsAsync(userManager);

							foreach (var userId in userIds)
							{
								await dailyMealService.AddDailyMealAsync(userId, DateOnly.FromDateTime(currentTime));
							}

							_logger.LogInformation("New DailyMeal inserted for all users at {Time}.", currentTime);
						}
					}
					// Wait a minute before checking again
					await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "An error occurred in DailyMealBackgroundService.");
				}
			}

			_logger.LogInformation("DailyMealBackgroundService stopped.");
		}

		private async Task<IEnumerable<string>> GetAllUserIdsAsync(UserManager<IdentityUser> userManager)
		{
			// Utiliser userManager pour accéder aux utilisateurs
			return await userManager.Users
				.Select(user => user.Id)
				.ToListAsync();
		}
	}
}
