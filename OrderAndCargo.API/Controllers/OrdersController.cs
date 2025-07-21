using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Application.Dto;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Repositories;

namespace OrderAndCargo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IOrderRepository _orderRepository;


       
        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);

            // command handlere taşınacak 
            var order = new OrderDto
            {
                Id = orderId,
                CargoCompany = command.CargoCompany,
                Items = command.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            

            return Ok(orderId);
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok();
        }

        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateOrderCommand command)
        {
            command.Id = id; 

            await _mediator.Send(command);
            return NoContent();
        }
        */

        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound("ID {id} ile eşleşen bir sipariş bulunamadı.");
        */

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderDto dto)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Geçersiz sipariş ID'si.");
            }

            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound($"ID {id} ile eşleşen sipariş bulunamadı.");
            }

            // güncelleme işlemleri altta
        

        // eski order'ı güncelleme
        order.CargoCompany = dto.CargoCompany;
            order.OrderItems = dto.Items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                
            }).ToList();

            await _orderRepository.SaveChangesAsync(); // tracking ediyor
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteOrderCommand(id));
            return NoContent();
        }
    }
}
// Katmanlı yapı kuruldu.