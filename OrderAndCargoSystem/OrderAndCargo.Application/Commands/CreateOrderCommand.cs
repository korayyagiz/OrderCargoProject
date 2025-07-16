/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/


using MediatR;
using System;
using System.Collections.Generic;
using OrderAndCargo.Application.Dto;

namespace OrderAndCargo.Application.Commands
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public List<OrderItemDto> Items { get; set; }  
        public string CargoCompany { get; set; }
    }
}

