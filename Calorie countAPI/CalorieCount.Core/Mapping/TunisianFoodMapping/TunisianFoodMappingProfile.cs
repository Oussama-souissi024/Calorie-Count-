using AutoMapper;
using CalorieCount.Core.Entites;

namespace CalorieCount.Core.Mapping.TunisianFoodMapping
{
	public class TunisianFoodMappingProfile : Profile
	{
        public TunisianFoodMappingProfile()
        {
            CreateMap<CreateTunisianFoodRequest, TunisianFood>()
                .ForMember(dest => dest.TunisianFoodID, op => op.Ignore());
        }
    }
}
