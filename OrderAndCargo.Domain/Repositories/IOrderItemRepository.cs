using OrderAndCargo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAndCargo.Domain.Repositories
{
    public interface IOrderItemRepository
    {
        void RemoveRange(List<OrderItem> items);
        Task SaveChangesAsync();
    }
}
