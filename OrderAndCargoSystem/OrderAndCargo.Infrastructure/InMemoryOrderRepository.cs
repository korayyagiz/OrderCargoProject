using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Repositories;

namespace OrderAndCargo.Infrastructure.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public List<Order> Orders => _orders;

        public Task<Order?> GetByIdAsync(Guid id) =>
            Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));

        public Task<Order?> GetOrderWithItemsAsync(Guid id) =>
            Task.FromResult(_orders.FirstOrDefault(o => o.Id == id)); 

        public Task AddAsync(Order order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }

        public void Remove(Order order) => _orders.Remove(order);

        public Task SaveChangesAsync() => Task.CompletedTask;
    }

}
