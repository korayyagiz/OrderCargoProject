/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/


using OrderAndCargo.Infrastructure.Data;
using MediatR;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Services;


namespace OrderAndCargo.Application.Handlers
{

    public class CreateOrderCommandHandler : IRequestHandler<Commands.CreateOrderCommand, Guid>
    {

        private readonly IEnumerable<ICargoService> _cargoServices;

        private readonly OrderAndCargoDbContext _context;

        public CreateOrderCommandHandler(OrderAndCargoDbContext context, IEnumerable<ICargoService> cargoServices)
        {
            _context = context;
            _cargoServices = cargoServices;
        }


        public CreateOrderCommandHandler(IEnumerable<ICargoService> cargoServices)
        {
            _cargoServices = cargoServices;
        }

        public async Task<Guid> Handle(Commands.CreateOrderCommand request, CancellationToken cancellationToken)
        {
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
    }
}

