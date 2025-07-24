using FluentValidation;
using OrderAndCargo.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CargoCompany)
            .NotEmpty().WithMessage("Kargo şirketi boş olamaz");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Sipariş en az 1 ürün içermelidir");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).NotEmpty().WithMessage("Ürün ID boş olamaz");
            item.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır");
            item.RuleFor(x => x.Name).NotEmpty().WithMessage("Ürün adı boş olamaz");
        });
    }
}

