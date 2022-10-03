using FluentValidation;
using Warehouse.Data.Dto.Products;
using Warehouse.Data.Models;

namespace Warehouse.Core.Validator.Products
{
    public class CreateProductValidator : AbstractValidator<ProductCreateDto>
    {
        public CreateProductValidator()
        {
            RuleFor(product => product.Name)  
                .NotEmpty().WithMessage("Mehsulun adini bos kecmeyin.")
                .NotNull().WithMessage("Mehsulun adini bos kecmeyin.")
                .MaximumLength(50)
                .MinimumLength(3).WithMessage("Mehsul adını 5 ile 50 herf arasında giriniz.");

            RuleFor(product => product.buyPrice)
                .NotEmpty().WithMessage("Mehsulun alis qiymetini bos kecmeyin.")
                .NotNull().WithMessage("Mehsulun alis qiymetini bos kecmeyin.")
                .Must(s => s >= 0).WithMessage("Mehsulun alis qiymeti menfi ola bilmez");

            RuleFor(product => product.sellPrice)
                .NotEmpty().WithMessage("Mehsulun satis qiymetini bos kecmeyin.")
                .NotNull().WithMessage("Mehsulun satis qiymetini bos kecmeyin.")
                .Must(s => s >= 0).WithMessage("Mehsulun satis qiymeti menfi ola bilmez");

            RuleFor(product => product.CategoryId)
                .NotEmpty().WithMessage("Kateqoryani bos kecmeyin.")
                .NotNull().WithMessage("Kateqoryani bos kecmeyin.");
        }
        //private bool IsPasswordValid(string arg)
        //{
        //    Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
        //    return regex.IsMatch(arg);
        //}
    }
}