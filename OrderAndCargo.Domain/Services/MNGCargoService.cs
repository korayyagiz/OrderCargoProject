/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderAndCargo.Domain.Entities;


namespace OrderAndCargo.Domain.Services
{
    public class MNGCargoService : ICargoService
    {
        public decimal CalculatePrice(Entities.Order order) => order.TotalPrice * 0.04m + 12;
        public int CalculateDeliveryDays(Entities.Order order) => 4;
    }
}
*/

using OrderAndCargo.Domain.Entities;

public class MNGCargoService : ICargoService
{
    public decimal CalculatePrice(Order order)
    {
        return 20 + order.OrderItems.Count * 2; // örnek hesap
    }

    public int CalculateDeliveryDays(Order order)
    {
        return 2;
    }
}

