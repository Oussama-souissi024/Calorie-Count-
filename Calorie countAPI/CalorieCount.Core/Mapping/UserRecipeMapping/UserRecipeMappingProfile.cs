using AutoMapper;
using CalorieCount.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCount.Core.Mapping.UserRecipeMapping
{
	public class UserRecipeMappingProfile : Profile
	{
        public UserRecipeMappingProfile()
        {
            CreateMap<CreateUserRecipeRequest, UserRecipe>()
                .ForMember(dest => dest.UserRecipeID, op => op.Ignore())
                .ForMember(dest => dest.UserID, op => op.Ignore())
                .ForMember(dest => dest.Date, op => op.Ignore());

            CreateMap<UserRecipe, GetUserRecipeResponce>();
                
        }
    }
}
