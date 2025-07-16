using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Application.Dto;

namespace OrderAndCargo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        private static List<OrderDto> _orderList = new List<OrderDto>();


        public OrdersController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);

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

            _orderList.Add(order); 

            return Ok(orderId);
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(_orderList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateOrderCommand command)
        {
            command.Id = id; 

            await _mediator.Send(command);
            return NoContent();
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