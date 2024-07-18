using FluentValidation;
using Para.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Para.Bussiness.Validators
{
    public class CustomerPhoneValidator : AbstractValidator<CustomerPhone>
    {
        public CustomerPhoneValidator()
        {
            RuleFor(x => x.CountyCode)
                .NotEmpty().WithMessage("Country code is required.")
                .Length(3).WithMessage("Country code must be 3 characters long.")
                .Matches(@"^\+90$").WithMessage("Country code must be '+90'."); //burada 3 karakterli(+90 seklinde) bir kod bekliyoruz. 

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^5\d{9}$").WithMessage("Phone number must start with 5 and be followed by 9 digits.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");
        }
    }
}
