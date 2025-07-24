using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.Linq;

namespace OrderAndCargo.Application.Commands
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Güncellenecek sipariş Id'si boş olamaz.");

            RuleFor(x => x.CargoCompany)
                .NotEmpty()
                .WithMessage("Kargo şirketi boş olamaz.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Sipariş en az 1 ürün içermelidir.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty()
                    .WithMessage("Ürün ID boş olamaz");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Miktar 0'dan büyük olmalıdır");

                item.RuleFor(i => i.Name)
                    .NotEmpty()
                    .WithMessage("Ürün adı boş olamaz");
            });
        }
    }
}

