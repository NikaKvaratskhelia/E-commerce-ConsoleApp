using E_commerce.Models;
using FluentValidation;

namespace E_commerce.Validators
{
    internal class UserValidators : AbstractValidator<User>
    {
        public UserValidators()
        {
            RuleFor(user => user.Username)
                .NotNull().WithMessage("Name cannot be empty.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
                .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Name can only contain letters, numbers, and underscores.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
        }
    }
}
