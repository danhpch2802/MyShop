USE [master]
GO
/****** Object:  Database [MyShop]    Script Date: 4/6/2022 9:29:03 PM ******/
CREATE DATABASE [MyShop]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MyShop', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\MyShop.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MyShop_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\MyShop_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [MyShop] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MyShop].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MyShop] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MyShop] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MyShop] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MyShop] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MyShop] SET ARITHABORT OFF 
GO
ALTER DATABASE [MyShop] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MyShop] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MyShop] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MyShop] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MyShop] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MyShop] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MyShop] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MyShop] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MyShop] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MyShop] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MyShop] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MyShop] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MyShop] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MyShop] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MyShop] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MyShop] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MyShop] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MyShop] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MyShop] SET  MULTI_USER 
GO
ALTER DATABASE [MyShop] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MyShop] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MyShop] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MyShop] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MyShop] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MyShop] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [MyShop] SET QUERY_STORE = OFF
GO
USE [MyShop]
GO
/****** Object:  Table [dbo].[ChiTietDonHang]    Script Date: 4/6/2022 9:29:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDonHang](
	[DonHang_id] [int] NOT NULL,
	[HangHoa_id] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[Gia] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonHang]    Script Date: 4/6/2022 9:29:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHang](
	[DonHang_id] [int] IDENTITY(1,1) NOT NULL,
	[NgayMua] [date] NOT NULL,
	[TongGia] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DonHang_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HangHoa]    Script Date: 4/6/2022 9:29:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HangHoa](
	[HangHoa_id] [int] IDENTITY(1,1) NOT NULL,
	[HangHoa_img] [varchar](100) NULL,
	[HangHoa_ten] [nvarchar](100) NOT NULL,
	[HangHoa_hieu] [nvarchar](50) NULL,
	[HangHoa_gia] [decimal](18, 0) NOT NULL,
	[HangHoa_soluong] [int] NOT NULL,
 CONSTRAINT [PK_HangHoa] PRIMARY KEY CLUSTERED 
(
	[HangHoa_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hieu]    Script Date: 4/15/2022 11:56:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hieu](
	[Hieu_id] [int] IDENTITY(1,1) NOT NULL,
	[Hieu_ten] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Hieu] PRIMARY KEY CLUSTERED 
(
	[Hieu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Hieu] ON 

INSERT [dbo].[Hieu] ([Hieu_id], [Hieu_ten]) VALUES (1, N'Asus')
INSERT [dbo].[Hieu] ([Hieu_id], [Hieu_ten]) VALUES (2, N'Dell')
INSERT [dbo].[Hieu] ([Hieu_id], [Hieu_ten]) VALUES (3, N'Apple')
SET IDENTITY_INSERT [dbo].[Hieu] OFF
GO

/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 4/6/2022 9:29:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[TaiKhoan_id] [nvarchar](50) NOT NULL,
	[TaiKhoan_username] [nvarchar](50) NOT NULL,
	[TaiKhoan_password] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TaiKhoan] PRIMARY KEY CLUSTERED 
(
	[TaiKhoan_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[ChiTietDonHang] ([DonHang_id], [HangHoa_id], [SoLuong], [Gia]) VALUES (1, 1, 1, 50000000)
INSERT [dbo].[ChiTietDonHang] ([DonHang_id], [HangHoa_id], [SoLuong], [Gia]) VALUES (2, 2, 2, 160000000)
INSERT [dbo].[ChiTietDonHang] ([DonHang_id], [HangHoa_id], [SoLuong], [Gia]) VALUES (3, 3, 1, 24000000)
INSERT [dbo].[ChiTietDonHang] ([DonHang_id], [HangHoa_id], [SoLuong], [Gia]) VALUES (3, 4, 1, 14000000)
GO
SET IDENTITY_INSERT [dbo].[DonHang] ON 

INSERT [dbo].[DonHang] ([DonHang_id], [NgayMua], [TongGia]) VALUES (1, CAST(N'2022-04-05' AS Date), 50000000)
INSERT [dbo].[DonHang] ([DonHang_id], [NgayMua], [TongGia]) VALUES (2, CAST(N'2022-04-01' AS Date), 160000000)
INSERT [dbo].[DonHang] ([DonHang_id], [NgayMua], [TongGia]) VALUES (3, CAST(N'2022-01-01' AS Date), 38000000)
SET IDENTITY_INSERT [dbo].[DonHang] OFF
GO
SET IDENTITY_INSERT [dbo].[HangHoa] ON 

INSERT [dbo].[HangHoa] ([HangHoa_id], [HangHoa_img], [HangHoa_ten], [HangHoa_hieu], [HangHoa_gia], [HangHoa_soluong]) VALUES (1, '/Images/zenbook_fold.jpg', N'Zenbook Fold', N'Asus', CAST(25000000 AS Decimal(18, 0)), 100)
INSERT [dbo].[HangHoa] ([HangHoa_id], [HangHoa_img], [HangHoa_ten], [HangHoa_hieu], [HangHoa_gia], [HangHoa_soluong]) VALUES (2, '/Images/zenbook_duo.jpg', N'Zenbook Duo', N'Asus', CAST(30000000 AS Decimal(18, 0)), 100)
INSERT [dbo].[HangHoa] ([HangHoa_id], [HangHoa_img], [HangHoa_ten], [HangHoa_hieu], [HangHoa_gia], [HangHoa_soluong]) VALUES (3, '/Images/zenbook_vivobook.jpg', N'Zenbook Vivobook AMOLED', N'Asus', CAST(24000000 AS Decimal(18, 0)), 100)
INSERT [dbo].[HangHoa] ([HangHoa_id], [HangHoa_img], [HangHoa_ten], [HangHoa_hieu], [HangHoa_gia], [HangHoa_soluong]) VALUES (4, '/Images/alienware_m15.jpg', N'Alienware M15', N'Dell', CAST(14000000 AS Decimal(18, 0)), 100)
INSERT [dbo].[HangHoa] ([HangHoa_id], [HangHoa_img], [HangHoa_ten], [HangHoa_hieu], [HangHoa_gia], [HangHoa_soluong]) VALUES (5, '/Images/xps_13_pro.jpg', N'XPS 13 Pro', N'Dell', CAST(15000000 AS Decimal(18, 0)), 100)
INSERT [dbo].[HangHoa] ([HangHoa_id], [HangHoa_img], [HangHoa_ten], [HangHoa_hieu], [HangHoa_gia], [HangHoa_soluong]) VALUES (6, '/Images/macbook_pro_14.jpg', N'Macbook Pro 14"', N'Apple', CAST(45000000 AS Decimal(18, 0)), 100)
INSERT [dbo].[HangHoa] ([HangHoa_id], [HangHoa_img], [HangHoa_ten], [HangHoa_hieu], [HangHoa_gia], [HangHoa_soluong]) VALUES (7, '/Images/mac_mini_m1.jpg', N'Macbook Mini M1', N'Apple', CAST(33000000 AS Decimal(18, 0)), 100)
INSERT [dbo].[HangHoa] ([HangHoa_id], [HangHoa_img], [HangHoa_ten], [HangHoa_hieu], [HangHoa_gia], [HangHoa_soluong]) VALUES (8, '/Images/imac_24.jpg', N'iMac 24"', N'Apple', CAST(32000000 AS Decimal(18, 0)), 100)
SET IDENTITY_INSERT [dbo].[HangHoa] OFF
GO

INSERT [dbo].[TaiKhoan] ([TaiKhoan_id], [TaiKhoan_username], [TaiKhoan_password]) VALUES (N'1', N'admin', N'jLIjfQZ5yojbZGTqxg2pY0VROWQ=')
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD FOREIGN KEY([DonHang_id])
REFERENCES [dbo].[DonHang] ([DonHang_id])
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD FOREIGN KEY([HangHoa_id])
REFERENCES [dbo].[HangHoa] ([HangHoa_id])
GO
USE [master]
GO
ALTER DATABASE [MyShop] SET  READ_WRITE 
GO
