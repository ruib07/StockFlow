namespace StockFlow.Domain.DTOs;

public static class LoginDTO
{
    public record Request(string Email, string Password);

    public record Response(string AccessToken, string TokenType = "Bearer");
}
