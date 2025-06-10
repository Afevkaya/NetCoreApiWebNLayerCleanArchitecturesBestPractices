using FluentValidation;

namespace App.Services.Products.Update;

public class UpdateRequestValidator: AbstractValidator<ProductUpdateRequest>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("İsim alanı boş olamaz.")
            .Length(3, 100).WithMessage("İsim alanı 3 ile 100 karakter arasında olmalıdır.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Fiyat sıfırdan büyük olmalıdır.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Adedi sıfırdan büyük olmalıdır.");
    }
}