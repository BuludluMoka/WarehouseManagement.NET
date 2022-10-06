using FluentValidation;
using Warehouse.Data.Dto.Products;
using Warehouse.Data.Models;

namespace Warehouse.Core.Validator.Products
{
    public class UpdateProductValidator: AbstractValidator<ProductUpdateDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(product => product.Name)
                 .MaximumLength(50)
                 .MinimumLength(3)
                     .WithMessage("Mehsul adını 5 ile 50 herf arasında giriniz.");


            RuleFor(product => product.sellPrice)
                .Must(s => s >= 0)
                    .WithMessage("Fiyat bilgisi negatif olamaz!");
                
        }
    }
}
