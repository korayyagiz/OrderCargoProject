using MediatR;
using Microsoft.Extensions.Logging;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Application.Dto;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Enums;
using OrderAndCargo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace OrderAndCargo.Application.Handlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;


        public UpdateOrderCommandHandler(IOrderRepository repository, ILogger<UpdateOrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update işlemi başladı. ID: {OrderId}, Kargo şirketi: {CargoCompany}", request.Id, request.CargoCompany);

            try
            {
                var order = await _repository.GetOrderWithItemsAsync(request.Id);
                if (order == null)
                    throw new Exception("Order not found");

                _logger.LogInformation("Sipariş bulundu. Güncelleniyor...");

                order.CargoCompany = Enum.Parse<CargoCompanies>(request.CargoCompany.ToString());

                var cargoPrice = order.CargoCompany switch
                {
                    CargoCompanies.ARAS => 44,
                    CargoCompanies.MNG => 33,
                    CargoCompanies.YURTICI => 22,
                    _ => throw new Exception("Geçersiz kargo şirketi")
                };
                order.CargoPrice = cargoPrice;

                foreach (var itemDto in request.Items)
                {
                    var existingItem = order.OrderItems.FirstOrDefault(i => i.OrderId == itemDto.OrderId);
                    if (existingItem != null)
                    {
                        _logger.LogInformation("Ürün güncelleniyor. OrderItemId: {OrderItemId}, Yeni Quantity: {Quantity}", itemDto.OrderId, itemDto.Quantity);
                        existingItem.Quantity = itemDto.Quantity;
                    }
                    else
                    {
                        _logger.LogWarning("OrderItem bulunamadı. ID: {OrderItemId}", itemDto.OrderId);
                    }
                }

                decimal totalProductPrice = 0;
                foreach (var item in order.OrderItems)
                {
                    totalProductPrice += item.Price * item.Quantity;
                }

                order.TotalPrice = totalProductPrice + order.CargoPrice;

                await _repository.SaveChangesAsync();

                _logger.LogInformation("Update işlemi başarıyla tamamlandı. ID: {OrderId}, Toplam Fiyat: {TotalPrice}", order.Id, order.TotalPrice);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update işleminde hata oluştu! ID: {OrderId}, Kargo şirketi: {CargoCompany}", request.Id, request.CargoCompany);
                throw;
            }
        }
    }
}
