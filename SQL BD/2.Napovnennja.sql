INSERT INTO Categories (CategoryName)
VALUES 
    ('Laptops'),
    ('Smartphones'),
    ('Home Appliances'),
    ('Gaming Consoles'),
    ('Audio Equipment'),
    ('Tablets'),
    ('Cameras'),
    ('Wearables'),
    ('Printers'),
    ('Smart Home Devices')
GO

INSERT INTO Suppliers (SupplierName, ContactInfo)
VALUES 
    ('Tech World Supply Co.', '123 Main St, Tech City, 111-222-3333'),
    ('Home Essentials Ltd.', '456 Elm St, Home Town, 222-333-4444'),
    ('Smart Devices Inc.', '789 Oak St, Smart City, 333-444-5555'),
    ('Gaming Universe', '101 Maple Ave, Game City, 444-555-6666'),
    ('Audio Masters', '202 Pine St, Audio Ville, 555-666-7777'),
    ('Tab World', '303 Tab St, Tech City, 777-888-9999'),
    ('Camera Center', '505 Lens Ave, Photo Town, 888-999-0000'),
    ('Wearable Solutions', '707 Fit St, Gadget City, 999-111-2222'),
    ('Print Pros', '909 Paper Ave, Print City, 555-777-8888'),
    ('Smart Living', '1120 Home St, Living City, 666-888-7777')
GO

INSERT INTO Products (Name, Description, CategoryId, SupplierId, Price, IsNew, IsTaxed, StockQuantity)
VALUES 
    ('Dell Inspiron 15', '15.6" Laptop with Intel i5', 1, 1, 750.00, 1, 1, 50),
    ('Apple iPhone 13', '256GB Smartphone', 2, 3, 999.99, 1, 1, 100),
    ('Samsung Refrigerator', 'Double Door Refrigerator', 3, 2, 1200.00, 1, 1, 30),
    ('PlayStation 5', 'Next Gen Gaming Console', 4, 4, 499.99, 1, 1, 20),
    ('Bose Headphones', 'Wireless Noise Cancelling', 5, 5, 299.99, 1, 1, 75),
    ('iPad Air', '64GB Wi-Fi Tablet', 6, 6, 599.00, 1, 1, 150),
    ('Canon EOS R', 'Mirrorless Camera', 7, 7, 1799.00, 1, 1, 40),
    ('Fitbit Versa', 'Fitness Smartwatch', 8, 8, 199.00, 1, 1, 200),
    ('HP LaserJet', 'Monochrome Laser Printer', 9, 9, 299.00, 1, 1, 60),
    ('Amazon Echo', 'Smart Speaker with Alexa', 10, 10, 129.00, 1, 1, 120)
GO

INSERT INTO Customers (FullName, PhoneNumber, Email, Address)
VALUES 
    ('John Doe', '+380 50 1234567', 'john.doe@example.com', '101 Blue St, City'),
    ('Jane Smith', '+380 97 7654321', 'jane.smith@example.com', '202 Green St, City'),
    ('Emily Davis', '+380 63 3332221', 'emily.davis@example.com', '303 Red St, City'),
    ('Michael Brown', '+380 44 9991111', 'michael.brown@example.com', '404 Yellow St, City'),
    ('Sophia Wilson', '+380 32 2224444', 'sophia.wilson@example.com', '505 Purple St, City'),
    ('Lucas Martinez', '+380 63 1112223', 'lucas.martinez@example.com', '606 Blue St, City'),
    ('Ava Thomas', '+380 99 4567891', 'ava.thomas@example.com', '707 Green St, City'),
    ('Mason Lee', '+380 44 3334455', 'mason.lee@example.com', '808 Red St, City'),
    ('Olivia Garcia', '+380 32 9998887', 'olivia.garcia@example.com', '909 Yellow St, City'),
    ('Liam Rodriguez', '+380 50 7776665', 'liam.rodriguez@example.com', '1001 Purple St, City')
GO

INSERT INTO Orders (CustomerId, OrderDate, TotalAmount)
VALUES 
    (1, CONVERT(DATETIME, '2024-10-20', 102), 1250.00),
    (2, CONVERT(DATETIME, '2024-10-21', 102), 750.00),
    (3, CONVERT(DATETIME, '2024-10-22', 102), 999.99),
    (4, CONVERT(DATETIME, '2024-10-23', 102), 1499.99),
    (5, CONVERT(DATETIME, '2024-10-24', 102), 299.99),
    (6, CONVERT(DATETIME, '2024-10-25', 102), 599.00),
    (7, CONVERT(DATETIME, '2024-10-25', 102), 199.00),
    (8, CONVERT(DATETIME, '2024-10-26', 102), 399.00),
    (9, CONVERT(DATETIME, '2024-10-26', 102), 299.00),
    (10, CONVERT(DATETIME, '2024-10-27', 102), 129.00)
GO

INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
VALUES 
    (1, 1, 1, 750.00),
    (1, 3, 1, 1200.00),
    (2, 1, 1, 750.00),
    (3, 2, 1, 999.99),
    (4, 4, 3, 499.99),
    (5, 5, 1, 299.99),
    (6, 6, 1, 599.00),
    (7, 8, 1, 199.00),
    (8, 10, 1, 399.00),
    (9, 9, 1, 299.00)
GO

INSERT INTO Reviews (ProductId, CustomerId, ReviewText, Rating, ReviewDate)
VALUES 
    (1, 1, 'Excellent laptop for the price!', 5, CONVERT(DATETIME, '2024-10-22', 102)),
    (2, 3, 'Absolutely love this phone!', 4, CONVERT(DATETIME, '2024-10-23', 102)),
    (3, 2, 'Good refrigerator, but noisy', 3, CONVERT(DATETIME, '2024-10-23', 102)),
    (4, 4, 'Fantastic console, worth the wait!', 5, CONVERT(DATETIME, '2024-10-24', 102)),
    (5, 5, 'Great sound quality', 4, CONVERT(DATETIME, '2024-10-24', 102))
GO

INSERT INTO Discounts (ProductId, DiscountPercentage, StartDate, EndDate)
VALUES 
    (1, 10, CONVERT(DATETIME, '2024-10-01', 102), CONVERT(DATETIME, '2024-10-31', 102)),
    (3, 15, CONVERT(DATETIME, '2024-10-10', 102), CONVERT(DATETIME, '2024-10-20', 102)),
    (5, 5, CONVERT(DATETIME, '2024-10-15', 102), CONVERT(DATETIME, '2024-10-25', 102)),
    (6, 5, CONVERT(DATETIME, '2024-10-20', 102), CONVERT(DATETIME, '2024-10-30', 102)),
    (10, 12, CONVERT(DATETIME, '2024-10-15', 102), CONVERT(DATETIME, '2024-10-25', 102))
GO

INSERT INTO Inventory (ProductId, QuantityAvailable, ReorderThreshold)
VALUES 
    (1, 50, 10),
    (2, 100, 20),
    (3, 30, 5),
    (4, 20, 5),
    (5, 75, 15),
    (6, 150, 20),
    (7, 40, 5),
    (8, 200, 30),
    (9, 60, 10),
    (10, 120, 20)
GO

INSERT INTO ProductAttributes (ProductId, AttributeName, AttributeValue)
VALUES 
    (1, 'Screen Size', '15.6 inches'),
    (1, 'Processor', 'Intel i5'),
    (2, 'Storage', '256GB'),
    (2, 'Color', 'Black'),
    (3, 'Capacity', '500 Liters'),
    (5, 'Noise Cancelling', 'Yes'),
    (6, 'Storage', '64GB'),
    (6, 'Color', 'Silver'),
    (8, 'Battery Life', '7 days'),
    (9, 'Print Speed', '20 ppm')
GO

INSERT INTO PaymentMethods (MethodName)
VALUES 
    ('Credit Card'),
    ('PayPal'),
    ('Bank Transfer'),
    ('Cash on Delivery'),
    ('Google Pay')
GO

INSERT INTO OrderPayments (OrderId, PaymentMethodId, PaymentDate, AmountPaid)
VALUES 
    (1, 1, CONVERT(DATETIME, '2024-10-20', 102), 1250.00),
    (2, 2, CONVERT(DATETIME, '2024-10-21', 102), 750.00),
    (3, 1, CONVERT(DATETIME, '2024-10-22', 102), 999.99),
    (4, 3, CONVERT(DATETIME, '2024-10-23', 102), 1499.99),
    (5, 1, CONVERT(DATETIME, '2024-10-24', 102), 299.99)
GO

INSERT INTO Shipping (OrderId, ShippingDate, DeliveryDate, ShippingStatus, ShippingAddress)
VALUES 
    (1, CONVERT(DATETIME, '2024-10-21', 102), CONVERT(DATETIME, '2024-10-23', 102), 'Delivered', '101 Blue St, City'),
    (2, CONVERT(DATETIME, '2024-10-22', 102), CONVERT(DATETIME, '2024-10-25', 102), 'Delivered', '202 Green St, City'),
    (3, CONVERT(DATETIME, '2024-10-23', 102), CONVERT(DATETIME, '2024-10-26', 102), 'Shipped', '303 Red St, City'),
    (4, CONVERT(DATETIME, '2024-10-24', 102), CONVERT(DATETIME, '2024-10-27', 102), 'Processing', '404 Yellow St, City'),
    (5, CONVERT(DATETIME, '2024-10-25', 102), CONVERT(DATETIME, '2024-10-28', 102), 'Processing', '505 Purple St, City')
GO

INSERT INTO WishList (CustomerId, ProductId, DateAdded)
VALUES 
    (1, 2, '2024-10-01'),
    (2, 3, '2024-10-02'),
    (3, 5, '2024-10-03'),
    (4, 1, '2024-10-04'),
    (5, 4, '2024-10-05')
GO

INSERT INTO Users (Username, PasswordHash, Role)
VALUES 
    ('admin_user', HASHBYTES('SHA2_256', 'admin_password'), 'Admin'),
    ('regular_user', HASHBYTES('SHA2_256', 'user_password'), 'User'),
    ('tech_user', HASHBYTES('SHA2_256', 'tech_password'), 'User'),
    ('manager_admin', HASHBYTES('SHA2_256', 'manager_password'), 'Admin'),
    ('guest_user', HASHBYTES('SHA2_256', 'guest_password'), 'User')
GO