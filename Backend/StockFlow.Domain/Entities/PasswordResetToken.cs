namespace StockFlow.Domain.Entities;

public class PasswordResetToken
{
    public Guid Id { get; set; }
    public Guid AdminId { get; set; }
    public Administrators Administrator { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
}
