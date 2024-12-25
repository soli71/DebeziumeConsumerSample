
CREATE DATABASE [DebeziumDb]

Create table Student(Id int,Name nvarchar(256))

--Enable database CDC
GO
EXEC sys.sp_cdc_enable_db;
GO


--Enable table CDC

EXEC sys.sp_cdc_enable_table
    @source_schema = N'dbo',
    @source_name = N'Student',
    @role_name = NULL;
GO


insert into Student values(61,'Send3')
