using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAndCargo.Application.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CargoCompany { get; set; } = string.Empty; 
        public List<OrderItemDto> Items { get; set; } = new();  
    }


    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

