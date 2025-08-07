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
        public string CargoCompany { get; set; }
        public decimal CargoCost { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class GetOrderResponse
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; } 
        public string CargoCompany { get; set; }
        public decimal CargoPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
    public class UpdateOrderRequest
    {
        public int CargoCompany { get; set; }
        public List<UpdateOrderItemDto> Items { get; set; }
    }
    public class UpdateOrderItemDto
    {
        public Guid OrderId { get; set; } 
        public int Quantity { get; set; }
    }
}

