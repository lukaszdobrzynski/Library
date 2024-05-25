using FluentValidation;

namespace Library.Modules.Reservation.Application.Patrons.PlaceBookOnHold;

public class PlaceBookOnHoldCommandValidator : AbstractValidator<PlaceBookOnHoldCommand>
{
    public PlaceBookOnHoldCommandValidator()
    {
        RuleFor(x => x.BookId).NotEmpty().WithMessage($"{nameof(PlaceBookOnHoldCommand.BookId)} cannot be empty.");
        RuleFor(x => x.PatronId).NotEmpty().WithMessage($"{nameof(PlaceBookOnHoldCommand.PatronId)} cannot be empty.");
    }
}