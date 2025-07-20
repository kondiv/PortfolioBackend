using Api.Extensions;
using Data.Interfaces;
using Data.Repositories;
using Data.Seeder;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Services.Interfaces;
using Services.Services;
using Services.Validators;
using Mapster;
using Services.Factories.RegistrationStrategyFactory;
using Services.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PortfolioContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<PortfolioContext>();

#region Other

builder.Services.AddJwtSettings(builder.Configuration);
builder.Services.AddTransient<RolePermissionSeeder>();

#endregion

#region Validators

builder.Services.AddSingleton<ProjectValidator>();
builder.Services.AddScoped<ISkillValidator, SkillValidator>();

#endregion

#region Factories

builder.Services.AddScoped<IRegistrationStrategyFactory, RegistrationStrategyFactory>();

#endregion

#region Services

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserSkillService, UserSkillService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

#endregion

#region Repositories

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUserSkillRepository, UserSkillRepository>();

#endregion

#region Mapper

builder.Services.AddMapster();
ApplicationUserMapper.Configure();

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:8080") // Vue dev server
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddCustomJwtBearer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var rolePermissionSeeder = scope.ServiceProvider.GetRequiredService<RolePermissionSeeder>();
    await rolePermissionSeeder.SeedDataAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("VueCorsPolicy");

app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
