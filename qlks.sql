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

INSERT INTO LoaiPhong (TenLoai, Gia, SucChua) VALUES
(N'Thường', 350000, 2),
(N'Cao cấp', 750000, 2),
(N'VIP', 1300000, 2),
(N'Suite', 2300000, 4);
GO

-- 2.2 PHÒNG (15 phòng - chi tiết hơn)
INSERT INTO Phong (SoPhong, LoaiPhongId, GiaMoiDem, SucChua, TrangThai, MoTa) VALUES
-- Tầng 1 (Phòng thường)
(N'101', 1, 350000, 2, N'Trống', N'Phòng đơn tiện nghi, có điều hòa'),
(N'102', 1, 350000, 2, N'Trống', N'Phòng view thành phố'),
(N'103', 1, 380000, 3, N'Trống', N'Phòng gia đình nhỏ'),
-- Tầng 2 (Phòng cao cấp)
(N'201', 2, 750000, 2, N'Trống', N'Ban công rộng, view biển'),
(N'202', 2, 750000, 2, N'Trống', N'Phòng có bồn tắm'),
(N'203', 2, 790000, 3, N'Trống', N'Phòng gia đình cao cấp'),
-- Tầng 3 (Phòng VIP)
(N'301', 3, 1300000, 2, N'Trống', N'Giường king size, quầy bar'),
(N'302', 3, 1300000, 2, N'Trống', N'Phòng có máy xông hơi'),
(N'303', 3, 1500000, 3, N'Trống', N'VIP gia đình 2 phòng ngủ'),
-- Tầng 4 (Suite)
(N'401', 4, 2300000, 4, N'Trống', N'Senior Suite có phòng khách'),
(N'402', 4, 2800000, 4, N'Trống', N'Family Suite 2PN + bếp'),
(N'403', 4, 3500000, 6, N'Trống', N'President Suite có bể bơi riêng'),
(N'404', 4, 4000000, 8, N'Trống', N'Suite cao cấp nhất, view toàn thành phố')
GO

-- 2.3 KHÁCH HÀNG (6 khách)
INSERT INTO KhachHang (HoTen, DienThoai, DiaChi) VALUES
(N'Nguyễn Văn An', '0912345001', N'12 Lý Thường Kiệt, Hà Nội'),
(N'Trần Thị Bích', '0987654002', N'45 Nguyễn Huệ, TP.HCM'),
(N'Lê Hoàng Cường', '0903123456', N'78 Trần Phú, Đà Nẵng'),
(N'Phạm Thị Dung', '0935789123', N'23 Lê Lợi, Huế'),
(N'Hoàng Văn Em', '0976543210', N'56 Hùng Vương, Nha Trang'),
(N'Đặng Thị Phương', '0912345888', N'90 Bạch Đằng, Hải Phòng');
GO

-- 2.4 NHÂN VIÊN (8 nhân viên)
INSERT INTO NhanVien (HoTen, DienThoai, Email, ChucVu, LuongCoBan, NgayVaoLam, TrangThai) VALUES
(N'Trần Văn Đạt', '0903123001', 'dat.tran@hotel.com', N'Quản lý', 18000000, '2023-01-10', 1),
(N'Lê Thị Hoa', '0903123002', 'hoa.le@hotel.com', N'Lễ tân', 8500000, '2023-03-15', 1),
(N'Nguyễn Văn Minh', '0903123003', 'minh.nguyen@hotel.com', N'Lễ tân', 8500000, '2023-06-20', 1),
(N'Phạm Thị Lan', '0903123004', 'lan.pham@hotel.com', N'Buồng phòng', 7000000, '2023-02-01', 1),
(N'Hoàng Văn Tùng', '0903123005', 'tung.hoang@hotel.com', N'Bảo vệ', 6500000, '2023-04-10', 1),
(N'Vũ Thị Mai', '0903123006', 'mai.vu@hotel.com', N'Nhân viên bếp', 7500000, '2023-05-05', 1),
(N'Đỗ Văn Khánh', '0903123007', 'khanh.do@hotel.com', N'Bảo trì', 7000000, '2023-07-12', 1),
(N'Bùi Thị Thu', '0903123008', 'thu.bui@hotel.com', N'Kế toán', 10000000, '2023-08-01', 0);
GO

-- 2.5 TÀI KHOẢN
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, KhachHangId, NhanVienId) VALUES
('admin', 'admin@123', N'Admin', NULL, 1),
('letanhoa', 'hoa@123', N'Nhân viên', NULL, 2),
('letanminh', 'minh@123', N'Nhân viên', NULL, 3),
('quanlydat', 'dat@123', N'Admin', NULL, 1),
('khach_an', 'an@123', N'Khách hàng', 1, NULL),
('khach_bich', 'bich@123', N'Khách hàng', 2, NULL),
('khach_cuong', 'cuong@123', N'Khách hàng', 3, NULL),
('khach_dung', 'dung@123', N'Khách hàng', 4, NULL);
GO

/*
-- 2.6 ĐẶT PHÒNG (8 đơn đặt - phong phú trạng thái)
INSERT INTO DatPhong (KhachHangId, PhongId, NgayCheckin, NgayCheckout, TrangThai, TongTien) VALUES
(1, 2, '2025-01-15 14:00:00', '2025-01-18 12:00:00', N'Đã xác nhận', 1050000),
(2, 5, '2025-01-16 08:00:00', '2025-01-20 12:00:00', N'Đã nhận phòng', 3000000),
(3, 8, '2025-01-17 10:00:00', '2025-01-19 12:00:00', N'Đang ở', 2600000),
(4, 11, '2025-01-18 14:00:00', '2025-01-22 12:00:00', N'Chờ xác nhận', 11200000),
(5, 3, '2025-01-20 12:00:00', '2025-01-23 12:00:00', N'Đã xác nhận', 1140000),
(6, 10, '2025-01-22 14:00:00', '2025-01-25 12:00:00', N'Chờ xác nhận', 6900000),
(1, 13, '2025-02-01 14:00:00', '2025-02-05 12:00:00', N'Đã đặt', 14000000),
(3, 7, '2025-02-10 14:00:00', '2025-02-12 12:00:00', N'Đã hủy', 0);
GO
*/

-- 2.7 DỊCH VỤ (8 dịch vụ mở rộng)
INSERT INTO DichVu (TenDichVu, DonGia, DonViTinh) VALUES
(N'Ăn sáng buffet', 150000, N'suất'),
(N'Ăn trưa', 120000, N'suất'),
(N'Ăn tối', 180000, N'suất'),
(N'Giặt ủi', 50000, N'kg'),
(N'Thuê xe máy', 150000, N'ngày'),
(N'Thuê ô tô 4 chỗ', 800000, N'ngày'),
(N'Massage Nhật', 400000, N'90 phút'),
(N'Gym pass', 100000, N'lượt');
GO

/*
-- 2.8 SỬ DỤNG DỊCH VỤ (12 bản ghi)
INSERT INTO SuDungDichVu (DatPhongId, DichVuId, SoLuong, ThanhTien, NgaySuDung) VALUES
(1, 1, 6, 900000, '2025-01-16 07:00:00'),   -- 2 người x 3 ngày
(1, 4, 5, 250000, '2025-01-17 09:00:00'),
(2, 1, 8, 1200000, '2025-01-17 07:00:00'),  -- 2 người x 4 ngày
(2, 5, 2, 300000, '2025-01-16 10:00:00'),
(2, 7, 2, 800000, '2025-01-18 20:00:00'),
(3, 1, 3, 450000, '2025-01-18 07:00:00'),
(3, 3, 2, 360000, '2025-01-17 19:00:00'),
(3, 8, 2, 200000, '2025-01-18 15:00:00'),
(5, 1, 6, 900000, '2025-01-21 07:00:00'),
(5, 5, 3, 450000, '2025-01-20 09:00:00'),
(6, 1, 6, 900000, '2025-01-23 07:00:00'),
(7, 7, 4, 1600000, '2025-02-02 18:00:00');
GO
*/
-- =====================================================
-- 3. KIỂM TRA DỮ LIỆU TỪNG BẢNG
-- =====================================================

PRINT '==================== KIỂM TRA DỮ LIỆU ====================';
GO

-- 3.1 Kiểm tra LoaiPhong
PRINT '1. Bảng LoaiPhong - Số dòng:';
SELECT COUNT(*) AS SoLuong FROM LoaiPhong;
SELECT * FROM LoaiPhong;
GO

-- 3.2 Kiểm tra Phong
PRINT '2. Bảng Phong - Số dòng:';
SELECT COUNT(*) AS SoLuong FROM Phong;
SELECT Id, SoPhong, LoaiPhongId, GiaMoiDem, SucChua, TrangThai FROM Phong ORDER BY Id;
GO

-- 3.3 Kiểm tra KhachHang
PRINT '3. Bảng KhachHang - Số dòng:';
SELECT COUNT(*) AS SoLuong FROM KhachHang;
SELECT * FROM KhachHang;
GO

-- 3.4 Kiểm tra NhanVien
PRINT '4. Bảng NhanVien - Số dòng:';
SELECT COUNT(*) AS SoLuong FROM NhanVien;
SELECT Id, HoTen, ChucVu, LuongCoBan, TrangThai FROM NhanVien ORDER BY Id;
GO

-- 3.5 Kiểm tra TaiKhoan
PRINT '5. Bảng TaiKhoan - Số dòng:';
SELECT COUNT(*) AS SoLuong FROM TaiKhoan;
SELECT Id, TenDangNhap, VaiTro, KhachHangId, NhanVienId FROM TaiKhoan ORDER BY Id;
GO

-- 3.6 Kiểm tra DatPhong (JOIN để dễ theo dõi)
PRINT '6. Bảng DatPhong - Số dòng:';
SELECT COUNT(*) AS SoLuong FROM DatPhong;
SELECT 
    dp.Id, 
    kh.HoTen AS KhachHang, 
    p.SoPhong AS Phong,
    dp.NgayCheckin, 
    dp.NgayCheckout, 
    dp.TrangThai, 
    dp.TongTien
FROM DatPhong dp
JOIN KhachHang kh ON dp.KhachHangId = kh.Id
JOIN Phong p ON dp.PhongId = p.Id
ORDER BY dp.Id;
GO

-- 3.7 Kiểm tra DichVu
PRINT '7. Bảng DichVu - Số dòng:';
SELECT COUNT(*) AS SoLuong FROM DichVu;
SELECT * FROM DichVu;
GO

-- 3.8 Kiểm tra SuDungDichVu (JOIN để dễ theo dõi)
PRINT '8. Bảng SuDungDichVu - Số dòng:';
SELECT COUNT(*) AS SoLuong FROM SuDungDichVu;
SELECT 
    sdd.Id,
    dp.Id AS DatPhongId,
    kh.HoTen AS KhachHang,
    dv.TenDichVu,
    sdd.SoLuong,
    sdd.ThanhTien,
    sdd.NgaySuDung
FROM SuDungDichVu sdd
JOIN DatPhong dp ON sdd.DatPhongId = dp.Id
JOIN KhachHang kh ON dp.KhachHangId = kh.Id
JOIN DichVu dv ON sdd.DichVuId = dv.Id
ORDER BY sdd.Id;
GO