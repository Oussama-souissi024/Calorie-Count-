using AutoMapper;
using CalorieCount.Core.Entites;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Mapping.DailyMealMapping
{
	public class DailyMealMappingProfile : Profile
	{
        public DailyMealMappingProfile()
        {
            CreateMap<DailyMeal, GetDailyMealResponse>();              
        }
    }
}
