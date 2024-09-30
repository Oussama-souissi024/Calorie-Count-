using AutoMapper;
using CalorieCount.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Mapping.FoodMapping
{
	public class FoodMappingProfile : Profile
	{
        public FoodMappingProfile()
        {
			CreateMap<CreateFoodRequest, Food>()
				 .ForMember(dest => dest.Food_ID , op => op.Ignore());
		}
    }
}
