namespace StockFlow.Domain.DTOs;

public static class SignupDTO
{
    public record Request(string Name, string Email, string Password);
}
