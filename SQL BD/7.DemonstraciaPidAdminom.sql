CREATE ROLE AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Categories TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Suppliers TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Products TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Customers TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Orders TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON OrderItems TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Reviews TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Discounts TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Inventory TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON ProductAttributes TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON PaymentMethods TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON OrderPayments TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Shipping TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON WishList TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON Users TO AdminRole
GRANT SELECT, INSERT, UPDATE, DELETE ON CartItems TO AdminRole
GO

-- Створення адміністратора
CREATE LOGIN AdminLogin WITH PASSWORD = 'AdminPassword123'
USE ElectroTechMegaStoreDB
GO
CREATE USER AdminUser FOR LOGIN AdminLogin
GO
EXEC sp_addrolemember 'AdminRole', 'AdminUser'
GO

-- Додати новий товар
INSERT INTO Products (Name, Description, CategoryId, SupplierId, Price, IsNew, IsTaxed, StockQuantity)
VALUES ('New Product', 'Description of the product', 1, 1, 1000.50, 1, 1, 50)

-- Оновити інформацію про товар
UPDATE Products
SET Price = 800.20
WHERE ProductId = 1

-- Видалити товар
DELETE FROM Products
WHERE ProductId = 2
GO