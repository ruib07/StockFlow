namespace StockFlow.Domain.Entities;

public class Sales
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Customers Customer { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal Total { get; set; }
}
