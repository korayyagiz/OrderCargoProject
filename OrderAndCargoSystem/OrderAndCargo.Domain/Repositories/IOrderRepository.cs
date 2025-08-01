﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrderAndCargo.Domain.Entities;

namespace OrderAndCargo.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<Order?> GetOrderWithItemsAsync(Guid id);

        Task AddAsync(Order order);
        void Remove(Order order);

        Task SaveChangesAsync();
        List<Order> Orders { get; }
    }

}

