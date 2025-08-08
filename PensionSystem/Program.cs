using Application.Interfaces.Services;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Data;
using Infrastructure.HangfireJobs;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using PensionSystem.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "PensionSystem API", Version = "v1" });

    // Add JWT Bearer Authorization header
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Server=(localdb)\\mssqllocaldb;Database=PensionDb;Trusted_Connection=True;";
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(connectionString, new SqlServerStorageOptions
    {
        SchemaName = "Hangfire"
    }));
builder.Services.AddHangfireServer();

var jwtSecret = "Xh7R2p9M5qB8nY3tJ0fW6cL4vZ2kQ1aE";
var jwtLifetimeMinutes = 60;
var jwtLifetime = TimeSpan.FromMinutes(jwtLifetimeMinutes);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IEmployerService, EmployerService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IContributionService, ContributionService>();
builder.Services.AddScoped<IJwtTokenGenerator>(provider =>
    new JwtTokenGenerator(jwtSecret, jwtLifetime));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// ? Register your recurring job classes
builder.Services.AddTransient<ContributionValidationJob>();
builder.Services.AddTransient<EligibilityUpdateJob>();
builder.Services.AddTransient<FailedTransactionJob>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard(); // Optional: enables dashboard at /hangfire

using (var scope = app.Services.CreateScope())
{
    var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobs.AddOrUpdate<ContributionValidationJob>(
        "ContributionValidationJob",
        job => job.ExecuteAsync(),
        Cron.Daily);

    recurringJobs.AddOrUpdate<EligibilityUpdateJob>(
        "EligibilityUpdateJob",
        job => job.ExecuteAsync(),
        Cron.Daily);

    recurringJobs.AddOrUpdate<FailedTransactionJob>(
        "FailedTransactionJob",
        job => job.ExecuteAsync(),
        Cron.Daily);
}

app.Run();
