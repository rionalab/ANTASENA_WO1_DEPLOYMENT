/****** Object:  Table [dbo].[ATMSTipePeriodeSemesteran]    Script Date: 7/13/2025 7:27:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATMSTipePeriodeSemester]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TipePeriodeId] [int] NULL,
	[PeriodName] [varchar](255) NULL,
	[Semester1MulaiBatasPelaporan] [int] NULL,
	[Semester1AkhirBatasPelaporan] [int] NULL,
	[Semester2MulaiBatasPelaporan] [int] NULL,
	[Semester2AkhirBatasPelaporan] [int] NULL,
	[BatasKeterlambatan] [int] NULL,
	[UpdDate] [datetime] NULL,
	[UpdUser] [varchar](255) NULL,
	[UpdFlag] [varchar](1) NULL
) ON [PRIMARY]
GO

