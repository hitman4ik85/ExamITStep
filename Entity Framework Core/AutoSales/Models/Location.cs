using Microsoft.EntityFrameworkCore;

namespace AutoSales.Models;

[Owned]
public class Location
{
    public string? City { get; set; }
    public string? Address { get; set; }
}