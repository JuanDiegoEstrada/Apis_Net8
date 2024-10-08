USE [master]
GO
/****** Object:  Database [LocalExcercice]    Script Date: 11/08/2024 23:45:01 ******/
CREATE DATABASE [LocalExcercice]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LocalExcercice', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LocalExcercice.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LocalExcercice_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LocalExcercice_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LocalExcercice] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LocalExcercice].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LocalExcercice] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LocalExcercice] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LocalExcercice] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LocalExcercice] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LocalExcercice] SET ARITHABORT OFF 
GO
ALTER DATABASE [LocalExcercice] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LocalExcercice] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LocalExcercice] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LocalExcercice] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LocalExcercice] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LocalExcercice] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LocalExcercice] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LocalExcercice] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LocalExcercice] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LocalExcercice] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LocalExcercice] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LocalExcercice] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LocalExcercice] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LocalExcercice] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LocalExcercice] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LocalExcercice] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LocalExcercice] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LocalExcercice] SET RECOVERY FULL 
GO
ALTER DATABASE [LocalExcercice] SET  MULTI_USER 
GO
ALTER DATABASE [LocalExcercice] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LocalExcercice] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LocalExcercice] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LocalExcercice] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LocalExcercice] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LocalExcercice] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LocalExcercice', N'ON'
GO
ALTER DATABASE [LocalExcercice] SET QUERY_STORE = ON
GO
ALTER DATABASE [LocalExcercice] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [LocalExcercice]
GO
/****** Object:  Table [dbo].[products]    Script Date: 11/08/2024 23:45:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[products](
	[idProduct] [uniqueidentifier] NULL,
	[nameProduct] [varchar](100) NULL,
	[descriptionProduct] [varchar](100) NULL,
	[price] [float] NULL,
	[dateRegistered] [datetime] NULL,
	[SKU] [varchar](20) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[userSessions]    Script Date: 11/08/2024 23:45:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userSessions](
	[id] [uniqueidentifier] NULL,
	[userName] [varchar](100) NULL,
	[userEmail] [varchar](100) NULL,
	[userPassword] [varbinary](500) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AddProduct]    Script Date: 11/08/2024 23:45:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[AddProduct](
	@Name NVARCHAR(100),
    @Description NVARCHAR(255),
    @Price FLOAT,
    @sku NVARCHAR(100)
) as
begin
	insert into dbo.products(idProduct, nameProduct, descriptionProduct, price, dateRegistered, SKU)
	values(NEWID(), @Name, @Description, @Price, GETDATE(), @sku)
end
GO
/****** Object:  StoredProcedure [dbo].[AddUser]    Script Date: 11/08/2024 23:45:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[AddUser](
@Name varchar(100),
@Email varchar(100),
@Password varchar(500)
) as

if (select COUNT(*) from userSessions where userEmail = @Email) > 0 
begin
	raiserror('user already exists',16,1)
	return -1
end
else
begin

declare @passEncrypted varBinary(500)
set @passEncrypted = ENCRYPTBYPASSPHRASE('Key', @Password)

insert into dbo.userSessions(id, userName, userEmail, userPassword)
values (NEWID(), @Name, @Email, @passEncrypted)

select ('inser Ok')

end
GO
/****** Object:  StoredProcedure [dbo].[DeleteProduc]    Script Date: 11/08/2024 23:45:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[DeleteProduc](
	@sku varchar(100)
) as
begin
	delete products where SKU = @sku
end
GO
/****** Object:  StoredProcedure [dbo].[GetProduct]    Script Date: 11/08/2024 23:45:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[GetProduct](
    @sku NVARCHAR(100) = null
) as
begin
	if @sku is null
	begin
		select * from products
	end
	else
	begin
		select * from products where SKU = @sku
	end
end
GO
/****** Object:  StoredProcedure [dbo].[LogInUser]    Script Date: 11/08/2024 23:45:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LogInUser](
    @Email VARCHAR(100),
    @Password VARCHAR(500)
) AS
BEGIN
    IF (SELECT COUNT(*) FROM userSessions WHERE userEmail = @Email) = 0 
    BEGIN
        RAISERROR('username doesn´t exist', 16, 1)
        RETURN -1
    END
    ELSE
    BEGIN
        DECLARE @passEncrypted VARBINARY(500)
        DECLARE @passDecrypted VARCHAR(500)

        -- Convierte userPassword a VARBINARY antes de asignarlo
        SELECT @passEncrypted = userPassword FROM userSessions WHERE userEmail = @Email

        SET @passDecrypted = DECRYPTBYPASSPHRASE('Key', @passEncrypted)

        IF (@passDecrypted = @Password)
        BEGIN
            SELECT id, userName, userEmail FROM userSessions WHERE userEmail = @Email
        END
        ELSE
        BEGIN
            RAISERROR('Invalid password', 16, 1)
            RETURN -1
        END
    END
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateProduc]    Script Date: 11/08/2024 23:45:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[UpdateProduc](
	@Name NVARCHAR(100),
    @Description NVARCHAR(255),
    @Price FLOAT,
    @sku NVARCHAR(100)
) as
begin
	update products set nameProduct = @Name, descriptionProduct = @Description, price = @Price where SKU = @sku
end
GO
USE [master]
GO
ALTER DATABASE [LocalExcercice] SET  READ_WRITE 
GO
