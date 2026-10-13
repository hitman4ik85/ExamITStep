using AutoSales.Models;
using Microsoft.EntityFrameworkCore;

using (ApplicationContext db = new ApplicationContext())
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    // Створення об'єктів
    Car car1 = new Car
    {
        Brand = "Toyota",
        Model = "Corolla",
        Price = 20000,
        Details = new CarDetail { EngineType = "Hybrid", Year = 2022 }
    };

    Car car2 = new Car
    {
        Brand = "Honda",
        Model = "C-RV",
        Price = 22000,
        Details = new CarDetail { EngineType = "Diesel", Year = 2021 }
    };

    Seller seller = new Seller
    {
        Name = "AutoTrader",
        Location = new Location { City = "Sumy", Address = "Street 128/2" },
        Cars = new List<Car> { car1, car2 }
    };

    db.Sellers.Add(seller);
    db.SaveChanges();

    // Завантаження та виведення даних
    var sellers = db.Sellers.Include(s => s.Cars).ToList();
    foreach (var sel in sellers)
    {
        Console.WriteLine($"Seller: {sel.Name}, City: {sel.Location.City}, Address: {sel.Location.Address}");
        foreach (var car in sel.Cars)
        {
            // Окреме використання Details
            Console.WriteLine($"\tCar: {car.Brand} {car.Model}, Year: {car.Details.Year}, Engine: {car.Details.EngineType}, Price: {car.Price}");
        }
    }
}