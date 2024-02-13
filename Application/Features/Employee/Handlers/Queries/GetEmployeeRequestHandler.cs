using Application.Contracts.Identity;
using Application.DTOs.Employee;
using Application.DTOs.Exceptions;
using Application.Features.Employee.Requests.Queries;
using Application.Models.Identity;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employee.Handlers.Queries
{
    public class GetEmployeeRequestHandler : IRequestHandler<GetEmployeeRequest, EmployeeDetailDTO>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetEmployeeRequestHandler(IUserService userService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EmployeeDetailDTO> Handle(GetEmployeeRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(
                    q => q.Type == CustomClaimTypes.Uid)?.Value ?? throw new BadRequestException("problem authentication");
            return _mapper.Map<EmployeeDetailDTO>(await _userService.GetEmployeeAsync(userId));
        }
    }
}
