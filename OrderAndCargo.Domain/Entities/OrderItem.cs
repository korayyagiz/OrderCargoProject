using OrderAndCargo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAndCargo.Domain.Entities
{    
    public class OrderItem
    {
        public Guid Id { get; set; } 
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Order? Order { get; set; } = null!;
        public Product? Product { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

