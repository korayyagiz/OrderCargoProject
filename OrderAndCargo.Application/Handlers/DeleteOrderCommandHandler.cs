using MediatR;
using Microsoft.EntityFrameworkCore;
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
        // OrderItemRepository eklemem gerekiyor
        private readonly IOrderItemRepository _orderItemRepository;

        public DeleteOrderCommandHandler(IOrderRepository repository,/*buraya tanımlama*/IOrderItemRepository orderItemRepository, OrderAndCargoDbContext context)
        {
            _repository = repository;
            _orderItemRepository = orderItemRepository;
        }


        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            try { await _repository.GetByIdAsync(request.Id); }
            catch (Exception ex) 
            { }

            var order = await _repository.GetByIdAsync(request.Id);
            if (order is null) throw new Exception("Order not found");

            // order dan orderi silbilirsin
            // orderItemRepository eklencek
            // _repository.Remove(order.OrderItems);
            _orderItemRepository.RemoveRange(order.OrderItems.ToList());
            _repository.Remove(order);
            await _orderItemRepository.SaveChangesAsync();
            await _repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
