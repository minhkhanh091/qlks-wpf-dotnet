-- =====================================================
-- DONG TAT CA CAC KET NOI TRUOC KHI XOA DATABASE
-- =====================================================
USE master;
GO

IF DB_ID('QLKS') IS NOT NULL
BEGIN
    ALTER DATABASE QLKS SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLKS;
END
GO

CREATE DATABASE QLKS;
GO

USE QLKS;
GO

-- =====================================================
-- BẢNG KHÁCH HÀNG
-- =====================================================
CREATE TABLE KhachHang
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    DienThoai NVARCHAR(15) NULL,
    DiaChi NVARCHAR(200) NULL
);
GO

-- =====================================================
-- BẢNG TÀI KHOẢN
-- =====================================================
CREATE TABLE TaiKhoan
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    MatKhau NVARCHAR(100) NOT NULL,
    VaiTro NVARCHAR(20) NOT NULL,
    KhachHangId INT NULL,
    NhanVienId INT NULL
);
GO

-- =====================================================
-- BẢNG LOẠI PHÒNG
-- =====================================================
CREATE TABLE LoaiPhong
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(50) NOT NULL,
    Gia DECIMAL(18,2) NOT NULL,
    SucChua INT NOT NULL DEFAULT 2
);
GO

-- =====================================================
-- BẢNG PHÒNG
-- =====================================================
CREATE TABLE Phong
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SoPhong NVARCHAR(20) NOT NULL UNIQUE,
    LoaiPhongId INT NOT NULL,
    GiaMoiDem DECIMAL(18,2) NOT NULL,
    SucChua INT NOT NULL DEFAULT 2,
    MoTa NVARCHAR(500) NULL,
    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Trống',
    NgayCheckin DATETIME NULL,
    NgayCheckout DATETIME NULL,

    CONSTRAINT FK_Phong_LoaiPhong FOREIGN KEY (LoaiPhongId)
        REFERENCES LoaiPhong(Id)
);
GO

-- =====================================================
-- BẢNG ĐẶT PHÒNG
-- =====================================================
CREATE TABLE DatPhong
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    KhachHangId INT NOT NULL,
    PhongId INT NOT NULL,
    NgayCheckin DATETIME NOT NULL,
    NgayCheckout DATETIME NOT NULL,
    TrangThai NVARCHAR(50) NOT NULL DEFAULT N'Chờ xác nhận',
    TongTien DECIMAL(18,2) NULL,

    CONSTRAINT FK_DatPhong_KhachHang FOREIGN KEY (KhachHangId)
        REFERENCES KhachHang(Id),
    
    CONSTRAINT FK_DatPhong_Phong FOREIGN KEY (PhongId)
        REFERENCES Phong(Id)
);
GO

-- =====================================================
-- BẢNG NHÂN VIÊN
-- =====================================================
CREATE TABLE NhanVien
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    DienThoai NVARCHAR(15) NOT NULL,
    Email NVARCHAR(100) NULL,
    ChucVu NVARCHAR(50) NOT NULL,
    LuongCoBan DECIMAL(18,2) NOT NULL DEFAULT 0,
    NgayVaoLam DATETIME DEFAULT GETDATE(),
    HinhAnh NVARCHAR(500) NULL,
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

-- =====================================================
-- BẢNG DỊCH VỤ
-- =====================================================
CREATE TABLE DichVu
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenDichVu NVARCHAR(100) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    DonViTinh NVARCHAR(50) NULL
);
GO

-- =====================================================
-- BẢNG SỬ DỤNG DỊCH VỤ
-- =====================================================
CREATE TABLE SuDungDichVu
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DatPhongId INT NOT NULL,
    DichVuId INT NOT NULL,
    SoLuong INT NOT NULL DEFAULT 1,
    ThanhTien DECIMAL(18,2) NOT NULL,
    NgaySuDung DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_SuDungDichVu_DatPhong FOREIGN KEY (DatPhongId)
        REFERENCES DatPhong(Id) ON DELETE CASCADE,
    
    CONSTRAINT FK_SuDungDichVu_DichVu FOREIGN KEY (DichVuId)
        REFERENCES DichVu(Id) ON DELETE CASCADE
);
GO

-- =====================================================
-- THÊM KHÓA NGOẠI CHO TÀI KHOẢN
-- =====================================================
ALTER TABLE TaiKhoan
ADD CONSTRAINT FK_TaiKhoan_KhachHang FOREIGN KEY (KhachHangId)
    REFERENCES KhachHang(Id) ON DELETE SET NULL;

ALTER TABLE TaiKhoan
ADD CONSTRAINT FK_TaiKhoan_NhanVien FOREIGN KEY (NhanVienId)
    REFERENCES NhanVien(Id) ON DELETE SET NULL;
GO

-- =====================================================
-- THÊM CHECK CONSTRAINT (TIẾNG VIỆT CÓ DẤU)
-- =====================================================
ALTER TABLE Phong
ADD CONSTRAINT CK_Phong_TrangThai CHECK (TrangThai IN (N'Trống', N'Đã đặt', N'Đang ở', N'Đang sửa chữa'));
GO

ALTER TABLE TaiKhoan
ADD CONSTRAINT CK_TaiKhoan_VaiTro CHECK (VaiTro IN (N'Admin', N'Nhân viên', N'Khách hàng'));
GO

-- =====================================================
-- INSERT DỮ LIỆU MẪU (TIẾNG VIỆT CÓ DẤU)
-- =====================================================

-- 1. Thêm loại phòng
INSERT INTO LoaiPhong (TenLoai, Gia, SucChua) VALUES
(N'Thường', 300000, 2),
(N'Cao cấp', 700000, 2),
(N'VIP', 1200000, 2),
(N'Suite', 2000000, 4);
GO

-- 2. Thêm phòng
INSERT INTO Phong (SoPhong, LoaiPhongId, GiaMoiDem, SucChua, TrangThai, MoTa) VALUES
(N'P101', 1, 300000, 2, N'Trống', N'Phòng view thành phố'),
(N'P102', 1, 300000, 2, N'Trống', N'Phòng yên tĩnh'),
(N'P103', 2, 700000, 2, N'Trống', N'Phòng có ban công'),
(N'P104', 2, 700000, 2, N'Đã đặt', N'Phòng sang trọng'),
(N'P201', 3, 1200000, 2, N'Trống', N'Phòng VIP'),
(N'P202', 4, 2000000, 4, N'Đang ở', N'Phòng gia đình');
GO

INSERT INTO Phong (SoPhong, LoaiPhongId, GiaMoiDem, SucChua, TrangThai, MoTa) VALUES
(N'P108', 1, 320000, 2, N'Trống', N'Phòng gần thang máy'),
(N'P109', 1, 340000, 3, N'Trống', N'Phòng có cửa sổ lớn'),
(N'P205', 2, 720000, 2, N'Trống', N'Phòng view núi'),
(N'P206', 2, 780000, 3, N'Trống', N'Phòng có bồn tắm massage'),
(N'P207', 2, 690000, 2, N'Trống', N'Phòng decor hiện đại'),
(N'P301', 3, 1250000, 2, N'Trống', N'Phòng VIP có quầy bar mini'),
(N'P302', 3, 1400000, 3, N'Trống', N'Phòng VIP gia đình'),
(N'P401', 4, 2200000, 4, N'Trống', N'Suite có phòng khách riêng'),
(N'P402', 4, 2800000, 5, N'Trống', N'Suite cao cấp 2 phòng ngủ'),
(N'P403', 4, 3500000, 6, N'Trống', N'President Suite có bể bơi riêng');
GO

-- 3. Thêm khách hàng
INSERT INTO KhachHang (HoTen, DienThoai, DiaChi) VALUES
(N'Nguyễn Văn A', '0912345678', N'Hà Nội'),
(N'Trần Thị B', '0987654321', N'TP. Hồ Chí Minh'),
(N'Lê Văn C', '0909090909', N'Đà Nẵng');
GO

-- 4. Thêm nhân viên (TIẾNG VIỆT CÓ DẤU)
INSERT INTO NhanVien (HoTen, DienThoai, Email, ChucVu, LuongCoBan) VALUES
(N'Phạm Văn Quản Lý', '0912345678', 'quanly@hotel.com', N'Quản lý', 15000000),
(N'Hoàng Thị Lễ Tân', '0912345679', 'letan@hotel.com', N'Lễ tân', 8000000),
(N'Vũ Văn Bảo Vệ', '0912345680', 'baove@hotel.com', N'Bảo vệ', 6000000),
(N'Nguyễn Thị Tạp Vụ', '0912345681', 'tapvu@hotel.com', N'Tạp vụ', 5000000),
(N'Trần Văn Bếp', '0912345682', 'bep@hotel.com', N'Nhân viên bếp', 7000000);
GO

-- 5. Thêm tài khoản
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, KhachHangId, NhanVienId) VALUES
('admin', 'admin123', N'Admin', NULL, 1),
('letan1', 'letan123', N'Nhân viên', NULL, 2),
('khach1', 'khach123', N'Khách hàng', 1, NULL),
('khach2', 'khach123', N'Khách hàng', 2, NULL);
GO

-- 6. Thêm đặt phòng
INSERT INTO DatPhong (KhachHangId, PhongId, NgayCheckin, NgayCheckout, TrangThai, TongTien) VALUES
(1, 1, '2024-12-20 14:00:00', '2024-12-22 12:00:00', N'Đã xác nhận', 600000),
(2, 4, '2024-12-21 08:00:00', '2024-12-25 12:00:00', N'Đã nhận phòng', 2800000),
(1, 6, '2024-12-22 10:00:00', '2024-12-24 12:00:00', N'Đang ở', 4000000);
GO

-- 7. Thêm dịch vụ
INSERT INTO DichVu (TenDichVu, DonGia, DonViTinh) VALUES
(N'Ăn sáng buffet', 150000, N'suất'),
(N'Giặt ủi', 50000, N'kg'),
(N'Thuê xe máy', 120000, N'ngày'),
(N'Massage', 300000, N'giờ');
GO

-- 8. Thêm sử dụng dịch vụ
INSERT INTO SuDungDichVu (DatPhongId, DichVuId, SoLuong, ThanhTien) VALUES
(1, 1, 2, 300000),
(1, 2, 3, 150000),
(2, 1, 4, 600000),
(3, 3, 2, 240000);
GO

SELECT * FROM DatPhong