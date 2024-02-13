using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.Entities
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.HasData(
                new LeaveType
                {
                    Id = 1,
                    DefaultDays = 10,
                    Name = "Vacation",
                    LastModifiedBy = "user",
                    DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    LastModifiedDate = DateTime.SpecifyKind(DateTime.Now.AddMinutes(10), DateTimeKind.Utc),
                    CreatedBy = "user"

                },
                new LeaveType
                {
                    Id = 2,
                    DefaultDays = 12,
                    Name = "Sick",
                    LastModifiedBy = "user",
                    DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    LastModifiedDate = DateTime.SpecifyKind(DateTime.Now.AddMinutes(10), DateTimeKind.Utc),
                    CreatedBy = "user"
                }
            ); ;
        }
    }
}
