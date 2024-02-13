
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Hosting;
using Persistence.DbContexts;
using Application.Contracts.Persistence;
using Persistence.Repositories;

namespace Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
        {
            

            if (environment.IsDevelopment())
            {
                services.AddDbContext<LeaveManagementDbContext>(options =>options.UseSqlite("Data Source=helloapp.db"));
            }
            else
            {
                //services.AddDbContext<LeaveManagementDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
                services.AddDbContext<LeaveManagementDbContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__DEFAULT")));
            }
           

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            //services.AddScoped<ILeaveRequestsRepository, LeaveRequestRepository>();
            //services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();

            return services;
        }
    }
}
