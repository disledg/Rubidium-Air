-- Создание базы данных
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AirportDB')
BEGIN
    CREATE DATABASE AirportDB;
END
GO

USE AirportDB;
GO

-- Создание таблицы Aircraft
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Aircraft')
BEGIN
    CREATE TABLE Aircraft (
        Id INT PRIMARY KEY IDENTITY(1,1),
        registration_number NVARCHAR(50) NOT NULL UNIQUE,
        model NVARCHAR(100) NOT NULL,
        year_of_manufacture INT NOT NULL,
        passenger_capacity INT NOT NULL,
        cargo_capacity DECIMAL(18,2) NOT NULL
    );
END
GO

-- Создание таблицы AircraftMetrics
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AircraftMetrics')
BEGIN
    CREATE TABLE AircraftMetrics (
        Id INT PRIMARY KEY IDENTITY(1,1),
        aircraft_id INT NOT NULL,
        date DATETIME NOT NULL,
        fuel_consumption DECIMAL(18,2) NOT NULL,
        passenger_load INT NOT NULL,
        cargo_load DECIMAL(18,2) NOT NULL,
        water_level DECIMAL(18,2) NOT NULL,
        oil_level DECIMAL(18,2) NOT NULL,
        technical_condition DECIMAL(18,2) NOT NULL,
        FOREIGN KEY (aircraft_id) REFERENCES Aircraft(Id)
    );
END
GO

-- Создание таблицы Users
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT PRIMARY KEY IDENTITY(1,1),
        username NVARCHAR(50) NOT NULL UNIQUE,
        password NVARCHAR(100) NOT NULL,
        role NVARCHAR(20) NOT NULL
    );
END
GO

-- Создание таблицы Employees
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
BEGIN
    CREATE TABLE Employees (
        Id INT PRIMARY KEY IDENTITY(1,1),
        first_name NVARCHAR(50) NOT NULL,
        last_name NVARCHAR(50) NOT NULL,
        position NVARCHAR(100) NOT NULL,
        contact_number NVARCHAR(20),
        email NVARCHAR(100)
    );
END
GO

-- Создание таблицы Flights
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flights')
BEGIN
    CREATE TABLE Flights (
        Id INT PRIMARY KEY IDENTITY(1,1),
        flight_number NVARCHAR(20) NOT NULL UNIQUE,
        aircraft_id INT NOT NULL,
        departure_time DATETIME NOT NULL,
        arrival_time DATETIME NOT NULL,
        departure_airport NVARCHAR(50) NOT NULL,
        arrival_airport NVARCHAR(50) NOT NULL,
        status NVARCHAR(20) NOT NULL,
        FOREIGN KEY (aircraft_id) REFERENCES Aircraft(Id)
    );
END
GO

-- Создание таблицы Baggage
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Baggage')
BEGIN
    CREATE TABLE Baggage (
        Id INT PRIMARY KEY IDENTITY(1,1),
        flight_id INT NOT NULL,
        weight DECIMAL(18,2) NOT NULL,
        status NVARCHAR(20) NOT NULL,
        FOREIGN KEY (flight_id) REFERENCES Flights(Id)
    );
END
GO

-- Добавление тестового пользователя admin/admin
IF NOT EXISTS (SELECT * FROM Users WHERE username = 'admin')
BEGIN
    INSERT INTO Users (username, password)
    VALUES ('root', 'c9lnYwVtSWfKvC7h7EQZvw==:82dx/OtnxfYtpcj+341Uecbt8lebyeOFwG3tR8F7ano=');
END
GO 