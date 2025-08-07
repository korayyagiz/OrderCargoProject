using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace OrderAndCargo.Application.Commands
{
    public class DeleteOrderCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DeleteOrderCommand(Guid id)
        {
            Id = id;
        }
        public DeleteOrderCommand() { }
    }
}


