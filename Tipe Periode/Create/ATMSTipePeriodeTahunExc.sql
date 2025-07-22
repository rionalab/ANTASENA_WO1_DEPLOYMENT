/****** Object:  Table [dbo].[ATMSTipePeriodeTahunExc]    Script Date: 4/24/2025 9:48:21 AM ******/
SET
	ANSI_NULLS ON
GO
SET
	QUOTED_IDENTIFIER ON
GO
	CREATE TABLE [dbo].[ATMSTipePeriodeTahunExc](
		[ID] [int] IDENTITY(1, 1) NOT NULL,
		[TipePeriodeId] [int] NULL,
		[PeriodNameOri] [varchar](255) NULL,
		[FgMove] [varchar](1) NULL,
		[TahunPeriodeData] [int] NULL,
		[MulaiBatasPelaporan] [int] NULL,
		[AkhirBatasPelaporan] [int] NULL,
		[PenambahanBulan] [int] NULL,
		[BatasKeterlambatan] [int] NULL,
		[UpdDate] [datetime] NULL,
		[UpdUser] [varchar](100) NULL,
		[UpdFlag] [varchar](1) NULL,
		[PeriodName] [varchar](255) NULL,
		PRIMARY KEY CLUSTERED ([ID] ASC) WITH (
			PAD_INDEX = OFF,
			STATISTICS_NORECOMPUTE = OFF,
			IGNORE_DUP_KEY = OFF,
			ALLOW_ROW_LOCKS = ON,
			ALLOW_PAGE_LOCKS = ON,
			OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY]
	) ON [PRIMARY]
GO