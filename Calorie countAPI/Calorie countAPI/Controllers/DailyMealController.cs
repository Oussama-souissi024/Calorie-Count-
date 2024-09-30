using AutoMapper;
using CalorieCount.Core.Entites.Date_Entity;
using CalorieCount.Core.Interfaces;
using CalorieCount.Core.Mapping.DailyMealMapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Calorie_countAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = "SimplesUserOnly")]
	public class DailyMealController : ControllerBase
	{
		private readonly IDailyMealServices _dailyMealService;
		private readonly IMapper _mapper;
		private readonly UserManager<IdentityUser> _userManager;

		public DailyMealController(IDailyMealServices dailyMealService, IMapper mapper, UserManager<IdentityUser> userManager)
		{
			_dailyMealService = dailyMealService;
			_mapper = mapper;
			_userManager = userManager;
		}

		// GET: api/DailyMeal
		[HttpGet("By Date")]
		public async Task<ActionResult<GetDailyMealResponse>> GetByUserIDandDate([FromQuery] DateInputModel dateInput)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Unauthorized();
			}

			try
			{
				var dateOnly = new DateOnly(dateInput.Year, dateInput.Month, dateInput.Day);

				var dailyMeal = await _dailyMealService.GetByUserIDandDateAsync(user.Id, dateOnly);
				if (dailyMeal == null)
				{
					return NotFound();
				}

				var response = _mapper.Map<GetDailyMealResponse>(dailyMeal);
				return Ok(response);
			}
			catch (Exception)
			{
				return BadRequest("Invalid date provided.");
			}
		}


		// GET: api/DailyMeal/userId
		[HttpGet("All")]
		public async Task<ActionResult<IEnumerable<GetDailyMealResponse>>> GetAllByUserID()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Unauthorized();
			}

			var dailyMeals = await _dailyMealService.GetAllByUserIDAsync(user.Id);
			if (dailyMeals == null || !dailyMeals.Any())
			{
				return NotFound("No daily meals found for this user.");
			}

			var response = _mapper.Map<IEnumerable<GetDailyMealResponse>>(dailyMeals);
			return Ok(response);
		}
	}
}
