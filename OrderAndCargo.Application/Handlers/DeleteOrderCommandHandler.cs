using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Domain.Repositories;
using OrderAndCargo.Infrastructure.Data;
using OrderAndCargo.Infrastructure.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace OrderAndCargo.Application.Handlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderRepository _repository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;
        public DeleteOrderCommandHandler(IOrderRepository repository,/*buraya tanımlama*/IOrderItemRepository orderItemRepository, ILogger<DeleteOrderCommandHandler> logger, OrderAndCargoDbContext context)
        {
            _repository = repository;
            _orderItemRepository = orderItemRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Delete işlemi başladı. ID: {OrderId}", request.Id);

            try
            {
                var order = await _repository.GetByIdAsync(request.Id);
                if (order is null)
                    throw new Exception("Order not found");

                _repository.Remove(order);
                await _repository.SaveChangesAsync();

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete işleminde hata oluştu. ID: {OrderId}", request.Id);
                throw; 
            }
        }
    }
}
