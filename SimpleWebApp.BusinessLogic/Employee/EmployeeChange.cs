﻿using FluentValidation;

namespace SimpleWebApp.BusinessLogic.Employee
{
    public class EmployeeChange
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public class Validator : AbstractValidator<EmployeeChange>
        {
            public Validator(ValidationOptions validationOptions)
            {
                RuleFor(x => x.FirstName)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Empty")
                    .MaximumLength(50)
                    .WithMessage("ToLong")
                    .MinimumLength(2)
                    .WithMessage("ToShort");

                RuleFor(x => x.LastName)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Empty")
                    .MaximumLength(50)
                    .WithMessage("ToLong")
                    .MinimumLength(2)
                    .WithMessage("ToShort");

                RuleFor(x => x.Birthday)
                    .Must(birthday =>
                    {
                        var age = (DateTime.Now - birthday).Days / 365;
                        return age < validationOptions.MinValueOfAge ? false : true;
                    })
                    .WithMessage("InvalidBirthday");
            }
        }
    }

}