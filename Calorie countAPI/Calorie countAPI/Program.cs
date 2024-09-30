using CalorieCount.Core.Enums;
using CalorieCount.Infrastructur.Data;
using CalorieCount.Infrastructure;
using CalorieCount.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		// Ajoutez le convertisseur JSON personnalisé
		options.JsonSerializerOptions.Converters.Add(new MealTypeConverter());
	});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calorie Count API", Version = "v1" });

	// Configure Swagger to use JWT Authentication without 'Bearer' prefix
	c.AddSecurityDefinition("Token", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer", // This field is required but will not affect the behavior in this case
		In = ParameterLocation.Header,
		Description = "Enter your token in the text input below. No 'Bearer' prefix is required."
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Token"
				}
			},
			new string[] {}
		}
	});
});


builder.Services.AddDbContext<ApplicationbDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.AddInfrastructureRegistraion();
builder.AddServiceRegistraion();

builder.Logging.AddConsole(); // Optional: to also see logs in the console during development
builder.Logging.AddEventLog(eventLogSettings =>
{
	eventLogSettings.SourceName = "CalorieCountAPI"; // Set a custom source name for Event Viewer
});


// Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminOnly", policy =>
		policy.RequireRole("Admin"));

	options.AddPolicy("SimplesUserOnly", policy =>
		policy.RequireRole("SimplesUser"));
});

var app = builder.Build();

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Code to create Admin and SimplesUser roles and default Admin user
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
	var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

	// Create the Admin role if it does not exist
	if (!await roleManager.RoleExistsAsync("Admin"))
	{
		var role = new IdentityRole("Admin");
		await roleManager.CreateAsync(role);
	}

	// Create the SimplesUser role if it does not exist
	if (!await roleManager.RoleExistsAsync("SimplesUser"))
	{
		var role = new IdentityRole("SimplesUser");
		await roleManager.CreateAsync(role);
	}

	// Create an admin user if it does not exist
	var user = await userManager.FindByNameAsync("admin");
	if (user == null)
	{
		user = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
		var result = await userManager.CreateAsync(user, "Admin2024#");
		if (result.Succeeded)
		{
			await userManager.AddToRoleAsync(user, "Admin");
		}
	}
}

app.Run();
