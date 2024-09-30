using CalorieCount.Core.Entites.Account_Entity;
using CalorieCount.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Calorie_countAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly IEmailSender _emailSender;

		public AccountController(UserManager<IdentityUser> userManager,
								SignInManager<IdentityUser> signInManager,
								RoleManager<IdentityRole> roleManager,
								IConfiguration configuration,
								IEmailSender emailSender)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_emailSender = emailSender;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				if (!await _roleManager.RoleExistsAsync("SimplesUser"))
				{
					await _roleManager.CreateAsync(new IdentityRole("SimplesUser"));
				}

				await _userManager.AddToRoleAsync(user, "SimplesUser");

				// Generate email confirmation token
				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				var callbackUrl = Url.Action(
					"ConfirmEmail",
					"Account",
					new { userId = user.Id, code = code },
					protocol: Request.Scheme);

				// Send confirmation email
				await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
					$"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

				return Ok(new { Message = "User registered successfully with role SimplesUser. Please check your email to confirm your account." });
			}

			return BadRequest(result.Errors);
		}

		[HttpGet("confirm-email")]
		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
				return BadRequest("User ID and code must be provided.");

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
				return BadRequest("Invalid user.");

			var result = await _userManager.ConfirmEmailAsync(user, code);
			if (result.Succeeded)
				return Ok("Email confirmed successfully.");

			return BadRequest("Email confirmation failed.");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var user = await _userManager.FindByNameAsync(model.UserName);

			if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				// Check if the user has confirmed their email
				if (!await _userManager.IsEmailConfirmedAsync(user))
				{
					return BadRequest(new { Message = "Please confirm your email before logging in." });
				}
				// Generate token if email is confirmed
				var token = GenerateJwtToken(user);
				return Ok(new { Message = "Login successful.", Token = token });
			}

			return Unauthorized(new { Message = "Invalid login attempt." });
		}

		// 1. Forgot Password - Send reset token to email
		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
				return BadRequest(new { Message = "User not found." });

			// Générer un token de réinitialisation du mot de passe
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);

			// Générer l'URL du formulaire HTML
			var formUrl = $"https://localhost:7146/reset-password.html?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

			// Envoyer le lien de réinitialisation par e-mail
			await _emailSender.SendEmailAsync(user.Email, "Reset Password",
				$"Please reset your password by <a href='{formUrl}'>clicking here</a>.");

			return Ok(new { Message = "Password reset email sent. Please check your inbox." });
		}


		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
				return BadRequest(new { Message = "Invalid request." });

			// Check if the old password is correct
			if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
			{
				return BadRequest(new { Message = "Old password is incorrect." });
			}

			// Reset the password
			var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

			if (result.Succeeded)
				return Ok(new { Message = "Password has been reset successfully." });

			return BadRequest(result.Errors);
		}


		private string GenerateJwtToken(IdentityUser user)
		{
			var jwtSettings = _configuration.GetSection("JwtConfig");
			var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(ClaimTypes.NameIdentifier, user.Id)
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
