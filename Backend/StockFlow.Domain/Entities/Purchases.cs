namespace StockFlow.Domain.Entities;

public class Purchases
{
    public Guid Id { get; set; }
    public Guid SupplierId { get; set; }
    public Suppliers Supplier { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal Total { get; set; }
}
