using MediatR;
using OrderAndCargo.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAndCargo.Application.Commands
{
    public class UpdateOrderCommand : IRequest<Unit>

    {
        public Guid Id { get; set; }
        public string CargoCompany { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new();
    }

}
