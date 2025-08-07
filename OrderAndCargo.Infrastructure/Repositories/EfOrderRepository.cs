using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Repositories;
using OrderAndCargo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace OrderAndCargo.Infrastructure.Repositories
{
    public class EfOrderRepository : GenericRepository<Order>, IOrderRepository

    {
        private readonly OrderAndCargoDbContext _context;
        protected readonly DbSet<Order> _dbSet;
        public EfOrderRepository(OrderAndCargoDbContext context) : base(context) { _context = context; _dbSet = context.Set<Order>(); } 

        public async Task<List<Order>> GetAllWithItemsAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems) 
            .ToListAsync();
    }
        public List<Order> Orders => _context.Orders.ToList();
        public async Task<Order?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public async Task<Order?> GetOrderWithItemsAsync(Guid id) => await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public void Remove(Order order) => _context.Orders.Remove(order);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
        public Task<List<Order>> GetOrdersWithItemsAsync()
        {
            throw new NotImplementedException();
        }
    }
    public class EfOrderItemRepository : IOrderItemRepository
    {
        private readonly OrderAndCargoDbContext _context;

        public EfOrderItemRepository(OrderAndCargoDbContext context)
        {
            _context = context;
        }
        public void RemoveRange(List<OrderItem> items)
        {
            _context.OrderItems.RemoveRange(items);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

