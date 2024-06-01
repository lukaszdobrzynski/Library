using FluentValidation;

namespace Library.Modules.Reservation.Application.Patrons.CancelHold;

public class CancelHoldCommandValidator : AbstractValidator<CancelHoldCommand>
{
    public CancelHoldCommandValidator()
    {
        RuleFor(x => x.PatronId).NotEmpty().WithMessage($"{nameof(CancelHoldCommand.PatronId)} cannot be empty.");
        RuleFor(x => x.HoldId).NotEmpty().WithMessage($"{nameof(CancelHoldCommand.HoldId)} cannot be empty.");
    }
}