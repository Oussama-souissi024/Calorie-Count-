using AutoMapper;
using CalorieCount.Core.Entites;
using CalorieCount.Core.Interfaces;
using CalorieCount.Core.Mapping.FoodMapping;
using CalorieCount.Services;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Calorie_countAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FoodController : ControllerBase
	{
		private readonly IFoodSevices _foodServices;
		private readonly IMapper _mapper;

		public FoodController(IFoodSevices foodServices, IMapper mapper)
		{
			_foodServices = foodServices;
			_mapper = mapper;
		}

		// GET: api/Food
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Food>>> Get()
		{
			try
			{
				var foods = await _foodServices.GetAll();
				return Ok(foods);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while fetching foods." });
			}
		}

		// GET api/Food/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Food>> GetByID(int id)
		{
			try
			{
				var food = await _foodServices.GetByID(id);
				if (food == null)
				{
					return NotFound(new { message = "Food not found." });
				}
				return Ok(food);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while fetching the food." });
			}
		}

		// POST api/<FoodController>/upload
		[HttpPost("upload")]
		public async Task<IActionResult> Upload(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest(new { message = "No file uploaded." });

			try
			{
				using (var stream = new MemoryStream())
				{
					await file.CopyToAsync(stream);

					using (var package = new ExcelPackage(stream))
					{
						var worksheet = package.Workbook.Worksheets[0]; // assuming first worksheet
						int rowCount = worksheet.Dimension.Rows;

						for (int row = 2; row <= rowCount; row++) // start from row 2 to skip header
						{
							var food = new Food
							{
								Name = worksheet.Cells[row, 1].Text.Trim(),
								Calories = double.TryParse(worksheet.Cells[row, 2].Text.Trim(), out double calories) ? (double?)calories : null,
								Carbohydrates = double.TryParse(worksheet.Cells[row, 3].Text.Trim(), out double carbs) ? (double?)carbs : null,
								Fiber = double.TryParse(worksheet.Cells[row, 4].Text.Trim(), out double fiber) ? (double?)fiber : null,
								Fats = double.TryParse(worksheet.Cells[row, 6].Text.Trim(), out double fats) ? (double?)fats : null,
								Proteins = double.TryParse(worksheet.Cells[row, 7].Text.Trim(), out double proteins) ? (double?)proteins : null,
								Category = "Vegetables" // or derive this value from a column in Excel
							};

							await _foodServices.Add(food);
						}
					}
				}

				return Ok(new { message = "File uploaded and data saved." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while uploading the file." });
			}
		}

		// POST api/<FoodController>/Add Food
		[HttpPost("Add Food")]
		public async Task<IActionResult> Post([FromBody] CreateFoodRequest FoodRequest)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var AddedFood = _mapper.Map<Food>(FoodRequest);
				var result = await _foodServices.Add(AddedFood);
				if (result != null)
					return Ok(new { message = "Food added successfully and data saved."});
				else
					return StatusCode(400, new { message = "Bad Request" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while adding the food." });
			}
		}


		// PUT api/Food/5
		[HttpPut("{id}")]
		public IActionResult Put(string id, [FromBody] Food food)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				if (id != food.Food_ID.ToString())
				{
					return BadRequest(new { message = "Mismatched food ID." });
				}

				_foodServices.Update(food);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while updating the food." });
			}
		}

		// DELETE api/Food/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var food = await _foodServices.GetByID(id);
				if (food == null)
				{
					return NotFound(new { message = "Food not found." });
				}

				await _foodServices.Delete(id);
				return Ok(new { message = "Food deleted successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while deleting the food." });
			}
		}
	}
}
