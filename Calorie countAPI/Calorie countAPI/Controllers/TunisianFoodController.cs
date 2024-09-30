using AutoMapper;
using CalorieCount.Core.Entites;
using CalorieCount.Core.Interfaces;
using CalorieCount.Core.Mapping.TunisianFoodMapping;
using CalorieCount.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calorie_countAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TunisianFoodController : ControllerBase
	{
		private readonly ITunisianFoodServices _tunisianFoodService;
		private readonly IMapper _mapper;

		public TunisianFoodController(ITunisianFoodServices tunisianFoodService, IMapper mapper)
		{
			_tunisianFoodService = tunisianFoodService;
			_mapper = mapper;
		}

		// GET: api/TunisianFood
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TunisianFood>>> Get()
		{
			try
			{
				var tunisianFoods = await _tunisianFoodService.GetAll();
				return Ok(tunisianFoods);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération des plats tunisiens." });
			}
		}

		// GET api/TunisianFood/5
		[HttpGet("{id}")]
		public async Task<ActionResult<TunisianFood>> GetByID(int id)
		{
			try
			{
				var tunisianFood = await _tunisianFoodService.GetByID(id);
				if (tunisianFood == null)
				{
					return NotFound(new { message = "Le plat tunisien n'a pas été trouvé." });
				}
				return Ok(tunisianFood);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Une erreur est survenue lors de la récupération du plat tunisien." });
			}
		}

		// POST api/TunisianFood
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateTunisianFoodRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var tunisianFood = _mapper.Map<TunisianFood>(request);
				var createdTunisianFood = await _tunisianFoodService.Add(tunisianFood);
				return Ok(createdTunisianFood);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Une erreur est survenue lors de la création du plat tunisien." });
			}
		}

		// PUT api/TunisianFood/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] CreateTunisianFoodRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var tunisianFood = await _tunisianFoodService.GetByID(id);
				if (tunisianFood == null)
				{
					return NotFound(new { message = "Le plat tunisien n'a pas été trouvé." });
				}

				_mapper.Map(request, tunisianFood);
				_tunisianFoodService.Update(tunisianFood);

				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Une erreur est survenue lors de la mise à jour du plat tunisien." });
			}
		}

		// DELETE api/TunisianFood/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var tunisianFood = await _tunisianFoodService.GetByID(id);
				if (tunisianFood == null)
				{
					return NotFound(new { message = "Le plat tunisien n'a pas été trouvé." });
				}

				await _tunisianFoodService.Delete(id);
				return Ok(new { message = "Le plat tunisien a été supprimé avec succès." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Une erreur est survenue lors de la suppression du plat tunisien." });
			}
		}
	}
}
