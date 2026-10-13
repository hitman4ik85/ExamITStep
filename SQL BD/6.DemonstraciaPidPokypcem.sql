-- дати роль покупцев≥
CREATE ROLE JaneSmithRole
GRANT SELECT ON Categories TO JaneSmithRole
GRANT SELECT ON Products TO JaneSmithRole
GRANT SELECT ON Reviews TO JaneSmithRole
GRANT SELECT ON Discounts TO JaneSmithRole
GRANT SELECT ON WishList TO JaneSmithRole
GRANT SELECT ON Orders TO JaneSmithRole
GRANT SELECT ON OrderItems TO JaneSmithRole
GRANT SELECT ON CartItems TO JaneSmithRole
GO

-- створенн€ покупц€
CREATE LOGIN JaneSmithLogin WITH PASSWORD = 'JaneSmithPassword123'
GO
USE ElectroTechMegaStoreDB
GO
CREATE USER JaneSmithUser FOR LOGIN JaneSmithLogin
GO
EXEC sp_addrolemember 'JaneSmithRole', 'JaneSmithUser'
GO

-- перегл€д доступних категор≥й
SELECT * FROM Categories

-- перегл€д товар≥в у певн≥й категор≥њ
SELECT * FROM Products
WHERE CategoryId = 1

-- перегл€д доступних знижок
SELECT * FROM Discounts
WHERE GETDATE() BETWEEN StartDate AND EndDate

-- перегл€д замовлень користувача
SELECT * FROM Orders
WHERE CustomerId = 2
GO



-- додаванн€ товару до корзини користувача:
INSERT INTO CartItems (CustomerId, ProductId, Quantity)
VALUES (2, 5, 3) -- 2 Ц ID покупц€, 5 Ц ID товару, 3 Ц к≥льк≥сть
INSERT INTO CartItems (CustomerId, ProductId, Quantity)
VALUES (2, 1, 5)

-- перегл€д товар≥в у корзин≥ дл€ певного покупц€:
SELECT Products.Name AS ProductName, Products.Price, CartItems.Quantity, 
       (Products.Price * CartItems.Quantity) AS TotalPrice
FROM CartItems
INNER JOIN Products ON CartItems.ProductId = Products.ProductId
WHERE CartItems.CustomerId = 2 -- ID покупц€

-- оновленн€ к≥лькост≥ товару в корзин≥:
UPDATE CartItems
SET Quantity = 5
WHERE CustomerId = 2 AND ProductId = 5

-- видаленн€ товару з корзини:
DELETE FROM CartItems
WHERE CustomerId = 2 AND ProductId = 5



-- перенесенн€ товар≥в з корзини користувача до замовлень:
DECLARE @NewOrderId INT

-- 1. створюЇмо новий запис у таблиц≥ Orders
INSERT INTO Orders (CustomerId, OrderDate, TotalAmount)
VALUES (2, GETDATE(), 0)

-- отримуЇмо ≥дентиф≥катор нового замовленн€
SET @NewOrderId = SCOPE_IDENTITY()

-- 2. переносимо товари з CartItems до OrderItems
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
SELECT @NewOrderId, CartItems.ProductId, CartItems.Quantity, Products.Price
FROM CartItems
INNER JOIN Products ON CartItems.ProductId = Products.ProductId
WHERE CartItems.CustomerId = 2

-- 3. оновленн€ загальноњ суми замовленн€
UPDATE Orders
SET TotalAmount = (SELECT SUM(OrderItems.Quantity * OrderItems.UnitPrice)
                   FROM OrderItems
                   WHERE OrderItems.OrderId = @NewOrderId)
WHERE OrderId = @NewOrderId

-- 4. очищуЇмо корзину покупц€
DELETE FROM CartItems WHERE CustomerId = 2