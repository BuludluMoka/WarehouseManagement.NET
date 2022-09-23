using FluentValidation;
using Warehouse.Data.Dto.Ambar;
using Warehouse.Data.Models;

namespace Warehouse.Core.Validator.Warehouse
{
    public class CreateWarehouseValidation :AbstractValidator<AnbarCreateDto>
    {
        public CreateWarehouseValidation()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Xais edirik anbarin adini bos kecmeyin.");

        }
    }
}
