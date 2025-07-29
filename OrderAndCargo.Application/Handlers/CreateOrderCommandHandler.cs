/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/


using MediatR;
using Microsoft.Extensions.Logging;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Repositories;
using OrderAndCargo.Domain.Services;
using OrderAndCargo.Infrastructure.Data;


namespace OrderAndCargo.Application.Handlers
{

    public class CreateOrderCommandHandler : IRequestHandler<Commands.CreateOrderCommand, Guid>
    {


        private readonly IEnumerable<ICargoService> _cargoServices;

        private readonly OrderAndCargoDbContext _context;

        private readonly ILogger<CreateOrderCommandHandler> _logger;

        
        public CreateOrderCommandHandler(OrderAndCargoDbContext context, IEnumerable<ICargoService> cargoServices, ILogger<CreateOrderCommandHandler> logger) // IOrderRepository orderRepository)
        {
            _context = context;
            _cargoServices = cargoServices;
            _logger = logger;
        }
        

        public async Task<Guid> Handle(Commands.CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Create işlemi başladı. CargoCompany: {CargoCompany}", request.CargoCompany);

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    OrderDate = DateTime.UtcNow,
                    CargoCompany = request.CargoCompany,
                    OrderItems = request.Items.Select(i => new OrderItem

                    {
                        ProductId = i.ProductId,
                        ProductName = "ProductX",
                        Quantity = i.Quantity,
                        UnitPrice = 100
                    }).ToList()
                };

                order.TotalPrice = order.OrderItems.Sum(i => i.UnitPrice * i.Quantity);

                var cargoService = _cargoServices.FirstOrDefault(x =>
                    x.GetType().Name.StartsWith(request.CargoCompany));

                if (cargoService != null)
                {
                    order.CargoPrice = cargoService.CalculatePrice(order);
                }

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();


                return order.Id;


                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create işleminde hata oluştu.");
                throw;
            }

        }
           
    }
}

