using System;
using FluentValidation;

namespace RSVP.Application.Features.Attendie.Command.AddAttendieByGroup;

public class AddAttendieByGroupValidator:AbstractValidator<AddAttendieByGroupCommand>
{
    public AddAttendieByGroupValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("EventId must be a positive integer.");

        RuleFor(x => x.AttendieEmails)
            .NotEmpty().WithMessage("AttendieEmails list cannot be empty.")
            .Must(emails => emails.All(email => IsValidEmail(email))).WithMessage("All AttendieEmails must be valid email addresses.");
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}