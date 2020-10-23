using AutoMapper;
using ProjectPlutus.API.ViewModels;
using ProjectPlutus.Domain.Models;

namespace ProjectPlutus.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(vm => vm.Id, map => map.MapFrom(m => m.Id))
                .ForMember(vm => vm.FirstName, map => map.MapFrom(m => m.FirstName))
                .ForMember(vm => vm.LastName, map => map.MapFrom(m => m.LastName))
                .ForMember(vm => vm.Temperature, map => map.MapFrom(m => m.Temperature))
                .ForMember(vm => vm.CreatedAt, map => map.MapFrom(m => m.CreatedAt))
                .ForMember(vm => vm.ModifiedAt, map => map.MapFrom(m => m.ModifiedAt));

            CreateMap<EmployeeCreationViewModel, Employee>();
            CreateMap<EmployeeModificationViewModel, Employee>();
        }
    }
}
