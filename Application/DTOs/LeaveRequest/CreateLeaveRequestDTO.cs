using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.LeaveRequest
{
    public class CreateLeaveRequestDTO  : ILeaveRequestDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LeaveTypeId { get; set; }
        public string RequestComments { get; set; }
       
    }
}
