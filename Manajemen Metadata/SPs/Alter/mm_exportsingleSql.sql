SET
	ANSI_NULLS ON
GO
SET
	QUOTED_IDENTIFIER ON
GO
	ALTER procedure [dbo].[mm_exportsingleSql] --'tkk01','tes'
	@versioncode varchar(max),
	@startDate DATE,
	@endDate DATE as
set
	@versioncode = dbo.fn_StripCharacters(@versioncode, '^a-zA-Z0-9 [-]_@.!#$&/\[]') declare @idinformation varchar(max),
	@return varchar(max)
set
	@idinformation = (
		select
			top 1 idinformation
		from
			[metadata_mgmt]
		where
			[versioncode] = @versioncode
	) --select compositekey_yn,* FROM [dbo].[metadata_mgmt] WHERE [versioncode] = 'aak01'
	declare @icount int =(
		select
			count(1)
		FROM
			[dbo].[metadata_mgmt]
		WHERE
			[versioncode] = @versioncode
	) --SELECT 
	--		[idelement],
	--		[idtoindex],
	--		[nillable_yn],
	--		[idtodata],
	--		[length]
	--	  FROM [dbo].[metadata_mgmt] 	
	--	  WHERE [versioncode] = @versioncode
set
	@return = '';

set
	@return = @return + 'CREATE TABLE [dbo].' + @idinformation + '( '
select
	@return = @return + idelement + ' ' +case
		when idtodata = 'string' then 'varchar(' + convert(varchar, [length]) + '), '
		when idtodata = 'integer' then 'int,'
		when idtodata = 'date' then 'date,'
		when idtodata = 'time' then 'time,'
		when idtodata = 'datetime' then 'datetime,'
		when idtodata = 'decimal' then 'decimal(' + convert(varchar, [length]) + ',' + convert(varchar, [fractiondigit]) + '),'
		else idtodata + '(' + convert(varchar, [length]) + '), '
	end + CHAR(13)
FROM
	[dbo].[metadata_mgmt]
WHERE
	[versioncode] = @versioncode
	and idtodata is not null
ORDER BY
	[sequence] asc
set
	@return = @return + 'PRIMARY KEY(';

select
	@return = @return + case
		when compositekey_yn = 'y' then [idelement] + ','
	end
FROM
	[dbo].[metadata_mgmt]
WHERE
	[versioncode] = @versioncode
	and compositekey_yn = 'y'
	and (
		(
			@startDate is null
			and datestart is null
		)
		or (
			@startDate is not null
			and datestart >= @startDate
		)
	)
	and (
		(
			@endDate is null
			and dateend is null
		)
		or (
			@endDate is not null
			and dateend <= @endDate
		)
	)
ORDER BY
	[sequence] asc
set
	@return = LEFT(@return, DATALENGTH(@return) -1) + ') ';

--declare @i int=0;
--while @i<@icount
--begin
--	set @return=@return+'[Id] [int] IDENTITY(1,1) NOT NULL, '
--end
set
	@return = @return + ') ON [PRIMARY]'
select
	@return dtsql