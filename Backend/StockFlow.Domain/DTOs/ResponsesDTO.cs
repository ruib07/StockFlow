namespace StockFlow.Domain.DTOs;

public static class ResponsesDTO
{
    public record Creation(string Message, Guid Id);
    public record Error(string Message, int StatusCode);
}
