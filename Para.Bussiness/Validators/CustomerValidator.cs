using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Para.Data.Domain;


namespace Para.Bussiness.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.FirstName)
                .NotEmpty().WithMessage("First name is required.");  // Zorunlu alan.

            RuleFor(customer => customer.LastName)
                .NotEmpty().WithMessage("Last name is required."); 

            RuleFor(customer => customer.Email)
                .NotEmpty().WithMessage("Email address is required.")  
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(customer => customer.IdentityNumber)
                .NotEmpty().WithMessage("Identity number is required.")
                .Length(11).WithMessage("Identity number must be 11 characters.");

            RuleFor(customer => customer.CustomerNumber)
                .GreaterThan(0).WithMessage("Customer number must be greater than zero.");

            RuleFor(customer => customer.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .Must(BeAValidDate).WithMessage("Date of birth must be a valid date.");
        }

        private bool BeAValidDate(DateTime date) // Geçerli bir tarih mi?
        {
            return date <= DateTime.Today;  
        }
    }
}
