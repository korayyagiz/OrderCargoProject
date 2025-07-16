using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAndCargo.Domain.Services
{
    public class MNGCargoService : ICargoService
    {
        public decimal CalculatePrice(Entities.Order order) => order.TotalPrice * 0.04m + 12;
        public int CalculateDeliveryDays(Entities.Order order) => 4;
    }
}

