using AutoMapper;
using MVC.Models;
using MVC.Services.Base;

namespace MVC
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateLeaveTypeDTO, CreateLeaveTypeVM>().ReverseMap();
            CreateMap<CreateLeaveRequestDTO, CreateLeaveRequestVM>().ReverseMap();
            CreateMap<LeaveRequestDTO, LeaveRequestVM>().ReverseMap();
            CreateMap<LeaveRequestListDTO, LeaveRequestVM>();
            CreateMap<EmployeeVM, EmployeeDTO>().ReverseMap();  
            CreateMap<LeaveTypeDTO, LeaveTypeVM>().ReverseMap();
            CreateMap<LeaveAllocationDTO, LeaveAllocationVM>().ReverseMap();
            CreateMap<RegisterVM, RegistrationRequest>().ReverseMap();
            CreateMap<EmployeeVM, EmployeeDetailDTO>().ReverseMap();           
        }
    }
}
