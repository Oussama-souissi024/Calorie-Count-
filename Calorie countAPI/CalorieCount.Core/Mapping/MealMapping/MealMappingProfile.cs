using AutoMapper;
using CalorieCount.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Mapping.MealMapping
{
	public class MealMappingProfile : Profile
	{
		public MealMappingProfile()
		{
			CreateMap<CreateMealRequest, Meal>()
				.ForMember(dest => dest.MealID, op => op.Ignore())
				.ForMember(dest => dest.DailyMeal, op => op.Ignore())
				.ForMember(dest => dest.DailyMealID, op => op.Ignore());

			CreateMap<Meal, GetMealResponse>()
				.ForMember(dest => dest.Date, op => op.MapFrom(src => src.DailyMeal.Date.ToString()));
		}
	}
}

