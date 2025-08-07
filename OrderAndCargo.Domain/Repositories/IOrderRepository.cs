using OrderAndCargo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<List<Order>> GetAllWithItemsAsync();
        Task<List<Order>> GetOrdersWithItemsAsync();
    }
}
