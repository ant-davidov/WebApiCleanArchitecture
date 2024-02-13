using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.Models.Identity;
using Microsoft.AspNetCore.Http;
using Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly LeaveManagementDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILeaveAllocationRepository _leaveAllocationRepository;
        private ILeaveTypeRepository _leaveTypeRepository;
        private ILeaveRequestsRepository _leaveRequestRepository;
      


        public UnitOfWork(LeaveManagementDbContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
           _httpContextAccessor = httpContextAccessor;
           
        }

        public ILeaveAllocationRepository LeaveAllocationRepository =>
            _leaveAllocationRepository ??= new LeaveAllocationRepository(_context);
        public ILeaveTypeRepository LeaveTypeRepository =>
            _leaveTypeRepository ??= new LeaveTypeRepository(_context);
        public ILeaveRequestsRepository LeaveRequestRepository =>
            _leaveRequestRepository ??= new LeaveRequestRepository(_context);

      
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.Uid)?.Value;
            await _context.SaveChangesAsync(username);
        }
           
        
    }
}
