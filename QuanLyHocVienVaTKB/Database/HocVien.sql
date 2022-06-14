use master
drop database HOCVIENVATKB
drop database HOCVIENVATKB
create database HOCVIENVATKB
use HOCVIENVATKB

create table ADMIN
(
 username nvarchar(20) primary key,
 password nvarchar(30)
 )
 
create table GIANGVIEN
(
	GiangVienID int IDENTITY(1,1) primary key,
	TenGiangVien nvarchar(50),
	SoDienThoai varchar(20),
	HocVi nvarchar(20), 
	Email nvarchar(20),
	Matkhau varchar(30)
)

create table PHONGHOC 
(
	PhongHocId varchar(5) primary key,
	LoaiPhongHoc nvarchar (20),
	Diachi nvarchar (100),
	SucChuaPhongHoc int,
	TenPhongHoc nvarchar (20)
)

create table KHOAHOC
(
	KhoaHocID int IDENTITY(1,1) primary key,
	TenKhoaHoc nvarchar(50),
	Soluongsinhvien int
)

create table HOCVIEN
(
	HocVienID int IDENTITY(1,1) primary key,
	HoTen nvarchar(50),
	DiaChi nvarchar(100),
	SoDienThoai varchar(20),
	NgaySinh datetime,
	Email varchar(30),
	GioiTinh nvarchar (15),
	UserName varchar(10), 
	Password varchar(30), 
	BangDiemID int
	
)

create table CHITIETKHOAHOC
(
	ChiTietID int IDENTITY(1,1) primary key, 
	GiangVienID int,
	PhongHocId varchar(5),
	KhoaHocID int,
	Thu nvarchar (20),
	NgayBatDauKhoaHoc datetime, 
	NgayKetThucKhoaHoc datetime,
	SoTiet tinyint,
	TietBatDau tinyint,
)

create table THOIKHOABIEU
(
	ThoiKhoaBieuID int IDENTITY(1,1) primary key, 
	ChiTietID int,
	HocVienID int, 
	NgayBatDau datetime, 
	NgayKetThuc datetime,
	HocKy int,
)

create table BANGDIEM
(	
	BangDiemID int IDENTITY(1,1) primary key, 
	ChiTietID int,
	HocVienID int,
	HoTen nvarchar(50),
	Diem1 decimal(4,1), 
	Diem2 decimal(4,1),
	Diem3 decimal(4,1),
	TongDiem tinyint,
	XepLoai nvarchar (10), 
	LanThi tinyint
)

--khoa ngoai bang CTKH
alter table CHITIETKHOAHOC add constraint FK_CTKH_GV
foreign key (GiangVienID) references GIANGVIEN (GiangVienID)
alter table CHITIETKHOAHOC add constraint FK_CTKH_KH
foreign key (KhoaHocID) references KHOAHOC (KhoaHocID)
alter table CHITIETKHOAHOC add constraint FK_CTKH_PH
foreign key (PhongHocId) references PHONGHOC (PhongHocId)

--khoa ngoai bang Thoi Khoa Bieu
alter table THOIKHOABIEU add constraint FK_TKB_CTKH
foreign key (ChiTietID) references CHITIETKHOAHOC (ChiTietID)
alter table THOIKHOABIEU add constraint FK_TKB_HV
foreign key (HocVienID) references HOCVIEN (HocVienID)
--khoa ngoai bang BANGDIEM
alter table BANGDIEM add constraint FK_BD_HV
foreign key (HocVienID) references HOCVIEN (HocVienID)
alter table BANGDIEM add constraint FK_BD_CTKH
foreign key (ChiTietID) references CHITIETKHOAHOC (ChiTietID)
exec sp_helpconstraint CHITIETKHOAHOC
go
exec sp_helpconstraint THOIKHOABIEU
go
exec sp_helpconstraint BANGDIEM
go

