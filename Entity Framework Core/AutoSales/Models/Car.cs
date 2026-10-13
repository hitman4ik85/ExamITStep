namespace AutoSales.Models;

public class Car
{
    public int Id { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public double Price { get; set; }
    public required CarDetail Details { get; set; }
}