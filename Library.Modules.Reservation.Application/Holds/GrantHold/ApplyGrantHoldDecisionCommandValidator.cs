using FluentValidation;

namespace Library.Modules.Reservation.Application.Holds.GrantHold;

public class ApplyGrantHoldDecisionCommandValidator : AbstractValidator<ApplyGrantHoldDecisionCommand>
{
    public ApplyGrantHoldDecisionCommandValidator()
    {
        RuleFor(x => x.RequestHoldId).NotEmpty().WithMessage($"{nameof(ApplyGrantHoldDecisionCommand.RequestHoldId)} cannot be empty.");
    }
}