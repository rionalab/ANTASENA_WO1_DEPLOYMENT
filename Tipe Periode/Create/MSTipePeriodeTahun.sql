SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MSTipePeriodeTahun]
(
	[TipePeriodeId] [int] IDENTITY(1,1) NOT NULL,
	[PeriodName] [varchar](255) NULL,
	[MulaiBatasPelaporan] [int] NULL,
	[AkhirBatasPelaporan] [int] NULL,
	[BatasKeterlambatan] [int] NULL,
	PRIMARY KEY CLUSTERED 
(
	[TipePeriodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MSTipePeriodeTahun]  WITH CHECK ADD FOREIGN KEY([PeriodName])
REFERENCES [dbo].[MSTipePeriode] ([PeriodName])
GO


