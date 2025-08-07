using MediatR;
using OrderAndCargo.Application.Dto;
using System.Collections.Generic;

namespace OrderAndCargo.Application.Queries
{
    public class GetOrdersQuery : IRequest<List<GetOrderResponse>>
    {
    }
}
