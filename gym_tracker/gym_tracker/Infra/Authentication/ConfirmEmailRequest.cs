namespace gym_tracker.Infra.Authentication;

public record ConfirmEmailRequest(string Code, string Email);