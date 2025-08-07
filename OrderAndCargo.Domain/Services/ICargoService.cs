/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAndCargo.Domain.Services
{
    public interface ICargoService
    {
        decimal CalculatePrice(OrderAndCargo.Domain.Entities.Order order);
        int CalculateDeliveryDays(OrderAndCargo.Domain.Entities.Order order);
    }

}
*/

using OrderAndCargo.Domain.Entities;

public interface ICargoService
{
    decimal CalculatePrice(Order order);
    int CalculateDeliveryDays(Order order);
}
