using Application.DTOs.Employee;
using Application.DTOs.LeaveAllocation;
using Application.DTOs.LeaveRequest;
using Application.DTOs.LeaveType;
using Application.Models.Identity;
using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    internal class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<LeaveRequest,LeaveRequestDTO>().ReverseMap();
            CreateMap<LeaveRequest, CreateLeaveRequestDTO>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestListDTO>().ReverseMap();
            CreateMap<LeaveType, LeaveTypeDTO>().ReverseMap();
            CreateMap<LeaveType, CreateLeaveTypeDTO>().ReverseMap();
            CreateMap<LeaveAllocation,LeaveAllocationDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDetailDTO>().ReverseMap();

        }
    }
}
