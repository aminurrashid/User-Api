using FluentValidation;

namespace User_Api.Validators.User
{
    public class UserValidator : AbstractValidator<Domain.Users.User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().NotEmpty().WithMessage("Email is not provided").EmailAddress().WithMessage("Invalid email address");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name is not provided");
            RuleFor(x => x.Phone).Matches(@"[+\(\)\d]").When(x => !string.IsNullOrEmpty(x.Phone)).WithMessage("Phone number is not valid");
            RuleFor(x => x.Age).InclusiveBetween(0, 150).WithMessage("Invalid age provided");
        }
    }
}
