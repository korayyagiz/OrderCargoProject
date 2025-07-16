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
    public class DeleteOrderCommandHandler
    : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderRepository _repo;
        private readonly OrderAndCargoDbContext _context;   

        private readonly IOrderRepository _repository;

        public DeleteOrderCommandHandler(IOrderRepository repository, OrderAndCargoDbContext context)
        {
            _repository = repository;
            _context = context;
        }


        public async Task<Unit> Handle(DeleteOrderCommand request,
                                       CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id);
            if (order is null) throw new Exception("Order not found");


            _context.OrderItems.RemoveRange(order.OrderItems); 
            _repo.Remove(order);
            await _repo.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
