using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Enums;
using OrderAndCargo.Domain.Repositories;
using OrderAndCargo.Infrastructure.Data;

namespace OrderAndCargo.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<Commands.CreateOrderCommand, Guid>
    {
        private readonly IEnumerable<ICargoService> _cargoServices;
        private readonly OrderAndCargoDbContext _context;
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        public CreateOrderCommandHandler(OrderAndCargoDbContext context, IEnumerable<ICargoService> cargoServices, ILogger<CreateOrderCommandHandler> logger, IServiceProvider serviceProvider)
        {
            _context = context;
            _cargoServices = cargoServices;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Create işlemi başladı. CargoCompany: {CargoCompany}", request.CargoCompany);

                var orderItems = new List<OrderItem>();

                foreach (var i in request.Items)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == i.ProductId);
                    if (product == null)
                        throw new Exception($"Ürün bulunamadı: {i.ProductId}");

                    orderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = i.Quantity,
                        UnitPrice = product.Price
                    });
                }

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    OrderDate = DateTime.UtcNow,
                    CargoCompany = request.CargoCompany,
                    OrderItems = orderItems
                };

                ICargoService cargoService = order.CargoCompany switch
                {
                    CargoCompanies.MNG => _serviceProvider.GetRequiredService<MNGCargoService>(),
                    CargoCompanies.ARAS => _serviceProvider.GetRequiredService<ArasCargoService>(),
                    CargoCompanies.YURTICI => _serviceProvider.GetRequiredService<YurticiCargoService>(),
                    _ => throw new Exception("Kargo şirketi bilinmiyor!")
                };

                var cargoCost = cargoService.CalculatePrice(order);
                order.CargoCost = cargoCost;
                order.TotalPrice += cargoCost;

                order.TotalPrice = order.OrderItems.Sum(i => i.UnitPrice * i.Quantity);

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

