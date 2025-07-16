/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OrderAndCargo.Infrastructure.Data;

namespace OrderAndCargo.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OrderAndCargoDbContext>
    {
        public OrderAndCargoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderAndCargoDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=OrderAndCargoDb;Trusted_Connection=True;");

            return new OrderAndCargoDbContext(optionsBuilder.Options);
        }
    }
}

