using AuthServices.Data.Context;
using AuthServices.Data.Entities;
using AuthServices.Data.Interfaces;
using AuthServices.Data.Repositories;
using AuthServices.Services;
using AuthServices.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddIdentity<UsersEntity, IdentityRole>(x =>
{
   
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API v1");
    c.RoutePrefix = string.Empty; 
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

