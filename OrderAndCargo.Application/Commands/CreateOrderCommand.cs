using MediatR;
using OrderAndCargo.Application.Dto;
using OrderAndCargo.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OrderAndCargo.Application.Commands
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public CargoCompanies CargoCompany { get; set; }
        public List<OrderItemCommand> Items { get; set; }

    }
    public class OrderItemCommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

