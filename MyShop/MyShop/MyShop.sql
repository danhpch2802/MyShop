USE [MyShop]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 4/4/2022 11:22:23 AM ******/
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
INSERT [dbo].[TaiKhoan] ([TaiKhoan_id], [TaiKhoan_username], [TaiKhoan_password]) VALUES (N'1', N'admin', N'jLIjfQZ5yojbZGTqxg2pY0VROWQ=')
GO
