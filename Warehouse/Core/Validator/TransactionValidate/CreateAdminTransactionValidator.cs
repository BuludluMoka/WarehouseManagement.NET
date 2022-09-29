using FluentValidation;
using Warehouse.Data.Dto;
using Warehouse.Data.Models;

namespace Warehouse.Core.Validator.TransactionValidate
{
    public class CreateAdminTransactionValidator : AbstractValidator<Data.Dto.Transactions.AdminTransactionCreateDto>
    {
        public CreateAdminTransactionValidator()
        {
            RuleFor(x => x.TransactionNo)
                .NotEmpty()
                .WithMessage("Xais edirik Transaction Nomresini bos kecmeyin");

            RuleFor(t => t.receiver_id)
                .NotEmpty()
                .WithMessage("Xais edirik gonderilen anbari bos kecmeyin.")
                .NotNull()
                    .WithMessage("Xais edirik gonderilen anbari bos kecmeyin.");

            RuleFor(t => t.ProductId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Xais edirik gonderilen mehsulu bildiresiniz.");
                
                    

            RuleFor(t => t.Count)
                .NotNull()
                .NotEmpty()
                .WithMessage("Mehsulun miqdarini bos kecmeyin")
                
                
               
                .Must(s => s >= 0)
                    .WithMessage("Mehsulun miqdari menfi ola bilmez");
        }

    }

}
