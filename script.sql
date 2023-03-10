USE [SQLTestDB]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_PropertiSales]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_PropertiSales](  
    @quantity INT,  
    @price DEC(10,2),  
    @discount DEC(3,2)  
)  
RETURNS DEC(10,2)  
AS   
BEGIN  
    RETURN @quantity * @price * (1 - @discount);  
END;

-- Scalar function in SQL Server always accepts parameters, either single or multiple and returns a single value. The scalar functions are useful in the simplification of our code.
--select [dbo].[FN_PropertiSales](1, 5000, 0.1) as NetSales;
GO
/****** Object:  UserDefinedFunction [dbo].[MULTIVALUED]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[MULTIVALUED]()  
RETURNS @Employee TABLE  
(Id INT, Name VARCHAR(50), Income INT, Department VARCHAR(10)) AS  
BEGIN  
    INSERT INTO @Employee  
    SELECT o.ID, o.Name, o.Income, o.Department from OBroInfo o  
    UPDATE @Employee SET Name = 'SYMN e' WHERE Id = 2;  
    RETURN  
END   


--select * from MULTIVALUED();
--select * from OBroInfo
GO
/****** Object:  Table [dbo].[ContactInfo]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContactInfo](
	[Id] [int] NOT NULL,
	[ContactNumber] [varchar](15) NULL,
 CONSTRAINT [PK_ContactInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DetailsAddress]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DetailsAddress](
	[Id] [int] NOT NULL,
	[Address] [varchar](50) NULL,
 CONSTRAINT [PK_BasicTableNew] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MonthlyExp]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MonthlyExp](
	[Id] [int] NULL,
	[Description] [varchar](50) NULL,
	[Amount] [int] NULL,
	[Month] [int] NULL,
	[Year] [int] NULL,
	[UId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_MonthlyExp] PRIMARY KEY CLUSTERED 
(
	[UId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OBroInfo]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OBroInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Department] [varchar](50) NULL CONSTRAINT [DF__SQLTest__dt1__108B795B]  DEFAULT (getdate()),
	[Income] [int] NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK__SQLTest__3214EC27BBCC765A] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Status]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Status](
	[Id] [int] NOT NULL,
	[CurrentLocation] [varchar](50) NULL,
	[Time] [datetime] NULL,
	[Expense] [int] NULL,
 CONSTRAINT [PK_TestDB] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[fudf_GetOsthirEmployee]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fudf_GetOsthirEmployee]()  
RETURNS TABLE  
AS  
 RETURN (SELECT * FROM OBroInfo)  


 --Table-valued functions in SQL Server are the user-defined function that returns data of a table type. Since this function's return type is a table, we can use it the same way as we use a table.
 -- select * from fudf_GetOsthirEmployee()
GO
INSERT [dbo].[ContactInfo] ([Id], [ContactNumber]) VALUES (1, N'01756441573')
INSERT [dbo].[ContactInfo] ([Id], [ContactNumber]) VALUES (2, N'01832880003')
INSERT [dbo].[ContactInfo] ([Id], [ContactNumber]) VALUES (3, N'79102457266')
INSERT [dbo].[ContactInfo] ([Id], [ContactNumber]) VALUES (4, N'01856028296')
INSERT [dbo].[ContactInfo] ([Id], [ContactNumber]) VALUES (5, N'01512658412')
INSERT [dbo].[ContactInfo] ([Id], [ContactNumber]) VALUES (6, N'01815124562')
INSERT [dbo].[DetailsAddress] ([Id], [Address]) VALUES (1, N'Dhanmondi 1207,Dhaka')
INSERT [dbo].[DetailsAddress] ([Id], [Address]) VALUES (2, N'Osio kanzi, Osaka')
INSERT [dbo].[DetailsAddress] ([Id], [Address]) VALUES (3, N'Frankfoot, Berlin')
INSERT [dbo].[DetailsAddress] ([Id], [Address]) VALUES (4, N'Mirpur 1212, Dhaka')
INSERT [dbo].[DetailsAddress] ([Id], [Address]) VALUES (5, N'Niketon, Dhaka')
INSERT [dbo].[DetailsAddress] ([Id], [Address]) VALUES (6, N'Gulsan, Dhaka')
SET IDENTITY_INSERT [dbo].[MonthlyExp] ON 

INSERT [dbo].[MonthlyExp] ([Id], [Description], [Amount], [Month], [Year], [UId]) VALUES (1, N'Eat food with friends', 500, 1, 2023, 1)
INSERT [dbo].[MonthlyExp] ([Id], [Description], [Amount], [Month], [Year], [UId]) VALUES (5, N'Eat food with friends at star kabab', 590, 1, 2023, 2)
INSERT [dbo].[MonthlyExp] ([Id], [Description], [Amount], [Month], [Year], [UId]) VALUES (2, N'Eat food with friends', 500, 1, 2023, 3)
INSERT [dbo].[MonthlyExp] ([Id], [Description], [Amount], [Month], [Year], [UId]) VALUES (1, N'Eat food with Wife', 1500, 1, 2023, 4)
INSERT [dbo].[MonthlyExp] ([Id], [Description], [Amount], [Month], [Year], [UId]) VALUES (3, N'Eat food with friends', 500, 1, 2023, 5)
INSERT [dbo].[MonthlyExp] ([Id], [Description], [Amount], [Month], [Year], [UId]) VALUES (4, N'Eat food with friends', 500, 1, 2023, 6)
INSERT [dbo].[MonthlyExp] ([Id], [Description], [Amount], [Month], [Year], [UId]) VALUES (6, N'Eat food with friends', 1600, 1, 2023, 7)
INSERT [dbo].[MonthlyExp] ([Id], [Description], [Amount], [Month], [Year], [UId]) VALUES (5, N'Eat', 2, 1, 2023, 8)
SET IDENTITY_INSERT [dbo].[MonthlyExp] OFF
SET IDENTITY_INSERT [dbo].[OBroInfo] ON 

INSERT [dbo].[OBroInfo] ([ID], [Name], [Department], [Income], [Status]) VALUES (1, N'Minhaz Uddin Emon', N'CSE', 200000, 1)
INSERT [dbo].[OBroInfo] ([ID], [Name], [Department], [Income], [Status]) VALUES (2, N'Ifratul Saymon', N'Mechanic', 150000, 2)
INSERT [dbo].[OBroInfo] ([ID], [Name], [Department], [Income], [Status]) VALUES (3, N'Azim Mahmood', N'EEE', 150000, 3)
INSERT [dbo].[OBroInfo] ([ID], [Name], [Department], [Income], [Status]) VALUES (4, N'Nahid Hasan Anik', N'Math', 150000, 1)
INSERT [dbo].[OBroInfo] ([ID], [Name], [Department], [Income], [Status]) VALUES (5, N'Sakib Sadman', N'Civil', 150000, 1)
INSERT [dbo].[OBroInfo] ([ID], [Name], [Department], [Income], [Status]) VALUES (6, N'Hasan Prince', N'Management', 150000, 1)
SET IDENTITY_INSERT [dbo].[OBroInfo] OFF
INSERT [dbo].[Status] ([Id], [CurrentLocation], [Time], [Expense]) VALUES (1, N'Bangladesh', CAST(N'2023-01-29 17:32:31.930' AS DateTime), 15000)
INSERT [dbo].[Status] ([Id], [CurrentLocation], [Time], [Expense]) VALUES (2, N'Japan', CAST(N'2023-01-29 17:35:17.510' AS DateTime), 50000)
INSERT [dbo].[Status] ([Id], [CurrentLocation], [Time], [Expense]) VALUES (3, N'Germany', CAST(N'2023-01-29 17:36:12.260' AS DateTime), 45000)
INSERT [dbo].[Status] ([Id], [CurrentLocation], [Time], [Expense]) VALUES (4, N'Arab', CAST(N'2023-02-26 00:00:00.000' AS DateTime), 50000)
INSERT [dbo].[Status] ([Id], [CurrentLocation], [Time], [Expense]) VALUES (5, N'USA', CAST(N'2023-02-26 14:13:56.070' AS DateTime), 15000000)
/****** Object:  StoredProcedure [dbo].[USP_GET_OsthirBroINFO]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- EXEC USP_GET_OsthirBroINFO
-- =============================================
-- Author:		<Author,Minhaz Uddin Emon>
-- Create date: <Create Date,31 Jan 2023>
-- Description:	<Description>
-- =============================================
CREATE PROCEDURE [dbo].[USP_GET_OsthirBroINFO]
	
AS
BEGIN
	
	SELECT 
	O.Name
	, O.Department
	,S.CurrentLocation
	, ISNULL(C.ContactNumber,0) AS ContactNumber
	,s.Time

	
	FROM OBroInfo O
	INNER JOIN Status S ON O.Status = S.Id
	LEFT JOIN ContactInfo C ON O.ID = C.Id
END


GO
/****** Object:  StoredProcedure [dbo].[USP_GET_OsthirBroMonthlySaving]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- EXEC USP_GET_OsthirBroMonthlySaving
-- =============================================
-- Author:		<Author,Minhaz Uddin Emon>
-- Create date: <Create Date,8 Feb 2023>
-- Description:	<Description>
-- =============================================
CREATE PROCEDURE [dbo].[USP_GET_OsthirBroMonthlySaving]
	
AS
BEGIN
select
	o.ID
	,o.Name
	,c.ContactNumber
	, d.Address + '-' + s.CurrentLocation as Address
	, o.Income 
	, s.Expense
	, case when
				m.Month = 1 then 'January'
				when m.Month = 2 then 'February'
				when m.Month = 3 then 'March'
				END MonthNam
	,sum(m.Amount) as MonthlyExpense
	, (s.Expense + sum(m.Amount)) TotalExpense
	, (o.Income - s.Expense - sum(m.Amount)) as CurrentAmmount
	from OBroInfo o
	inner join Status s on o.Status = s.Id
	left join MonthlyExp m on o.ID = m.Id
	left join ContactInfo c on o.ID = c.Id
	inner join DetailsAddress d on o.ID = d.Id
	group by o.Name, o.Income, s.CurrentLocation, s.Expense, m.Id, c.ContactNumber, o.ID, d.Address, m.Month
	order by o.ID ASC

END


GO
/****** Object:  StoredProcedure [dbo].[USP_INSERT_Status]    Script Date: 06-Mar-23 4:04:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- EXEC USP_INSERT_Status 7, '', 0
CREATE PROCEDURE [dbo].[USP_INSERT_Status]
@Id INT,
@CurrentLocation varchar(100) = '',
@Expense int
AS
BEGIN
INSERT INTO Status(Id, CurrentLocation, Time, Expense)
VALUES(@Id, @CurrentLocation, GETDATE(), @Expense);
END
GO
