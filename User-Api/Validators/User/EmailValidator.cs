using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Api.Validators.User
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(x => x).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Email is not provided").EmailAddress().WithMessage("Invalid email address");
        }
    }
}
