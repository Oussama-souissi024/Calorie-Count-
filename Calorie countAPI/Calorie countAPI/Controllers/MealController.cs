using AutoMapper;
using CalorieCount.Core.Entites;
using CalorieCount.Core.Interfaces;
using CalorieCount.Core.Mapping.MealMapping;
using CalorieCount.Infrastructur.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calorie_countAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MealController : ControllerBase
	{
		private readonly IMealServices _mealServices;
		private readonly IMapper _mapper;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ApplicationbDbContext _context;

		public MealController(IMealServices mealServices, 
							  IMapper mapper,
							  UserManager<IdentityUser> userManager,
							  ApplicationbDbContext context)
		{
			_mealServices = mealServices;
			_mapper = mapper;
			_userManager = userManager;
			_context = context;
		}

		// GET: api/Meal/{date}
		[HttpGet("{date}")]
		public async Task<ActionResult<IEnumerable<GetMealResponse>>> GetMealsByUserIDAndDate(DateOnly date)
		{
			try
			{
				var userID = _userManager.GetUserId(User);
				if (string.IsNullOrEmpty(userID))
					return Unauthorized(new { message = "User not authenticated." });

				var meals = await _mealServices.GetAllMealByUSerIDandDate(userID, date);
				if (meals == null || !meals.Any())
					return NotFound(new { message = "No meals found for the given user and date." });

				var response = _mapper.Map<IEnumerable<GetMealResponse>>(meals);
				return Ok(new { data = response, message = "Meals retrieved successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
			}
		}

		// POST: api/Meal
		[HttpPost]
		// POST: api/Meal
		[HttpPost]
		public async Task<ActionResult> CreateMeal([FromBody] CreateMealRequest createMealRequest)
		{
			if (!ModelState.IsValid)
				return BadRequest(new { message = "Invalid model state." });

			try
			{
				var userID = _userManager.GetUserId(User);
				if (string.IsNullOrEmpty(userID))
					return Unauthorized(new { message = "User not authenticated." });

				var meal = _mapper.Map<Meal>(createMealRequest);
				await _mealServices.Add(meal, userID);

				return Ok(new { message = "Meal created successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
			}
		}


		// GET: api/Meal/{id}
		[HttpGet("{id:int}")]
		public async Task<ActionResult<GetMealResponse>> GetMealById(int id)
		{
			try
			{
				var meal = await _mealServices.GetByID(id);
				if (meal == null)
					return NotFound(new { message = $"Meal with ID {id} not found." });

				var response = _mapper.Map<GetMealResponse>(meal);
				return Ok(new { data = response, message = "Meal retrieved successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
			}
		}

		// PUT: api/Meal/{id}
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateMeal(int id, [FromBody] CreateMealRequest updateMealRequest)
		{
			if (!ModelState.IsValid)
				return BadRequest(new { message = "Invalid model state." });

			try
			{
				var mealBeforeUpdate = await _mealServices.GetByID(id);
				if (mealBeforeUpdate == null)
				{
					return NotFound(new { message = $"Meal with ID {id} not found." });
				}

				var mealToUpdate = _mapper.Map<Meal>(updateMealRequest);
				mealToUpdate.MealID = id;
				mealToUpdate.DailyMealID = mealBeforeUpdate.DailyMealID;

				// Ensure the previous entity instance is detached
				_context.Entry(mealBeforeUpdate).State = EntityState.Detached;

				_mealServices.Update(mealToUpdate);
				return Ok(new { message = "Meal updated successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
			}
		}



		// DELETE: api/Meal/{id}
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteMeal(int id)
		{
			try
			{
				await _mealServices.Delete(id);
				return Ok(new { message = "Meal Deleted successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
			}
		}
	}
}
