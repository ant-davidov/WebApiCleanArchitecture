using Application;
using Infrastructure;
using Persistence;
using Identity;
using Microsoft.OpenApi.Models;
using WebApiCleanArchitecture.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DbContexts;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
AddSwaggerDoc(builder.Services);
ConfiguratioLogger();

builder.Host.UseSerilog();
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.ConfigurePersistenceServices(builder.Environment, builder.Configuration);
builder.Services.ConfigureIdentityServices(builder);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers().RequireAuthorization();



var scope = app.Services.CreateScope();
try
{
    var leaveManagementContext = scope.ServiceProvider.GetRequiredService<LeaveManagementDbContext>();
    var leaveManagementDatabaseCreator = (leaveManagementContext.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);


    var leaveIdentityManagementContext = scope.ServiceProvider.GetRequiredService<LeaveManagementIdentityDbContext>();
    var leaveIdentityManagementDatabaseCreator = (leaveIdentityManagementContext.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);

    if (!leaveManagementDatabaseCreator.HasTables())
    {
        leaveManagementDatabaseCreator.CreateTables();
        leaveIdentityManagementDatabaseCreator.CreateTables();
    }
}
catch { }



//Debug.WriteLine(builder.Configuration["ELASTIC_URI"]);

app.Run();


void AddSwaggerDoc(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });

        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "HR Leave Management Api",

        });

    });
}

void ConfiguratioLogger()
{
    var uri = String.Empty;
    if (builder.Environment.IsDevelopment())
        uri = builder.Configuration["ElasticConfiguration:Uri"];
    else
        uri = builder.Configuration["ELASTIC_URI"];
    Log.Logger = new LoggerConfiguration()
      .Enrich.FromLogContext()
      .Enrich.WithExceptionDetails()
      .WriteTo.Debug()
      .WriteTo.Console()
      .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(uri))
      {
          AutoRegisterTemplate = true,
          IndexFormat = $"{builder.Configuration["ApplicationName"]}-logs-{builder.Environment.EnvironmentName?.ToLower().Replace('.', '-')}-{DateTime.Now:yyyy-MM}",
      })
       .CreateLogger();
}
