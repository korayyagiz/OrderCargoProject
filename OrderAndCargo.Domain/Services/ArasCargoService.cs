/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderAndCargo.Domain.Entities;


namespace OrderAndCargo.Domain.Services
{
    public class ArasCargoService : ICargoService
    {
        public decimal CalculatePrice(Entities.Order order) => order.TotalPrice * 0.07m + 8;
        public int CalculateDeliveryDays(Entities.Order order) => 2;
    }
}
*/

using OrderAndCargo.Domain.Entities;

public class ArasCargoService : ICargoService
{
    public decimal CalculatePrice(Order order)
    {
        return 40 + order.OrderItems.Count * 4; // örnek hesap
    }

    public int CalculateDeliveryDays(Order order)
    {
        return 4;
    }
}

