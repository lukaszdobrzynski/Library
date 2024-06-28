using FluentValidation;

namespace Library.Modules.Reservation.Application.Holds.GrantHold;

public class ApplyGrantHoldDecisionCommandValidator : AbstractValidator<ApplyGrantHoldDecisionCommand>
{
    public ApplyGrantHoldDecisionCommandValidator()
    {
        RuleFor(x => x.HoldId).NotEmpty().WithMessage($"{nameof(ApplyGrantHoldDecisionCommand.HoldId)} cannot be empty.");
    }
}