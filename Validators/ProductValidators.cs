using E_commerce.Models;
using FluentValidation;

namespace E_commerce.Validators
{
    internal class ProductValidators : AbstractValidator<Product>
    {
        public ProductValidators()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(100).WithMessage("Name Required");

            RuleFor(p => p.Description)
                .NotEmpty()
                .MaximumLength(500).WithMessage("Description Required");

            RuleFor(p => p.Category)
                .NotEmpty().WithMessage("Category Required");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be greater or equal to 0");
        }
    }
}
