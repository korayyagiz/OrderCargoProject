using Microsoft.EntityFrameworkCore;
using OrderAndCargo.Application;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Infrastructure.Data;
using static OrderAndCargo.Application.Handlers.UpdateOrderCommandHandler;
using OrderAndCargo.Domain.Repositories;
using OrderAndCargo.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddDbContext<OrderAndCargoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>());

builder.Services.AddScoped<IOrderRepository, InMemoryOrderRepository>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


