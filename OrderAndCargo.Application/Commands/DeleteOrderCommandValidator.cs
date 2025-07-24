using FluentValidation;
using OrderAndCargo.Application.Commands;
using OrderAndCargo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderCommandValidator(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Silinecek sipariş Id'si boş olamaz.")
            .MustAsync(async (id, cancellation) => await OrderExists(id))
            .WithMessage("Girilen sipariş bulunamadı.");
    }

    private async Task<bool> OrderExists(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order != null;
    }
}


