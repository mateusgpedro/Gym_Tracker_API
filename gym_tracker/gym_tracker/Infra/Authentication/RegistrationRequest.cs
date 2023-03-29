namespace gym_tracker.Infra.Authentication;

public record RegistrationRequest(string Username, string Email, string Password);