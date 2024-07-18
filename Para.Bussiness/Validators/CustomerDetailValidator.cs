using FluentValidation;
using Para.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Para.Bussiness.Validators
{
    public class CustomerDetailValidator : AbstractValidator<CustomerDetail>
    {
        public CustomerDetailValidator()
        {
            RuleFor(x => x.FatherName)
                .NotEmpty().WithMessage("Father name is required.")
                .Length(2, 100).WithMessage("Father name must be between 2 and 100 characters.");

            RuleFor(x => x.MotherName)
                .NotEmpty().WithMessage("Mother name is required.")
                .Length(2, 100).WithMessage("Mother name must be between 2 and 100 characters.");

            RuleFor(x => x.EducationStatus)
                .NotEmpty().WithMessage("Education status is required.")
                .Length(2, 50).WithMessage("Education status must be between 2 and 50 characters.");

            RuleFor(x => x.MontlyIncome)
                .NotEmpty().WithMessage("Monthly income is required.")
                .Matches(@"^\d+(\.\d{1,2})?$").WithMessage("Monthly income must be a valid number with up to 2 decimal places.");

            RuleFor(x => x.Occupation)
                .NotEmpty().WithMessage("Occupation is required.")
                .Length(2, 100).WithMessage("Occupation must be between 2 and 100 characters.");
        }
    }
}
