using MediatR;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Application.Dto;
using OrderAndCargo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OrderAndCargo.Domain.Repositories;



namespace OrderAndCargo.Application.Handlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository _repository;

        public UpdateOrderCommandHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
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


            return Unit.Value;
        }
    }
}
