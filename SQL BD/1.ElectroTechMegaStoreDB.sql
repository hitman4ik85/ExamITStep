CREATE DATABASE ElectroTechMegaStoreDB
GO

USE ElectroTechMegaStoreDB
GO

CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY,
    CategoryName NVARCHAR(100) NOT NULL
)
GO

CREATE TABLE Suppliers (
    SupplierId INT PRIMARY KEY IDENTITY,
    SupplierName NVARCHAR(100) NOT NULL,
    ContactInfo NVARCHAR(255)
)
GO

CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId) ON DELETE SET NULL,
    SupplierId INT FOREIGN KEY REFERENCES Suppliers(SupplierId) ON DELETE SET NULL,
    Price MONEY NOT NULL,
    IsNew BIT NOT NULL,
    IsTaxed BIT NOT NULL,
    StockQuantity INT NOT NULL
)
GO

CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(30),
    Email NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255)
)
GO

CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY,
    CustomerId INT FOREIGN KEY REFERENCES Customers(CustomerId) ON DELETE CASCADE,
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount MONEY NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Очікує доставки'
)
GO

CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY,
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId) ON DELETE CASCADE,
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId) ON DELETE CASCADE,
    Quantity INT NOT NULL,
    UnitPrice MONEY NOT NULL
)
GO

CREATE TABLE Reviews (
    ReviewId INT PRIMARY KEY IDENTITY,
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId) ON DELETE CASCADE,
    CustomerId INT FOREIGN KEY REFERENCES Customers(CustomerId) ON DELETE CASCADE,
    ReviewText NVARCHAR(500),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    ReviewDate DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE ReviewHistory (
    HistoryId INT PRIMARY KEY IDENTITY,
    ReviewId INT,
    ProductId INT,
    CustomerId INT,
    ReviewText NVARCHAR(500),
    Rating INT,
    ReviewDate DATETIME,
    ModifiedDate DATETIME
)
GO

CREATE TABLE Discounts (
    DiscountId INT PRIMARY KEY IDENTITY,
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId) ON DELETE CASCADE,
    DiscountPercentage DECIMAL(5, 2) CHECK (DiscountPercentage BETWEEN 0 AND 100),
    StartDate DATETIME,
    EndDate DATETIME
)
GO

CREATE TABLE Inventory (
    InventoryId INT PRIMARY KEY IDENTITY,
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId) ON DELETE CASCADE,
    QuantityAvailable INT NOT NULL,
    ReorderThreshold INT NOT NULL
)
GO

CREATE TABLE ProductAttributes (
    AttributeId INT PRIMARY KEY IDENTITY,
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId) ON DELETE CASCADE,
    AttributeName NVARCHAR(100),
    AttributeValue NVARCHAR(100)
)
GO

CREATE TABLE PaymentMethods (
    PaymentMethodId INT PRIMARY KEY IDENTITY,
    MethodName NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE OrderPayments (
    PaymentId INT PRIMARY KEY IDENTITY,
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId) ON DELETE CASCADE,
    PaymentMethodId INT FOREIGN KEY REFERENCES PaymentMethods(PaymentMethodId) ON DELETE SET NULL,
    PaymentDate DATETIME DEFAULT GETDATE(),
    AmountPaid MONEY NOT NULL
)
GO

CREATE TABLE Shipping (
    ShippingId INT PRIMARY KEY IDENTITY,
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId) ON DELETE CASCADE,
    ShippingDate DATETIME,
    DeliveryDate DATETIME,
    ShippingStatus NVARCHAR(50),
    ShippingAddress NVARCHAR(255)
)
GO

CREATE TABLE WishList (
    WishListId INT PRIMARY KEY IDENTITY,
    CustomerId INT FOREIGN KEY REFERENCES Customers(CustomerId) ON DELETE CASCADE,
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId) ON DELETE CASCADE,
    DateAdded DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE CartItems (
    CartItemId INT PRIMARY KEY IDENTITY,
    CustomerId INT FOREIGN KEY REFERENCES Customers(CustomerId) ON DELETE CASCADE,
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId) ON DELETE CASCADE,
    Quantity INT NOT NULL,
    DateAdded DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE Notifications (
    NotificationId INT PRIMARY KEY IDENTITY,
    Message NVARCHAR(255) NOT NULL,
    NotificationDate DATETIME DEFAULT GETDATE()
)
GO