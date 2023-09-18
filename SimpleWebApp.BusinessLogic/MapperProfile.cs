using AutoMapper;
using SimpleWebApp.BusinessLogic.Models;
using SimpleWebApp.Storage.EmployeeModels;

namespace SimpleWebApp.BusinessLogic
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
