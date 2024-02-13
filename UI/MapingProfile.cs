using AutoMapper;
using MVC.Models;
using MVC.Services.Base;

namespace MVC
{
    public class MapingProfile : Profile
    {
        public MapingProfile()
        {
            CreateMap<CreateLeaveTypeDTO, CreateLeaveTypeVM>().ReverseMap();
            CreateMap<LeaveTypeDTO, LeaveTypeVM>().ReverseMap();
        }
    }
}
