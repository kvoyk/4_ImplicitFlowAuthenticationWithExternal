using FluentValidation;

namespace MLaw.Idp.Mvc.Models.Validators
{
    public class LoginUserModelValidator : AbstractValidator<LogingViewModel>
    {
        public LoginUserModelValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty().Length(3, 100);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Name).NotNull().NotEmpty().Length(1, 100);
            RuleFor(x => x.RedirectUrl).NotNull().NotEmpty();
            RuleFor(x => x.ValidationCode).NotNull().NotEmpty();
            RuleFor(x => x.State).NotNull().NotEmpty();
        }
    }
}