-- для перегляду всіх товарів з інформацією про категорії, постачальників та запаси
CREATE VIEW ProductDetailsWithCategoryAndSupplierInformation AS
SELECT 
    Products.ProductId,
    Products.Name AS ProductName,
    Products.Description AS ProductDescription,
    Categories.CategoryName AS CategoryName,
    Suppliers.SupplierName AS SupplierName,
    Products.Price AS ProductPrice,
    Products.IsNew AS IsProductNew,
    Products.IsTaxed AS IsProductTaxed,
    Inventory.QuantityAvailable AS AvailableQuantityInInventory
FROM Products
JOIN Categories ON Products.CategoryId = Categories.CategoryId
JOIN Suppliers ON Products.SupplierId = Suppliers.SupplierId
JOIN Inventory ON Products.ProductId = Inventory.ProductId
GO

-- для відображення замовлень з деталями по товарам та загальною сумою
CREATE VIEW OrderDetailsWithProductInformationAndTotalAmount AS
SELECT 
    Orders.OrderId AS OrderIdentifier,
    Customers.FullName AS CustomerFullName,
    Orders.OrderDate AS OrderDate,
    OrderItems.ProductId AS ProductIdentifier,
    Products.Name AS ProductName,
    OrderItems.Quantity AS ProductQuantity,
    OrderItems.UnitPrice AS ProductUnitPrice,
    (OrderItems.Quantity * OrderItems.UnitPrice) AS TotalPricePerProduct,
    Orders.TotalAmount AS TotalOrderAmount
FROM Orders
JOIN OrderItems ON Orders.OrderId = OrderItems.OrderId
JOIN Products ON OrderItems.ProductId = Products.ProductId
JOIN Customers ON Orders.CustomerId = Customers.CustomerId
GO

-- для аналітики продажів за категоріями товарів і знижками
CREATE VIEW SalesAnalyticsByProductCategoryAndDiscount AS
SELECT 
    Categories.CategoryName AS ProductCategory,
    Products.Name AS ProductName,
    Discounts.DiscountPercentage AS DiscountPercentageOnProduct,
    SUM(OrderItems.Quantity) AS TotalQuantitySold,
    SUM(OrderItems.Quantity * OrderItems.UnitPrice) AS TotalSalesAmount
FROM OrderItems
JOIN Products ON OrderItems.ProductId = Products.ProductId
JOIN Categories ON Products.CategoryId = Categories.CategoryId
LEFT JOIN Discounts ON Products.ProductId = Discounts.ProductId
GROUP BY Categories.CategoryName, Products.Name, Discounts.DiscountPercentage
GO

-- для середнього рейтингу товарів за відгуками
CREATE VIEW ProductAverageRatingsAndReviewCount AS
SELECT 
    Products.ProductId AS ProductIdentifier,
    Products.Name AS ProductName,
    AVG(Reviews.Rating) AS AverageProductRating,
    COUNT(Reviews.ReviewId) AS TotalNumberOfReviews
FROM Products
LEFT JOIN Reviews ON Products.ProductId = Reviews.ProductId
GROUP BY Products.ProductId, Products.Name
GO

SELECT * FROM ProductDetailsWithCategoryAndSupplierInformation
SELECT * FROM OrderDetailsWithProductInformationAndTotalAmount
SELECT * FROM SalesAnalyticsByProductCategoryAndDiscount
SELECT * FROM ProductAverageRatingsAndReviewCount