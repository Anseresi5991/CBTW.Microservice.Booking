using CBTW.Microservice.Booking.Application.Mapper;
using CBTW.Microservice.Booking.Application.Services.Implementations;
using CBTW.Microservice.Booking.Application.Services.Interfaces;
using CBTW.Microservice.Booking.Domain.Reporitories;
using CBTW.Microservice.Booking.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
services.AddSingleton<IMongoClient>(s =>
{
    var connectionString = configuration["MongoDb:ConnectionString"];
    return new MongoClient(connectionString);
});

services.AddScoped<IMongoDatabase>(s =>
{
    var client = s.GetRequiredService<IMongoClient>();
    var databaseName = configuration["MongoDb:DatabaseName"];
    return client.GetDatabase(databaseName);
});

services.AddScoped<IAppointmentRepository, AppointmentRepository>();
services.AddScoped<IClientRepository, ClientRepository>();
services.AddScoped<IEmployeeRepository, EmployeeRepository>();
services.AddScoped<IAppointmentService, AppointmentService>();
services.AddScoped<IClientService, ClientService>();
services.AddScoped<IEmployeeService, EmployeeService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
