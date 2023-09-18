using AutoMapper;
using SimpleWebApp.Api.Models;
using SimpleWebApp.Storage.EmployeeModels;

namespace SimpleWebApp.Api
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<EmployeeDto, Employee>().ReverseMap();
            CreateMap<EmployeeCreateDto, EmployeeCreate>().ReverseMap();
            CreateMap<EmployeeUpdateDto, EmployeeUpdate>().ReverseMap();
            CreateMap<GetEmployeePageDto, EmployeePage>().ReverseMap();
        }
    }
}
