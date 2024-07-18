using FluentValidation;
using Para.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Para.Bussiness.Validators
{
    public class CustomerAddressValidator : AbstractValidator<CustomerAddress>
    {
        public CustomerAddressValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .Length(2, 100).WithMessage("Country must be between 2 and 100 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .Length(2, 100).WithMessage("City must be between 2 and 100 characters.");

            RuleFor(x => x.AddressLine)
                .NotEmpty().WithMessage("AddressLine is required.")
                .Length(5, 200).WithMessage("AddressLine must be between 5 and 200 characters.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("ZipCode is required.")
                .Matches(@"^\d{5}(-\d{4})?$").WithMessage("ZipCode must be a valid postal code.");

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("IsDefault must be specified.");
        }
    }
}
