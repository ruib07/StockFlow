namespace StockFlow.Domain.DTOs;

public static class RecoverPasswordDTO
{
    public record Request(string Email);
    public record UpdatePassword(string Token, string NewPassword, string ConfirmNewPassword);
}
