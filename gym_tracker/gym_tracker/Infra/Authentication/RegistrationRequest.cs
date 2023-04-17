namespace gym_tracker.Infra.Authentication;

public record RegistrationRequest(string Username, string Fullname,string Email, string Password);