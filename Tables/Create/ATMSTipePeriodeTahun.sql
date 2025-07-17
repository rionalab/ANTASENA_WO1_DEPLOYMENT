SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ATMSTipePeriodeTahun]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TipePeriodeId] [int] NULL,
	[PeriodName] [varchar](255) NULL,
	[MulaiBatasPelaporan] [int] NULL,
	[AkhirBatasPelaporan] [int] NULL,
	[BatasKeterlambatan] [int] NULL,
	[UpdDate] [datetime] NULL,
	[UpdUser] [varchar](255) NULL,
	[UpdFlag] [varchar](1) NULL
) ON [PRIMARY]
GO


