using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Application.Dto;
using OrderAndCargo.Application.Queries;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Enums;
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

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);


            return Ok(orderId);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _mediator.Send(new GetOrdersQuery());
            return Ok(orders);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderRequest request)
        {
            var command = new UpdateOrderCommand
            {
                Id = id,
                CargoCompany = request.CargoCompany,
                Items = request.Items
            };

            await _mediator.Send(command);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteOrderCommand { Id = id };

            var validationResult = await _deleteValidator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            await _mediator.Send(new DeleteOrderCommand(id));
            return NoContent();
        }
    }
}
