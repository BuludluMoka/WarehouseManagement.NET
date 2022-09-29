using FluentValidation;
using Warehouse.Data.Dto;
using Warehouse.Data.Models;

namespace Warehouse.Core.Validator.TransactionValidate
{
    public class CreateTransactionValidator : AbstractValidator<TransactionCreateDto>
    {
        public CreateTransactionValidator()
        {
            RuleFor(x => x.TransactionNo)
                .NotEmpty()
                .WithMessage("Xais edirik Transaction Nomresini bos kecmeyin")
                .NotNull()
                .WithMessage("Xais edirik TransactionNOmresini bos kecmeyin");
            RuleFor(t => t.Receiver)
                .NotEmpty().When(t => t.Sender == true)
                .WithMessage("Xais edirik gonderilen anbari bos kecmeyin.")
                .NotNull()
                    .WithMessage("Xais edirik gonderilen anbari bos kecmeyin.").When(t => t.Sender == true);

            RuleFor(t => t.productId)
                .NotEmpty()
                .WithMessage("Xais edirik gonderilen anbari bos kecmeyin.")
                .NotNull()
                    .WithMessage("Xais edirik gonderilen mehsulu bildiresiniz.")
                    .GreaterThan(0)
                    .WithMessage("Olmayan bir mehsul girmisiniz");

            RuleFor(t => t.Count)
                .NotEmpty()
                .WithMessage("Xais edirik miqdari bos kecmeyin.")
                .NotNull()
                .WithMessage("Mehsulun miqdarini bos kecmeyin")
                .GreaterThan(0)
                    .WithMessage("Mehsulun miqdarini duzgun girin")
                .Must(s => s >= 0)
                    .WithMessage("Mehsulun miqdari menfi ola bilmez");

        }

    }

}
