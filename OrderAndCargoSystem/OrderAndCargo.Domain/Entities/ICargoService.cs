﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAndCargo.Domain.Services
{
    public interface ICargoService
    {
        decimal CalculatePrice(Entities.Order order);
        int CalculateDeliveryDays(Entities.Order order);
    }
}

