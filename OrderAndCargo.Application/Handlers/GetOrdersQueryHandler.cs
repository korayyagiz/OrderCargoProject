using MediatR;
using OrderAndCargo.Application.Dto;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderAndCargo.Application.Queries;


namespace OrderAndCargo.Application.Handlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<GetOrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<List<GetOrderResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllWithItemsAsync();

            var response = orders.Select(o => new GetOrderResponse
            {
                Id = o.Id,
                CargoCompany = o.CargoCompany.ToString(),
                CargoPrice = o.CargoPrice,
                OrderDate = o.OrderDate,
                TotalPrice = o.OrderItems.Sum(i => i.UnitPrice * i.Quantity) + o.CargoPrice
            }).ToList();

            return response;
        }
    }
}
