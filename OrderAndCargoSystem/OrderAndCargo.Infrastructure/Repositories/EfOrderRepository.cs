
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Repositories;
using OrderAndCargo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace OrderAndCargo.Infrastructure.Repositories
{
    public class EfOrderRepository : IOrderRepository
    {
        private readonly OrderAndCargoDbContext _context;
        public EfOrderRepository(OrderAndCargoDbContext context) => _context = context;

        public List<Order> Orders => _context.Orders.ToList();

        public async Task<Order?> GetByIdAsync(Guid id) => await _context.Orders
                  .Include(o => o.OrderItems)        // update / delete’te de lazım
                  .AsNoTracking()                    
                  .FirstOrDefaultAsync(o => o.Id == id);


        public async Task<Order?> GetOrderWithItemsAsync(Guid id) => await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public void Remove(Order order) => _context.Orders.Remove(order);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }

}
