using Microsoft.Extensions.Logging;
using Moq;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Application.Dto;
using OrderAndCargo.Application.Handlers;
using OrderAndCargo.Domain.Entities;
using OrderAndCargo.Domain.Repositories;
using OrderAndCargo.Domain.Services;
using OrderAndCargo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;


namespace OrderAndCargo.Tests
{
    public class UpdateOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<ILogger<UpdateOrderCommandHandler>> _loggerMock;
        private readonly UpdateOrderCommandHandler _handler;

        public UpdateOrderCommandHandlerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _loggerMock = new Mock<ILogger<UpdateOrderCommandHandler>>();
            _handler = new UpdateOrderCommandHandler(_orderRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_OrderExists_UpdatesSuccessfully()
        {
            
            var orderId = Guid.NewGuid();
            var command = new UpdateOrderCommand
            {
                Id = orderId,
                CargoCompany = "Aras",
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = Guid.NewGuid(), Quantity = 2 }
                }
            };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId))
                .ReturnsAsync(new Order { Id = orderId });

            _orderRepositoryMock.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            
            var result = await _handler.Handle(command, CancellationToken.None);

            
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ThrowsException()
        {
            
            var orderId = Guid.NewGuid();
            var command = new UpdateOrderCommand
            {
                Id = orderId,
                CargoCompany = "Yurtiçi",
                Items = new List<OrderItemDto>()
            };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId))
                .ReturnsAsync((Order)null); 

            
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    }

    
}
