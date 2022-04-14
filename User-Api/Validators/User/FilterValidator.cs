using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Api.Models.Filters;

namespace User_Api.Validators.User
{
    public class FilterValidator : AbstractValidator<UserFilter>
    {
        public FilterValidator()
        {
            RuleFor(x => x.Email).NotEmpty().When(x => string.IsNullOrEmpty(x.Phone)).WithMessage("Email/phone number is not provided");
            RuleFor(x => x.Phone).NotEmpty().When(x => string.IsNullOrEmpty(x.Email)).WithMessage("Email/phone number is not provided");
            RuleFor(x => x.SortingField).Must(s => typeof(Domain.Users.User).GetProperty(s) != null).When(x => !string.IsNullOrEmpty(x.SortingField)).WithMessage("Invalid sorting field provided");
            RuleFor(x => x.SortOrder).Must(s => Enum.IsDefined(typeof(SortOrder), s)).WithMessage("Sort order is not valid");
        }
    }
}
