using FluentValidation;

namespace Library.Modules.Reservation.Application.Holds.RejectHold;

public class ApplyRejectHoldDecisionValidator : AbstractValidator<ApplyRejectHoldDecisionCommand>
{
    public ApplyRejectHoldDecisionValidator()
    {
        RuleFor(x => x.RequestHoldId).NotEmpty().WithMessage($"{nameof(ApplyRejectHoldDecisionCommand.RequestHoldId)} cannot be empty.");
    }
}