using System.ComponentModel.DataAnnotations.Schema;

namespace AutoSales.Models;

[ComplexType]
public class CarDetail
{
    public string? EngineType { get; set; }
    public int Year { get; set; }
}