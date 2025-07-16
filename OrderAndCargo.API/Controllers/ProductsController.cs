using Microsoft.AspNetCore.Mvc;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Infrastructure.Data;

namespace OrderAndCargo.API.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class ProductsController : ControllerBase
        {
            private readonly OrderAndCargoDbContext _context;

            public ProductsController(OrderAndCargoDbContext context)
            {
                _context = context;
            }

            [HttpPut("{id}")]
            public IActionResult Update(Guid id, [FromBody] Product updatedProduct)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                    return NotFound();

                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;

                _context.SaveChanges();
                return Ok(product);
            }

            [HttpDelete("{id}")]
            public IActionResult Delete(Guid id)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                    return NotFound();

                _context.Products.Remove(product);
                _context.SaveChanges();
                return NoContent();
            }
        }

    }
// Katmanlı yapı kuruldu.