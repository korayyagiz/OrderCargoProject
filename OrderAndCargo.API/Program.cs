using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderAndCargo.Application;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Application.Dto;
using OrderAndCargo.Application.Handlers;
using OrderAndCargo.Domain.Repositories;
using OrderAndCargo.Infrastructure.Data;
using OrderAndCargo.Infrastructure.Repositories;
using static OrderAndCargo.Application.Handlers.UpdateOrderCommandHandler;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteOrderCommandValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderAndCargoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>());

builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, EfOrderItemRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommandHandler).Assembly));

var app = builder.Build();

app.UseMiddleware<OrderAndCargo.API.Middlewares.ApiExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

Log.CloseAndFlush();

