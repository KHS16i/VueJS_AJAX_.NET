USE [master]
GO
/****** Object:  Database [Trazabilidad_Cod_Diverscan]    Script Date: 18/10/2023 15:50:26 ******/
CREATE DATABASE [Trazabilidad_Cod_Diverscan]

GO

CREATE TABLE [dbo].[ComponenteSistema](
	[IdComponenteSistema] [int] IDENTITY(1,1) NOT NULL,
	[IdSistema] [int] NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](100) NULL,
	[Version] [varchar](20) NOT NULL,
	[FechaLiberacion] [datetime] NULL,
	[IdDesarrollador] [int] NOT NULL,
 CONSTRAINT [PK_ComponenteSistema] PRIMARY KEY CLUSTERED 
(
	[IdComponenteSistema] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Desarrolladores]    Script Date: 18/10/2023 15:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Desarrolladores](
	[IdDesarrollador] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellidos] [varchar](50) NOT NULL,
	[Telefono] [varchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[FechaIngreso] [datetime] NULL,
	[FechaSalida] [datetime] NULL,
 CONSTRAINT [PK_Desarrolladores] PRIMARY KEY CLUSTERED 
(
	[IdDesarrollador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmpresaCliente]    Script Date: 18/10/2023 15:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpresaCliente](
	[IdEmpresa] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Telefono] [varchar](100) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Direccion] [varchar](500) NULL,
	[Cuidad] [varchar](50) NULL,
	[Pais] [varchar](50) NULL,
 CONSTRAINT [PK_EmpresaCliente] PRIMARY KEY CLUSTERED 
(
	[IdEmpresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Respaldo]    Script Date: 18/10/2023 15:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Respaldo](
	[IdRespaldo] [int] IDENTITY(1,1) NOT NULL,
	[IdSistema] [int] NOT NULL,
	[IdComponenteSistema] [int] NOT NULL,
	[IdDesarrollador] [int] NOT NULL,
	[FechaLiberacion] [datetime] NULL,
	[FechaUltimaModificacion] [datetime] NULL,
	[FechaUltimoRespaldo] [datetime] NOT NULL,
	[FechaPruebaRespaldo] [datetime] NOT NULL,
	[RespaldoGIT] [bit] NOT NULL,
	[RespaldoNube] [bit] NOT NULL,
	[RespaldoFisico] [bit] NOT NULL,
	[Comentarios] [varchar](1000) NULL,
	[Version] [varchar](20) NOT NULL,
	[IdDesarrolladorCertifica] [int] NULL,
	[RespaldoDB] [bit] NULL,
 CONSTRAINT [PK_Respaldo] PRIMARY KEY CLUSTERED 
(
	[IdRespaldo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sistema]    Script Date: 18/10/2023 15:50:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sistema](
	[IdSistema] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](100) NULL,
	[Version] [varchar](20) NOT NULL,
	[FechaLiberacion] [datetime] NULL,
	[IdEmpresa] [int] NOT NULL,
 CONSTRAINT [PK_Sistema] PRIMARY KEY CLUSTERED 
(
	[IdSistema] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ComponenteSistema]  WITH CHECK ADD  CONSTRAINT [FK_ComponenteSistema_PK_Desarrolladores] FOREIGN KEY([IdDesarrollador])
REFERENCES [dbo].[Desarrolladores] ([IdDesarrollador])
GO
ALTER TABLE [dbo].[ComponenteSistema] CHECK CONSTRAINT [FK_ComponenteSistema_PK_Desarrolladores]
GO
ALTER TABLE [dbo].[ComponenteSistema]  WITH CHECK ADD  CONSTRAINT [FK_ComponenteSistema_PK_Sistema] FOREIGN KEY([IdSistema])
REFERENCES [dbo].[Sistema] ([IdSistema])
GO
ALTER TABLE [dbo].[ComponenteSistema] CHECK CONSTRAINT [FK_ComponenteSistema_PK_Sistema]
GO
ALTER TABLE [dbo].[Respaldo]  WITH CHECK ADD  CONSTRAINT [FK_Respaldo_PK_ComponenteSistema] FOREIGN KEY([IdComponenteSistema])
REFERENCES [dbo].[ComponenteSistema] ([IdComponenteSistema])
GO
ALTER TABLE [dbo].[Respaldo] CHECK CONSTRAINT [FK_Respaldo_PK_ComponenteSistema]
GO
ALTER TABLE [dbo].[Respaldo]  WITH CHECK ADD  CONSTRAINT [FK_Respaldo_PK_Desarrolladores] FOREIGN KEY([IdDesarrollador])
REFERENCES [dbo].[Desarrolladores] ([IdDesarrollador])
GO
ALTER TABLE [dbo].[Respaldo] CHECK CONSTRAINT [FK_Respaldo_PK_Desarrolladores]
GO
ALTER TABLE [dbo].[Respaldo]  WITH CHECK ADD  CONSTRAINT [FK_Respaldo_PK_Sistema] FOREIGN KEY([IdSistema])
REFERENCES [dbo].[Sistema] ([IdSistema])
GO
ALTER TABLE [dbo].[Respaldo] CHECK CONSTRAINT [FK_Respaldo_PK_Sistema]
GO
ALTER TABLE [dbo].[Sistema]  WITH CHECK ADD  CONSTRAINT [FK_Sistema_PK_EmpresaCliente] FOREIGN KEY([IdEmpresa])
REFERENCES [dbo].[EmpresaCliente] ([IdEmpresa])
GO
ALTER TABLE [dbo].[Sistema] CHECK CONSTRAINT [FK_Sistema_PK_EmpresaCliente]
GO
USE [master]
GO
ALTER DATABASE [Trazabilidad_Cod_Diverscan] SET  READ_WRITE 
GO
