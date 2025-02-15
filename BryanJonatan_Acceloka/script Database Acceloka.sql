USE [master]
GO
/****** Object:  Database [Acceloka]    Script Date: 13/02/2025 09.13.43 ******/
CREATE DATABASE [Acceloka]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Acceloka', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Acceloka.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Acceloka_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Acceloka_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Acceloka] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Acceloka].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Acceloka] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Acceloka] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Acceloka] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Acceloka] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Acceloka] SET ARITHABORT OFF 
GO
ALTER DATABASE [Acceloka] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Acceloka] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Acceloka] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Acceloka] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Acceloka] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Acceloka] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Acceloka] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Acceloka] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Acceloka] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Acceloka] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Acceloka] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Acceloka] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Acceloka] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Acceloka] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Acceloka] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Acceloka] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Acceloka] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Acceloka] SET RECOVERY FULL 
GO
ALTER DATABASE [Acceloka] SET  MULTI_USER 
GO
ALTER DATABASE [Acceloka] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Acceloka] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Acceloka] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Acceloka] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Acceloka] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Acceloka] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Acceloka', N'ON'
GO
ALTER DATABASE [Acceloka] SET QUERY_STORE = ON
GO
ALTER DATABASE [Acceloka] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Acceloka]
GO
/****** Object:  Table [dbo].[BookedTickets]    Script Date: 13/02/2025 09.13.43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookedTickets](
	[BookingId] [varchar](255) NOT NULL,
	[TicketCode] [varchar](255) NULL,
	[Quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BookingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tickets]    Script Date: 13/02/2025 09.13.43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tickets](
	[TicketCode] [varchar](255) NOT NULL,
	[TicketName] [varchar](255) NOT NULL,
	[CategoryName] [varchar](255) NOT NULL,
	[EventDateMinimum] [datetime] NOT NULL,
	[EventDateMaximum] [datetime] NOT NULL,
	[Quota] [int] NOT NULL,
	[Price] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TicketCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BookedTickets] ADD  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[BookedTickets]  WITH CHECK ADD FOREIGN KEY([TicketCode])
REFERENCES [dbo].[Tickets] ([TicketCode])
GO
USE [master]
GO
ALTER DATABASE [Acceloka] SET  READ_WRITE 
GO
