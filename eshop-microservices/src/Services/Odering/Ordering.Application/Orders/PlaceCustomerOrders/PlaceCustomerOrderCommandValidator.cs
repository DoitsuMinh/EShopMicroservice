using FluentValidation;

namespace Ordering.Application.Orders.PlaceCustomerOrders;

public class PlaceCustomerOrderCommandValidator : AbstractValidator<PlaceCustomerOrderCommand>
{
    public PlaceCustomerOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID must not be empty.");
        RuleFor(x => x.Products)
            .NotEmpty()
            .WithMessage("Products list must not be empty.")
            .Must(products => products.All(p => p.Quantity > 0))
            .WithMessage("All products must have a quantity greater than zero.");
        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(currency => currency == "USD" || currency == "AUD")
            .WithMessage("At least one product has invalid currency");
    }
}
