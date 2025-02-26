namespace StockFlow.Domain.Entities;

public class PurchaseItems
{
    public Guid Id { get; set; }
    public Guid PurchaseId { get; set; }
    public Purchases Purchase { get; set; }
    public Guid ProductId { get; set; }
    public Products Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
}
