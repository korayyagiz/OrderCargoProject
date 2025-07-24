using FluentValidation;
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

        private readonly IValidator<UpdateOrderCommand> _updateValidator;
        private readonly IValidator<DeleteOrderCommand> _deleteValidator;

        private readonly ILogger<OrdersController> _logger;
        public OrdersController(
            IOrderRepository orderRepository,
            IMediator mediator,
            IValidator<UpdateOrderCommand> updateValidator,
            IValidator<DeleteOrderCommand> deleteValidator,
            ILogger<OrdersController> logger)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
            _updateValidator = updateValidator;
            _deleteValidator = deleteValidator;
            _logger = logger;
        }


        /*
        public OrdersController(IOrderRepository orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }
        */


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
            var command = new UpdateOrderCommand
            {
                Id = id,
                CargoCompany = dto.CargoCompany,
                Items = dto.Items
            };

            var validationResult = await _updateValidator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // Hataları döner

            if (id == Guid.Empty)
            {
                return BadRequest("Geçersiz sipariş ID'si.");
            }

            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound($"ID {id} ile eşleşen sipariş bulunamadı.");
            }

            order.CargoCompany = dto.CargoCompany;
            order.OrderItems = dto.Items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                
            }).ToList();

            await _orderRepository.SaveChangesAsync(); 
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteOrderCommand { Id = id };

            var validationResult = await _deleteValidator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // Hataları döner

            await _mediator.Send(new DeleteOrderCommand(id));
            return NoContent();
        }
    }
}
