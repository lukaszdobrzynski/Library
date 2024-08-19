using FluentValidation;

namespace Library.Modules.Reservation.Application.Holds.CancelHold;

public class CancelExpiredHoldCommandValidator : AbstractValidator<CancelExpiredHoldCommand>
{
    public CancelExpiredHoldCommandValidator()
    {
        RuleFor(x => x.BookId).NotEmpty().WithMessage($"{nameof(CancelExpiredHoldCommand.BookId)} cannot be empty.");
        RuleFor(x => x.HoldId).NotEmpty().WithMessage($"{nameof(CancelExpiredHoldCommand.HoldId)} cannot be empty.");
    }
}