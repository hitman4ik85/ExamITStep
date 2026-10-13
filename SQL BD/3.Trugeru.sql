-- для оновлення загальної суми замовлення
CREATE TRIGGER UpdateTotalAmount
ON OrderItems
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @OrderId INT
    SELECT TOP 1 @OrderId = OrderId FROM inserted
    UPDATE Orders
    SET TotalAmount = (SELECT SUM(UnitPrice * Quantity) FROM OrderItems WHERE OrderId = @OrderId)
    WHERE OrderId = @OrderId
END
GO

-- для оновлення кількості на складі після додавання елементів у замовлення
CREATE TRIGGER UpdateStockQuantity
ON OrderItems
AFTER INSERT
AS
BEGIN
    UPDATE Inventory
    SET QuantityAvailable = QuantityAvailable - inserted.Quantity
    FROM Inventory
    JOIN inserted ON Inventory.ProductId = inserted.ProductId
END
GO

-- для попередження про низький запас товару
CREATE TRIGGER LowStockAlert
ON Inventory
AFTER UPDATE
AS
BEGIN
    DECLARE @ProductId INT, @QuantityAvailable INT, @ReorderThreshold INT
    SELECT @ProductId = ProductId, @QuantityAvailable = QuantityAvailable, @ReorderThreshold = ReorderThreshold FROM inserted

    IF @QuantityAvailable < @ReorderThreshold
    BEGIN
        PRINT 'Warning: Low stock for product with ID = ' + CAST(@ProductId AS NVARCHAR(10))
    END
END;
GO

-- для контролю за запасами (Inventory)
CREATE TRIGGER CheckInventoryThreshold
ON Inventory
AFTER UPDATE
AS
BEGIN
    INSERT INTO Notifications (Message, NotificationDate)
    SELECT CONCAT('Product with ID ', inserted.ProductId, ' needs restocking.')
    , GETDATE()
    FROM inserted
    WHERE inserted.QuantityAvailable <= inserted.ReorderThreshold
END
GO

-- для перевірки завершення оплати перед доставкою
CREATE TRIGGER CheckPaymentStatus
ON Shipping
INSTEAD OF UPDATE
AS
BEGIN
    DECLARE @OrderId INT, @TotalAmount MONEY, @AmountPaid MONEY
    SELECT @OrderId = OrderId FROM inserted
    SELECT @TotalAmount = TotalAmount FROM Orders WHERE OrderId = @OrderId
    SELECT @AmountPaid = SUM(AmountPaid) FROM OrderPayments WHERE OrderId = @OrderId

    IF @AmountPaid < @TotalAmount
    BEGIN
        RAISERROR ('Payment for the order is incomplete. The order cannot be shipped.', 16, 1)
    END
    ELSE
    BEGIN
        UPDATE Shipping
        SET ShippingStatus = inserted.ShippingStatus,
            ShippingDate = inserted.ShippingDate,
            DeliveryDate = inserted.DeliveryDate
        FROM inserted
        WHERE Shipping.OrderId = @OrderId
    END
END
GO

--для автоматичного оновлення запасів (StockQuantity) після замовлення
CREATE TRIGGER UpdateStockOnOrder
ON OrderItems
AFTER INSERT
AS
BEGIN
    UPDATE Products
    SET StockQuantity = StockQuantity - inserted.Quantity
    FROM Products
    INNER JOIN inserted ON Products.ProductId = inserted.ProductId
END
GO

--для оновлення статусу замовлення після доставки (Shipping)
CREATE TRIGGER UpdateOrderStatusOnDelivery
ON Shipping
AFTER UPDATE
AS
BEGIN
    UPDATE Orders
    SET Status = 'Completed'
    FROM Orders
    INNER JOIN inserted ON Orders.OrderId = inserted.OrderId
    WHERE inserted.ShippingStatus = 'Delivered'
END
GO

--для автоматичного застосування знижок (Discounts)
CREATE TRIGGER ApplyDiscount
ON Discounts
AFTER INSERT
AS
BEGIN
    UPDATE Products
    SET Price = Price * (1 - inserted.DiscountPercentage / 100)
    FROM Products
    INNER JOIN inserted ON Products.ProductId = inserted.ProductId
    WHERE GETDATE() BETWEEN inserted.StartDate AND inserted.EndDate
END
GO

--для ведення історії змін у відгуках (Reviews History)
CREATE TRIGGER SaveReviewHistory
ON Reviews
AFTER UPDATE
AS
BEGIN
    INSERT INTO ReviewHistory (ReviewId, ProductId, CustomerId, ReviewText, Rating, ReviewDate, ModifiedDate)
    SELECT ReviewId, ProductId, CustomerId, ReviewText, Rating, ReviewDate, GETDATE()
    FROM deleted
END
GO