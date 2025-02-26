namespace StockFlow.Domain.Entities;

public class Products
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Guid SupplierId { get; set; }
    public Suppliers Supplier { get; set; }
    public Guid CategoryId { get; set; }
    public Categories Category { get; set; }
    public DateTime CreatedAt { get; set; }
}
