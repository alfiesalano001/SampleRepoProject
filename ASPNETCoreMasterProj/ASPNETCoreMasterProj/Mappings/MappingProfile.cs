using AutoMapper;
using DomainModels.Entity;
using DomainModels.ViewModel.Menu;

namespace Repositories.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MenuDetailsViewModel, Menu>();
        }
    }
}
