using OrderAndCargo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderAndCargo.Domain.Entities
{
     public class Order
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        [Column(TypeName = "nvarchar(24)")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CargoCompanies CargoCompany { get; set; }
        public decimal CargoPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal CargoCost { get; set; }
    }
}

