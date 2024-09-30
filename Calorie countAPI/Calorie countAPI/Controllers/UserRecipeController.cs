using AutoMapper;
using CalorieCount.Core.Entites;
using CalorieCount.Core.Interfaces;
using CalorieCount.Core.Mapping.UserRecipeMapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Calorie_countAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserRecipeController : ControllerBase
	{
		private readonly IUserRecipe _userRecipeService;
		private readonly IMapper _mapper;
		private readonly UserManager<IdentityUser> _userManager;

		public UserRecipeController(IUserRecipe userRecipeService, IMapper mapper, UserManager<IdentityUser> userManager)
		{
			_userRecipeService = userRecipeService;
			_mapper = mapper;
			_userManager = userManager;
		}

		// GET: api/UserRecipe/{id}
		[HttpGet("{id:int}")]
		public async Task<ActionResult<GetUserRecipeResponce>> GetUserRecipeById(int id)
		{
			try
			{
				var userRecipe = await _userRecipeService.GetByID(id);
				if (userRecipe == null)
					return NotFound(new { message = $"UserRecipe with ID {id} not found." });

				var response = _mapper.Map<GetUserRecipeResponce>(userRecipe);
				return Ok(new { data = response, message = "UserRecipe retrieved successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
			}
		}

		// GET: api/UserRecipe/user/{userId}
		[HttpGet("user/{userId}")]
		public async Task<ActionResult<IEnumerable<GetUserRecipeResponce>>> GetAllUserRecipesByUserId(string userId)
		{
			try
			{
				var userRecipes = await _userRecipeService.GetAllUserRecipeByUSerID(userId);
				if (userRecipes == null || !userRecipes.Any())
					return NotFound(new { message = "No recipes found for the given user." });

				var response = _mapper.Map<IEnumerable<GetUserRecipeResponce>>(userRecipes);
				return Ok(new { data = response, message = "UserRecipes retrieved successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
			}
		}

		// POST: api/UserRecipe
		[HttpPost]
		public async Task<ActionResult> CreateUserRecipe([FromBody] CreateUserRecipeRequest createUserRecipeRequest)
		{
			if (!ModelState.IsValid)
				return BadRequest(new { message = "Invalid model state." });

			try
			{
				var userID = _userManager.GetUserId(User);
				if (string.IsNullOrEmpty(userID))
					return Unauthorized(new { message = "User not authenticated." });

				var userRecipe = _mapper.Map<UserRecipe>(createUserRecipeRequest);
				userRecipe.Date = DateOnly.FromDateTime(DateTime.Now);

				await _userRecipeService.Add(userRecipe, userID);

				return Ok(new { message = "UserRecipe created successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
			}
		}

		// PUT: api/UserRecipe/{id}
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateUserRecipe(int id, [FromBody] CreateUserRecipeRequest updateUserRecipeRequest)
		{
			if (!ModelState.IsValid)
				return BadRequest(new { message = "Invalid model state." });

			try
			{
				var userID = _userManager.GetUserId(User);
				if (string.IsNullOrEmpty(userID))
					return Unauthorized(new { message = "User not authenticated." });

				var userRecipeToUpdate = _mapper.Map<UserRecipe>(updateUserRecipeRequest);
				userRecipeToUpdate.UserRecipeID = id;
				userRecipeToUpdate.UserID = userID;
				userRecipeToUpdate.Date = DateOnly.FromDateTime(DateTime.Now);

				_userRecipeService.Update(userRecipeToUpdate);
				return Ok(new { message = "UserRecipe updated successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
			}
		}

		// DELETE: api/UserRecipe/{id}
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteUserRecipe(int id)
		{
			try
			{
				await _userRecipeService.Delete(id);
				return Ok(new { message = "UserRecipe deleted successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
			}
		}
	}
}
