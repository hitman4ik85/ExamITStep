namespace AutoSales.Models;

public class Seller
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Location Location { get; set; }
    public List<Car> Cars { get; set; } = new();
}