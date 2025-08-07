using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Infrastructure.Data;

namespace OrderAndCargo.API.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class ProductsController : ControllerBase
        {

         private readonly IMediator _mediator; 
         private readonly OrderAndCargoDbContext _context;

            public ProductsController(IMediator mediator, OrderAndCargoDbContext context)
            {
            _mediator = mediator;
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
    }
}
