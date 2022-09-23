using FluentValidation;
using Warehouse.Data.Dto;
using Warehouse.Data.Models;

namespace Warehouse.Core.Validator.TransactionValidate
{
    public class CreateTransactionValidator : AbstractValidator<TransactionCreateDto>
    {
        public CreateTransactionValidator()
        {
                RuleFor(t => t.receiverId)
                    .NotEmpty()
                    .WithMessage("Xais edirik gonderilen anbari bos kecmeyin.")
                    .NotNull()
                        .WithMessage("Xais edirik gonderilen anbari bos kecmeyin.");

                RuleFor(t => t.productId)
                    .NotEmpty()
                    .WithMessage("Xais edirik gonderilen anbari bos kecmeyin.")
                    .NotNull()
                        .WithMessage("Xais edirik gonderilen mehsulu bildiresiniz.")
                        .GreaterThan(0)
                        .WithMessage("Olmayan bir mehsul girmisiniz");

                RuleFor(t => t.Count)
                    .NotEmpty()
                    .WithMessage("Xais edirik gonderilen anbari bos kecmeyin.")
                    .NotNull()
                    .WithMessage("Mehsulun qiymetini bos kecmeyin")
                    .GreaterThan(0)
                        .WithMessage("Mehsulun qimmetini duzgun giriniz")
                    .Must(s => s >= 0)
                        .WithMessage("Mehsulun qiymeti menfi ola bilmez");
        }

    }

}
