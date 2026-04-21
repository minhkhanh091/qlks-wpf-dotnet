IF DB_ID('QLKS') IS NOT NULL
    DROP DATABASE QLKS;
GO

CREATE DATABASE QLKS;
GO

USE QLKS;
GO


CREATE TABLE Customers
(
    Id INT IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(15),
    Address NVARCHAR(200),

    CONSTRAINT PK_Customers PRIMARY KEY (Id)
);
GO


CREATE TABLE Accounts
(
    Id INT IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    CustomerId INT,

    CONSTRAINT PK_Accounts PRIMARY KEY (Id),

    CONSTRAINT UQ_Accounts_Username UNIQUE (Username),

    CONSTRAINT FK_Accounts_Customers FOREIGN KEY (CustomerId)
        REFERENCES Customers(Id)
);
GO


CREATE TABLE RoomTypes
(
    Id INT IDENTITY(1,1),
    Name NVARCHAR(50),
    Price DECIMAL(10,2),

    CONSTRAINT PK_RoomTypes PRIMARY KEY (Id)
);
GO


-- =========================
-- ROOMS
-- =========================
CREATE TABLE Rooms
(
    Id INT IDENTITY(1,1),
    Name NVARCHAR(50),
    TypeId INT,
    Status NVARCHAR(20),

    CONSTRAINT PK_Rooms PRIMARY KEY (Id),

    CONSTRAINT FK_Rooms_RoomTypes FOREIGN KEY (TypeId)
        REFERENCES RoomTypes(Id)
);
GO


CREATE TABLE Bookings
(
    Id INT IDENTITY(1,1),
    CustomerId INT,
    RoomId INT,
    CheckIn DATETIME,
    CheckOut DATETIME,
    Status NVARCHAR(50),

    CONSTRAINT PK_Bookings PRIMARY KEY (Id),

    CONSTRAINT FK_Bookings_Customers FOREIGN KEY (CustomerId)
        REFERENCES Customers(Id),

    CONSTRAINT FK_Bookings_Rooms FOREIGN KEY (RoomId)
        REFERENCES Rooms(Id)
);
GO


CREATE TABLE Services
(
    Id INT IDENTITY(1,1),
    Name NVARCHAR(100),
    Price DECIMAL(10,2),

    CONSTRAINT PK_Services PRIMARY KEY (Id)
);
GO


CREATE TABLE ServiceUsage
(
    Id INT IDENTITY(1,1),
    BookingId INT,
    ServiceId INT,
    Quantity INT,

    CONSTRAINT PK_ServiceUsage PRIMARY KEY (Id),

    CONSTRAINT FK_ServiceUsage_Bookings FOREIGN KEY (BookingId)
        REFERENCES Bookings(Id),

    CONSTRAINT FK_ServiceUsage_Services FOREIGN KEY (ServiceId)
        REFERENCES Services(Id)
);
GO


CREATE TABLE Invoices
(
    Id INT IDENTITY(1,1),
    BookingId INT,
    Total DECIMAL(12,2),
    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT PK_Invoices PRIMARY KEY (Id),

    CONSTRAINT FK_Invoices_Bookings FOREIGN KEY (BookingId)
        REFERENCES Bookings(Id)
);
GO


ALTER TABLE Rooms
ADD CONSTRAINT CK_Rooms_Status
CHECK (Status IN (N'Trong', N'DangSuDung', N'BaoTri'));
GO


INSERT INTO RoomTypes(Name, Price) VALUES
(N'Thường', 300000),
(N'Cao cấp', 700000);

INSERT INTO Rooms(Name, TypeId, Status) VALUES
(N'P101', 1, N'Trong'),
(N'P102', 2, N'Trong');

INSERT INTO Customers(Name, Phone, Address) VALUES
(N'Nguyễn Văn A', '0123456789', N'Hà Nội');

INSERT INTO Accounts(Username, Password, Role, CustomerId) VALUES
('admin', '123', 'Admin', NULL),
('user1', '123', 'Customer', 1);

INSERT INTO Services(Name, Price) VALUES
(N'Ăn sáng', 50000),
(N'Giặt ủi', 30000);
GO