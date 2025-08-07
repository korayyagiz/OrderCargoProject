/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderAndCargo.Domain.Entities;


namespace OrderAndCargo.Domain.Services
{
    public class YurtiçiCargoService : ICargoService
    {
        public decimal CalculatePrice(Entities.Order order) => order.TotalPrice * 0.05m + 10;
        public int CalculateDeliveryDays(Entities.Order order) => 3;
    }
}   
*/

using OrderAndCargo.Domain.Entities;

public class YurticiCargoService : ICargoService
{
    public decimal CalculatePrice(Order order)
    {
        return 30 + order.OrderItems.Count * 3; // örnek hesap
    }

    public int CalculateDeliveryDays(Order order)
    {
        return 3;
    }
}

