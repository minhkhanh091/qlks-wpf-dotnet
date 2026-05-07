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


CREATE TABLE LoaiPhong
(
    Id INT IDENTITY(1,1),
    Name NVARCHAR(50),
    Price DECIMAL(10,2),
	SucChua INT NOT NULL DEFAULT 2,

    CONSTRAINT PK_RoomTypes PRIMARY KEY (Id)
);
GO

CREATE TABLE Phong
(
    Id INT IDENTITY(1,1),
    SoPhong NVARCHAR(20) NOT NULL,
    LoaiPhongId INT NOT NULL,
    GiaMoiDem DECIMAL(18,2) NOT NULL,
    SucChua INT NOT NULL DEFAULT 2,
    MoTa NVARCHAR(500) NULL,
    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Trống',
    NgayCheckin DATETIME NULL,
    NgayCheckout DATETIME NULL,

    CONSTRAINT PK_Phong PRIMARY KEY (Id),

    CONSTRAINT FK_Phong_LoaiPhong FOREIGN KEY (LoaiPhongId)
        REFERENCES LoaiPhong(Id)
);
GO


CREATE TABLE DatPhong
(
    Id INT IDENTITY(1,1),
    MaKhachHang INT,
    MaPhong INT,
    NgayCheckin DATETIME,
    NgayCheckout DATETIME,
    TrangThai NVARCHAR(50),

    CONSTRAINT PK_DatPhong PRIMARY KEY (Id),

    CONSTRAINT FK_DatPhong_KhachHang FOREIGN KEY (KhangHangId)
        REFERENCES Customers(Id),

    CONSTRAINT FK_DatPhong_Phong FOREIGN KEY (MaPhong)
        REFERENCES Phong(Id)
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

    CONSTRAINT FK_ServiceUsage_DatPhong FOREIGN KEY (BookingId)
        REFERENCES DatPhong(Id),

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

    CONSTRAINT FK_Invoices_DatPhong FOREIGN KEY (BookingId)
        REFERENCES DatPhong(Id)
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