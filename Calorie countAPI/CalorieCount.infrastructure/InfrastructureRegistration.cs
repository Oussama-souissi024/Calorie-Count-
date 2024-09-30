using CalorieCount.Core.Interfaces;
using CalorieCount.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CalorieCount.Infrastructure
{
	public static class InfrastructureRegistration
	{
		public static void  AddInfrastructureRegistraion(this WebApplicationBuilder builder)
		{
			builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
		}
	}
}
