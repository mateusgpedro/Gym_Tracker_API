namespace gym_tracker.Infra.Authentication;

public record ResetPasswordRequest(string Email, string Code, string NewPassword);