--CREATING DATABASE 

Create DATABASE CarRentalDB;
USE CarRentalDB;

--Laying Down DB Table Schema

-- Vehicle Table
CREATE TABLE Vehicle (
    vehicleID INT PRIMARY KEY IDENTITY(1,1),
    make VARCHAR(50),
    model VARCHAR(50),
    year INT,
    dailyRate DECIMAL(10, 2),
    status VARCHAR(20) CHECK (status IN ('available', 'notAvailable')),
    passengerCapacity INT,
    engineCapacity VARCHAR(50)
);

-- Customer Table
CREATE TABLE Customer (
    customerID INT PRIMARY KEY IDENTITY(1,1),
    firstName VARCHAR(50),
    lastName VARCHAR(50),
    email VARCHAR(100) UNIQUE,
    phoneNumber VARCHAR(20) UNIQUE
);

-- Lease Table
CREATE TABLE Lease (
    leaseID INT PRIMARY KEY IDENTITY(1,1),
    vehicleID INT,
    customerID INT,
    startDate DATE,
    endDate DATE,
    type VARCHAR(20) CHECK (type IN ('DailyLease', 'MonthlyLease')),
    CONSTRAINT FK_Lease_Vehicle FOREIGN KEY (vehicleID) REFERENCES Vehicle(vehicleID),
    CONSTRAINT FK_Lease_Customer FOREIGN KEY (customerID) REFERENCES Customer(customerID)
);

-- Payment Table
CREATE TABLE Payment (
    paymentID INT PRIMARY KEY IDENTITY(1,1),
    leaseID INT,
    paymentDate DATE,
    amount DECIMAL(10, 2),
    CONSTRAINT FK_Payment_Lease FOREIGN KEY (leaseID) REFERENCES Lease(leaseID)
);

-- Populating Tables With Sample Values

-- Vehicle Table
INSERT INTO Vehicle (make, model, year, dailyRate, status, passengerCapacity, engineCapacity) VALUES
('Lamborghini', 'Huracan', 2023, 999.99, 'available', 2, '5.2L V10'),
('Ferrari', '488 GTB', 2022, 899.99, 'available', 2, '3.9L V8'),
('Porsche', '911 Turbo S', 2023, 749.99, 'notAvailable', 2, '3.8L H6'),
('McLaren', '720S', 2023, 1049.99, 'available', 2, '4.0L V8'),
('Aston Martin', 'DB11', 2021, 599.99, 'available', 4, '5.2L V12'),
('Rolls-Royce', 'Ghost', 2022, 1199.99, 'available', 4, '6.75L V12'),
('Bentley', 'Continental GT', 2021, 799.99, 'available', 4, '6.0L W12'),
('Bugatti', 'Chiron', 2023, 2999.99, 'notAvailable', 2, '8.0L W16'),
('Maserati', 'Quattroporte', 2020, 499.99, 'available', 5, '3.0L V6'),
('Tesla', 'Model S Plaid', 2023, 699.99, 'available', 5, 'Electric');

-- Customer Table
INSERT INTO Customer (firstName, lastName, email, phoneNumber) VALUES
('Sanskar', 'Kakde', 'sanskarkakde13@gmail.com', '6267609084'),
('Priya', 'Kumar', 'priya.kumar@outlook.com', '9876543211'),
('Suresh', 'Patel', 'suresh.patel@yahoo.com', '9876543212'),
('Anjali', 'Rao', 'anjali.rao@rediffmail.com', '9876543213'),
('Ravi', 'Singh', 'ravi.singh@gmail.com', '9876543214'),
('Sneha', 'Desai', 'sneha.desai@icloud.com', '9876543215'),
('Manoj', 'Mehta', 'manoj.mehta@hotmail.com', '9876543216'),
('Arjun', 'Verma', 'arjun.verma@inbox.com', '9876543217'),
('Neha', 'Kapoor', 'neha.kapoor@protonmail.com', '9876543218'),
('Vikas', 'Chawla', 'vikas.chawla@gmail.com', '9876543219');

-- Lease Table
INSERT INTO Lease (vehicleID, customerID, startDate, endDate, type) VALUES
(1, 1, '2024-01-01', '2024-01-07', 'DailyLease'),
(2, 2, '2024-02-01', '2024-02-28', 'MonthlyLease'),
(3, 3, '2024-03-05', '2024-03-12', 'DailyLease'),
(4, 4, '2024-04-10', '2024-04-20', 'DailyLease'),
(5, 5, '2024-05-15', '2024-05-30', 'MonthlyLease'),
(6, 6, '2024-06-01', '2024-06-10', 'DailyLease'),
(7, 7, '2024-07-01', '2024-07-15', 'MonthlyLease'),
(8, 8, '2024-08-01', '2024-08-10', 'DailyLease'),
(9, 9, '2024-09-01', '2024-09-07', 'DailyLease'),
(10, 10, '2024-10-01', '2024-10-31', 'MonthlyLease');

-- Payment Table
INSERT INTO Payment (leaseID, paymentDate, amount) VALUES
(1, '2024-01-07', 6999.93),
(2, '2024-02-28', 25199.72),
(3, '2024-03-12', 5249.93),
(4, '2024-04-20', 10499.90),
(5, '2024-05-30', 17999.70),
(6, '2024-06-10', 11999.90),
(7, '2024-07-15', 23999.85),
(8, '2024-08-10', 20999.90),
(9, '2024-09-07', 4899.93),
(10, '2024-10-31', 20999.70);
