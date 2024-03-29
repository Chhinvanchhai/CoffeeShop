USE [master]
GO
/****** Object:  Database [poscoffe]    Script Date: 6/30/2020 3:02:50 PM ******/
CREATE DATABASE [poscoffe]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'POSCoffee', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.CHHAISERVER\MSSQL\DATA\poscoffe.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'POSCoffee_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.CHHAISERVER\MSSQL\DATA\poscoffe_0.ldf' , SIZE = 7616KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [poscoffe] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [poscoffe].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [poscoffe] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [poscoffe] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [poscoffe] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [poscoffe] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [poscoffe] SET ARITHABORT OFF 
GO
ALTER DATABASE [poscoffe] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [poscoffe] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [poscoffe] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [poscoffe] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [poscoffe] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [poscoffe] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [poscoffe] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [poscoffe] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [poscoffe] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [poscoffe] SET  DISABLE_BROKER 
GO
ALTER DATABASE [poscoffe] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [poscoffe] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [poscoffe] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [poscoffe] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [poscoffe] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [poscoffe] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [poscoffe] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [poscoffe] SET RECOVERY FULL 
GO
ALTER DATABASE [poscoffe] SET  MULTI_USER 
GO
ALTER DATABASE [poscoffe] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [poscoffe] SET DB_CHAINING OFF 
GO
ALTER DATABASE [poscoffe] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [poscoffe] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [poscoffe] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'poscoffe', N'ON'
GO
ALTER DATABASE [poscoffe] SET QUERY_STORE = OFF
GO
USE [poscoffe]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_getDigitID]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[fn_getDigitID](@number int,@digit int) returns varchar(50)
as 
begin 
	return (REPLICATE('0',@digit-len(CONVERT(varchar(6),@number)))+Convert(varchar(6),@number))
end 

GO
/****** Object:  UserDefinedFunction [dbo].[fn_sum_expens]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_sum_expens](@datefrom DATE,@dateto DATE) RETURNS DECIMAL(20,2)
AS
BEGIN
	RETURN (SELECT SUM(amount) FROM dbo.tbexpens WHERE ex_date BETWEEN @datefrom AND @dateto)  
END

GO
/****** Object:  UserDefinedFunction [dbo].[fn_sum_sell]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_sum_sell](@datefrom DATE,@dateto DATE) RETURNS DECIMAL(20,2)
AS
BEGIN
	RETURN (SELECT SUM(payment) FROM dbo.tblInv WHERE InvDate BETWEEN @datefrom AND @dateto)  
END


GO
/****** Object:  Table [dbo].[tblCat]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCat](
	[CatID] [varchar](255) NOT NULL,
	[CatName] [nvarchar](255) NULL,
	[Image] [nvarchar](255) NULL,
	[description] [nvarchar](max) NULL,
	[createdate] [date] NULL,
	[createby] [nvarchar](50) NULL,
	[updatedate] [date] NULL,
	[updateby] [date] NULL,
	[lock] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[v_cate]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_cate]
AS
SELECT 
	CatID AS 'លេខកូត'
	
 FROM dbo.tblCat


GO
/****** Object:  Table [dbo].[tblProduct]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProduct](
	[ProID] [varchar](50) NOT NULL,
	[ProName] [nvarchar](255) NULL,
	[Price] [float] NULL,
	[Photo] [varbinary](max) NULL,
	[CatID] [nvarchar](255) NULL,
	[note] [nvarchar](max) NULL,
	[createddate] [date] NULL,
	[createby] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProductDetail]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProductDetail](
	[proid] [varchar](100) NULL,
	[type] [nvarchar](50) NULL,
	[price] [decimal](18, 2) NULL,
	[discount] [decimal](18, 2) NULL,
	[islocked] [tinyint] NULL,
	[total] [decimal](18, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[v_product]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_product]
AS
SELECT p.ProID ,
       p.ProName ,
       p.Photo ,
       p.CatID ,
	   c.CatName,
       p.note ,
       d.type ,
       d.price 
FROM dbo.tblProduct p INNER JOIN dbo.tblProductDetail d
ON p.ProID=d.proid INNER JOIN dbo.tblCat c
ON p.CatID=c.CatID

GO
/****** Object:  Table [dbo].[tbexpens]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbexpens](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ex_date] [date] NULL,
	[amount] [decimal](20, 2) NULL,
	[descriptions] [nvarchar](max) NULL,
	[createdby] [nvarchar](100) NULL,
	[createdate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblExchange]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblExchange](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[rate] [decimal](18, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblInv]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblInv](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[InvID] [varchar](100) NOT NULL,
	[InvDate] [date] NULL,
	[InvTime] [time](7) NULL,
	[Amount] [decimal](18, 2) NULL,
	[discount] [decimal](18, 2) NULL,
	[payment] [decimal](18, 2) NULL,
	[createdate] [date] NULL,
	[UserID] [nvarchar](50) NULL,
	[waiting] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblInv] PRIMARY KEY CLUSTERED 
(
	[InvID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblInv_Detail]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblInv_Detail](
	[InvID] [varchar](100) NULL,
	[ProID] [varchar](100) NULL,
	[ProName] [nvarchar](255) NULL,
	[size] [varchar](50) NULL,
	[Qty] [int] NULL,
	[Price] [decimal](18, 2) NULL,
	[discount] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblreport]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblreport](
	[ProID] [nvarchar](50) NULL,
	[ProName] [nvarchar](255) NULL,
	[Qty] [int] NULL,
	[Total] [int] NULL,
	[Inv_Date] [date] NULL,
	[UserID] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbltest]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbltest](
	[photo] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblType]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblType](
	[typeid] [varchar](50) NULL,
	[type] [nvarchar](50) NULL,
	[islocked] [tinyint] NULL,
	[created_date] [date] NULL,
	[created_by] [nvarchar](50) NULL,
	[update_date] [date] NULL,
	[update_by] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUser]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUser](
	[UserID] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[Phone] [nvarchar](255) NULL,
	[Photo] [nvarchar](255) NULL,
	[position] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbprint]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbprint](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[num] [varchar](100) NULL,
	[createdate] [date] NULL,
	[createby] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbwifi]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbwifi](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[pass] [varchar](100) NULL,
	[createdate] [date] NULL,
	[createby] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[insert_Inv]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[insert_Inv](
	@UserID nvarchar(50),
	@InvDate DATE,
	@invTime TIME,
	@Amount FLOAT,
	@discount FLOAT,
	@payment FLOAT,
	@id VARCHAR(100) OUTPUT
) as
BEGIN

DECLARE @wait VARCHAR(50)
DECLARE @num VARCHAR(50)

--SELECT @num=CASE WHEN (SELECT ISNULL(MAX(waiting),0)+1 FROM dbo.tblInv)=99 THEN 1 ELSE (SELECT ISNULL(MAX(waiting),0)+1 FROM dbo.tblInv) END;
SELECT @wait=dbo.fn_getDigitID((CASE WHEN (SELECT ISNULL(MAX(waiting),0)+1 FROM dbo.tblInv)=99 THEN 0 ELSE (SELECT ISNULL(MAX(waiting),0)+1 FROM dbo.tblInv) END),3);
SELECT @id=dbo.fn_getDigitID((SELECT ISNULL(MAX(InvID),0)+1 FROM dbo.tblInv),4);

	insert into tblInv(InvID,UserID,InvDate,Amount,discount,payment,createdate,InvTime,waiting) values(@id,@UserID,@InvDate,@Amount,@discount,@payment,GETDATE(),@invTime,@wait)
END

GO
/****** Object:  StoredProcedure [dbo].[mostProduct]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[mostProduct]
@dtFrom date,
@dtTo date
as
begin
	
		select top 5  ProName,ProID,sum(Qty) as tal from tblInv_Detail inner join tblInv on tblInv_Detail.InvID=tblInv.InvID
		 where tblInv.InvDate between @dtFrom and @dtTo  group by ProID,ProName order by tal desc

end

GO
/****** Object:  StoredProcedure [dbo].[mostProductNoCon]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[mostProductNoCon]

as 

begin
	select top 5 ProName,ProID,sum(Qty) as tal from tblInv_Detail group by ProID,ProName order by tal desc

end

GO
/****** Object:  StoredProcedure [dbo].[sp_cate]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_cate]

@name NVARCHAR(255),
@dec NVARCHAR(max),
@createby NVARCHAR(255)
AS
BEGIN
	DECLARE @cateid VARCHAR(255)
	select @cateid=dbo.fn_getDigitID(isnull(max(CatID),0)+1 ,4) from dbo.tblCat
	INSERT INTO dbo.tblCat
	        ( CatID, CatName, description,createdate,createby )
	VALUES  (@cateid, -- CatID - nvarchar(255)
	          @name, -- CatName - nvarchar(255)
	          @dec,  -- Image - nvarchar(255)
			  GETDATE(),
			  @createby
	          )
END


GO
/****** Object:  StoredProcedure [dbo].[sp_cate_e]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_cate_e]

@name NVARCHAR(255),
@dec NVARCHAR(max),
@createby NVARCHAR(255),
@id VARCHAR(50)
AS
BEGIN
	UPDATE dbo.tblCat SET CatName=@name,description=@dec,updateby=@createby,updatedate=GETDATE() WHERE CatID=@id          
END

GO
/****** Object:  StoredProcedure [dbo].[sp_expens]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_expens](
@ex_date DATE ,
@amount DECIMAL(20,2),
@descriptions NVARCHAR(max),
@createdby NVARCHAR(10) 
)
AS

BEGIN
    INSERT INTO dbo.tbexpens
            ( ex_date ,
              amount ,
              descriptions ,
              createdby ,
              createdate
            )
    VALUES  (   @ex_date ,
				@amount,
				@descriptions ,
				@createdby, 
              GETDATE()
            )
END

GO
/****** Object:  StoredProcedure [dbo].[sp_insert_inv_detail]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[sp_insert_inv_detail](
			  @InvID VARCHAR(100),
              @ProID VARCHAR(100) ,
              @ProName NVARCHAR(100),
              @Qty INT,
              @Price DECIMAL(18,2),
              @Total DECIMAL(18,2),
			  @size VARCHAR(10),
			  @decount DECIMAL(18,2)

)
AS
BEGIN
    INSERT INTO dbo.tblInv_Detail
            ( InvID ,
              ProID ,
              ProName ,
              Qty ,
              Price ,
              Total,
			  size,
			  discount
            )
    VALUES  ( @InvID ,
              @ProID  ,
              @ProName ,
              @Qty ,
              @Price ,
              @Total ,
			  @size ,
			  @decount 
            )
END

GO
/****** Object:  StoredProcedure [dbo].[sp_invoice]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_invoice](
	@invID VARCHAR(100)
)
AS 
BEGIN
    SELECT 
		i.InvID,
		i.InvDate,
		i.InvTime,
		i.Amount,
		i.discount,
		i.payment,
		d.ProName,
		d.discount AS dis,
		d.size,
		d.Qty,
		d.Price,
		d.Total,
		i.UserID,
		i.waiting
	 FROM dbo.tblInv i INNER JOIN
	dbo.tblInv_Detail d ON
    i.InvID=d.InvID WHERE i.InvID=@invID
END

GO
/****** Object:  StoredProcedure [dbo].[sp_product_detail_i]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[sp_product_detail_i]
(
	@pid VARCHAR(50),
	@type VARCHAR(5),
	@price DECIMAL(20,2),
	@discount DECIMAL(18,2)

)
AS

BEGIN
    DECLARE @total DECIMAL(18,2)
	SET @total=@price-(@price*@discount/100)
	INSERT INTO dbo.tblProductDetail
	        ( proid, type, price, islocked,discount,total)
	VALUES  ( @pid,
			  @type,
			  @price,
	          0  -- islocked - tinyint
			  ,@discount,
			  @total
	          )
END



GO
/****** Object:  StoredProcedure [dbo].[sp_product_e]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_product_e]
(
	@proname NVARCHAR(100),
	@photo VARBINARY(MAX),
	@cateid NVARCHAR(255),
	@note NVARCHAR(MAX),
	@createdby NVARCHAR(100),
	@id VARCHAR(50)
)
	AS
    
BEGIN
   update  dbo.tblProduct SET ProName=@proname, Photo=@photo, CatID=@cateid ,note=@note,createby=@createdby WHERE ProID=@id
   DELETE FROM dbo.tblProductDetail WHERE proid=@id
              
END

GO
/****** Object:  StoredProcedure [dbo].[sp_product_i]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_product_i]
(
	@proname NVARCHAR(100),
	@photo VARBINARY(MAX),
	@cateid NVARCHAR(255),
	@note NVARCHAR(MAX),
	@createdby NVARCHAR(100),
	@id VARCHAR(50) output
)
	AS
    
BEGIN
SELECT @id=dbo.fn_getDigitID((SELECT ISNULL(MAX(ProID),0)+1 FROM dbo.tblProduct),4);
   INSERT INTO dbo.tblProduct
           ( ProID,ProName, Price, Photo, CatID ,note,createby,createddate)
   VALUES  (@id, @proname, 0, @photo, @cateid,@note,@createdby,GETDATE())
   
              
END

GO
/****** Object:  StoredProcedure [dbo].[sp_rpt_change_time]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[sp_rpt_change_time]
    (
      @datefrom DATE ,
      @dateto DATE ,
      @userID NVARCHAR(100)
    )
AS
    BEGIN
        SELECT 
	 DISTINCT   d.ProID ,
                d.ProName ,
                d.Price ,
                d.size ,
                ( SELECT    SUM(l.Qty)
                  FROM      dbo.tblInv_Detail l INNER JOIN dbo.tblInv v ON l.InvID=v.InvID
                  WHERE     l.ProID = d.ProID
                            AND l.size = d.size AND v.UserID=i.UserID and v.InvDate BETWEEN @datefrom AND @dateto
                           
                ) AS qty ,
                ( SELECT    SUM(l.Qty)
                  FROM      dbo.tblInv_Detail l INNER JOIN dbo.tblInv v ON l.InvID=v.InvID
                  WHERE     l.ProID = d.ProID
                            AND l.size = d.size AND v.UserID=i.UserID and v.InvDate BETWEEN @datefrom AND @dateto
                           
                ) * d.Price AS Total ,
                u.Name ,
                u.UserID
        FROM    dbo.tblInv i
                INNER JOIN dbo.tblInv_Detail d ON i.InvID = d.InvID
                INNER JOIN dbo.tblUser u ON i.UserID = u.UserID
        WHERE   i.InvDate BETWEEN @datefrom AND @dateto
                AND i.UserID = @userID;
    END;
GO
/****** Object:  StoredProcedure [dbo].[sp_rpt_checkout]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_rpt_checkout](
@datefrom DATE,
@dateto DATE
)

AS
BEGIN
    SELECT payment AS amount,N'លក់' AS Note,'Income' AS status FROM dbo.tblInv WHERE InvDate BETWEEN @datefrom AND @dateto
	UNION ALL
    SELECT amount AS amount,descriptions AS Note,'Expens' AS status FROM dbo.tbexpens WHERE ex_date BETWEEN @datefrom AND @dateto
END

GO
/****** Object:  StoredProcedure [dbo].[sp_rpt_daily_checkout]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_rpt_daily_checkout]
    (
      @datefrom DATE ,
      @dateto DATE
    )
AS
    BEGIN
         SELECT 
	 DISTINCT   d.ProID ,
                d.ProName ,
                d.Price ,
                d.size ,
                ( SELECT    SUM(l.Qty)
                  FROM      dbo.tblInv_Detail l INNER JOIN dbo.tblInv v ON l.InvID=v.InvID
                  WHERE     l.ProID = d.ProID
                            AND l.size = d.size AND v.UserID=i.UserID
                           
                ) AS qty ,
                ( SELECT    SUM(l.Qty)
                  FROM      dbo.tblInv_Detail l INNER JOIN dbo.tblInv v ON l.InvID=v.InvID
                  WHERE     l.ProID = d.ProID
                            AND l.size = d.size AND v.UserID=i.UserID
                           
                ) * d.Price AS Total ,
                u.Name ,
                u.UserID
        FROM    dbo.tblInv i
                INNER JOIN dbo.tblInv_Detail d ON i.InvID = d.InvID
                INNER JOIN dbo.tblUser u ON i.UserID = u.UserID
        WHERE   i.InvDate BETWEEN @datefrom AND @dateto
    END;

--WHERE i.InvDate BETWEEN @datefrom AND @dateto


GO
/****** Object:  StoredProcedure [dbo].[sp_rpt_expense]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE PROC [dbo].[sp_rpt_expense]
(
@datefrom DATE,
@dateto DATE
)
AS BEGIN
       
	   SELECT x.id ,
              x.ex_date ,
              x.amount ,
              x.descriptions ,
              x.createdby ,
              u.Name
               FROM
       dbo.tbexpens x INNER JOIN dbo.tblUser u
	   ON x.createdby=u.UserID
	    WHERE ex_date BETWEEN @datefrom AND @dateto 
   END
GO
/****** Object:  StoredProcedure [dbo].[sp_rpt_expense_user]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_rpt_expense_user]
(
@datefrom DATE,
@dateto DATE,
@user VARCHAR(100)
)
AS BEGIN
       
	   SELECT x.id ,
              x.ex_date ,
              x.amount ,
              x.descriptions ,
              x.createdby ,
              u.Name
               FROM
       dbo.tbexpens x INNER JOIN dbo.tblUser u
	   ON x.createdby=u.UserID
	    WHERE ex_date BETWEEN @datefrom AND @dateto AND createdby=@user
   END

GO
/****** Object:  StoredProcedure [dbo].[sp_rpt_product]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_rpt_product] (
	@datefrom DATE,
	@dateto DATE
)
AS

BEGIN
    
	SELECT p.ProID,p.ProName,pd.type,pd.price,u.Name,p.createddate,
	ISNULL((SELECT SUM(d.qty) from dbo.tblInv i 
	INNER JOIN dbo.tblInv_Detail d ON i.InvID=d.InvID AND d.ProID=p.ProID AND i.InvDate BETWEEN @datefrom AND @dateto AND pd.type=d.size ),0) AS qty
	FROM dbo.tblProduct p INNER JOIN dbo.tblUser u ON p.createby=u.UserID INNER JOIN dbo.tblProductDetail pd
	ON p.ProID=pd.proid ORDER BY qty DESC
END

GO
/****** Object:  StoredProcedure [dbo].[sp_rpt_sell]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_rpt_sell](
@date DATE
)
AS
BEGIN
    
	SELECT InvID ,
           InvDate ,
           InvTime ,
           Amount ,
           discount ,
           payment ,
           createdate ,
           UserID FROM dbo.tblInv WHERE InvDate=@date
END

GO
/****** Object:  StoredProcedure [dbo].[sp_runSQL]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_runSQL]
	@sql nvarchar(max)
	as exec(@sql)

GO
/****** Object:  StoredProcedure [dbo].[sp_user]    Script Date: 6/30/2020 3:02:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_user](
@SureName NVARCHAR(100),
@UserName NVARCHAR(100),
@Password NVARCHAR(100),
@ProfileID NVARCHAR(20)
)
AS

BEGIN
DECLARE @user NVARCHAR(100)
SET @user=dbo.fn_getDigitID((SELECT ISNULL(MAX(UserID),0)+1 FROM dbo.tblUser),4);

    INSERT INTO dbo.tblUser
            ( UserID ,
              Name ,
              UserName ,
              Password ,
              position ,
              Status
            )
    VALUES  ( @user, -- UserID - nvarchar(50)
              @SureName  , -- Name - nvarchar(255)
              @UserName  , -- UserName - nvarchar(255)
              @Password  , -- Password - nvarchar(255)
              @ProfileID , -- position - nvarchar(50)
              N'1'  -- Status - nvarchar(50)
            )
    
END

GO
USE [master]
GO
ALTER DATABASE [poscoffe] SET  READ_WRITE 
GO
