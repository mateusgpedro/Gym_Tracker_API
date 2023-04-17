namespace gym_tracker.Infra.Authentication;

public record ConfirmPasswordReset(string Email, string Code, string NewPassword);