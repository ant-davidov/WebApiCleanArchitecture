
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.LeaveAllocation
{
    public class CreateLeaveAllocationDTO : ILeaveAllocationDTO
    {
        public int LeaveTypeId { get; set; }
    }
}
