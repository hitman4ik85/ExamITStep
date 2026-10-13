-- 1. Тригер UpdateTotalAmount
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
VALUES (1, 3, 2, 500.00) -- Додаємо два товари із ціною 500 у замовлення з ID = 1

-- 2. Тригер UpdateStockQuantity
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
VALUES (2, 4, 5, 300.00) -- Додаємо 5 одиниць товару з ID = 4 у замовлення з ID = 2

-- 3. Тригер LowStockAlert
UPDATE Inventory
SET QuantityAvailable = 1
WHERE ProductId = 4

-- 4. Тригер CheckInventoryThreshold
UPDATE Inventory
SET QuantityAvailable = 0
WHERE ProductId = 5

-- 5. Тригер CheckPaymentStatus
UPDATE Shipping
SET ShippingStatus = 'Shipped', ShippingDate = GETDATE()
WHERE OrderId = 3

-- 6. Тригер UpdateStockOnOrder
DECLARE @InitialQuantity INT
SELECT @InitialQuantity = StockQuantity
FROM Products
WHERE ProductId = 6

INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
VALUES (3, 6, 3, 450.00)

SELECT StockQuantity AS NewQuantity, @InitialQuantity AS InitialQuantity
FROM Products
WHERE ProductId = 6

-- 7. Тригер UpdateOrderStatusOnDelivery
UPDATE Shipping
SET ShippingStatus = 'Delivered', DeliveryDate = GETDATE()
WHERE OrderId = 4

-- 8. Тригер ApplyDiscount
INSERT INTO Discounts (ProductId, DiscountPercentage, StartDate, EndDate)
VALUES (7, 10, GETDATE(), DATEADD(DAY, 10, GETDATE())) -- Знижка 10% на товар з ID = 7

-- 9. Тригер SaveReviewHistory
UPDATE Reviews
SET ReviewText = 'Very good product!'
WHERE ReviewId = 1 -- Оновлення відгуку з ID = 1




--SELECT * FROM OrderItems
--SELECT * FROM Inventory
--SELECT * FROM Notifications
--SELECT * FROM Shipping
--SELECT * FROM Discounts
--SELECT * FROM Reviews



