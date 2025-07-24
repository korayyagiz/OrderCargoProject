using MediatR;
using Microsoft.Extensions.Logging;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Application.Dto;
using OrderAndCargo.Domain.Entities;
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
            _logger.LogInformation("Update işlemi başladı. ID: {OrderId}, Kargo Şirketi: {CargoCompany}", request.Id, request.CargoCompany);

            try
            {
                var order = await _repository.GetByIdAsync(request.Id);

                if (order == null)
                    throw new Exception("Order not found!");

                order.CargoCompany = request.CargoCompany;

                order.OrderItems = request.Items.Select(i => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                }).ToList();


                await _repository.AddAsync(order);
                await _repository.SaveChangesAsync();

                _logger.LogInformation("Update işlemi başarıyla tamamlandı. ID: {OrderId}, Toplam Fiyat: {TotalPrice}", order.Id, order.TotalPrice);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update işleminde hata oluştu! ID: {OrderId}, Kargo Şirketi: {CargoCompany}", request.Id, request.CargoCompany);
                throw;
            }
        }
    }
}
