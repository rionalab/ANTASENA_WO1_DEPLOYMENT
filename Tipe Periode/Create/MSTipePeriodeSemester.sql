/****** Object:  Table [dbo].[MSTipePeriodeSemesteran]    Script Date: 7/13/2025 7:26:21 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MSTipePeriodeSemester]
(
	[TipePeriodeId] [int] IDENTITY(1,1) NOT NULL,
	[PeriodName] [varchar](255) NULL,
	[Semester1MulaiBatasPelaporan] [int] NULL,
	[Semester1AkhirBatasPelaporan] [int] NULL,
	[Semester2MulaiBatasPelaporan] [int] NULL,
	[Semester2AkhirBatasPelaporan] [int] NULL,
	[BatasKeterlambatan] [int] NULL,
	PRIMARY KEY CLUSTERED 
(
	[TipePeriodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

 