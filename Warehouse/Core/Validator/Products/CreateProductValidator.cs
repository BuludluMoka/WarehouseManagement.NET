using FluentValidation;
using Warehouse.Data.Models;

namespace Warehouse.Core.Validator.Products
{
    public class CreateProductValidator : AbstractValidator<Product>
    {
        public CreateProductValidator()
        {
            //RuleFor(p => p.Id)
            //    .NotEmpty()
            //    .NotNull()
            //        .WithMessage("Lütfen ürün adını boş geçmeyiniz.")
            //    .MaximumLength(150)
            //    .MinimumLength(5)
            //        .WithMessage("Lütfen ürün adını 5 ile 150 karakter arasında giriniz.");

            //RuleFor(p => p.Stock)
            //    .NotEmpty()
            //    .NotNull()
            //        .WithMessage("Lütfen stok bilgisini boş geçmeyiniz.")
            //    .Must(s => s >= 0)
            //        .WithMessage("Stok bilgisi negatif olamaz!");

            //RuleFor(p => p.Price)
            //    .NotEmpty()
            //    .NotNull()
            //        .WithMessage("Lütfen fiyat bilgisini boş geçmeyiniz.")
            //    .Must(s => s >= 0)
            //        .WithMessage("Fiyat bilgisi negatif olamaz!");
        }
    }
}