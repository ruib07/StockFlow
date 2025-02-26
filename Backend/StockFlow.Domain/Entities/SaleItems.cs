namespace StockFlow.Domain.Entities;

public class SaleItems
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public Sales Sale { get; set; }
    public Guid ProductId { get; set; }
    public Products Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
}
