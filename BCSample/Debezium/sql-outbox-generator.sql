CREATE DATABASE OutboxCDC;
GO

USE OutboxCDC;

CREATE TABLE [dbo].[Outbox](
	[Id] [uniqueidentifier] NOT NULL,
	[AggregateId] [uniqueidentifier] NOT NULL,
	[Context] [nvarchar](50) NOT NULL,
	[Payload] [varbinary](max) NOT NULL,
	[Occured] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Outbox] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Outbox] ADD  CONSTRAINT [DF__Outbox__Id__5070F446]  DEFAULT (newsequentialid()) FOR [Id]
GO

ALTER TABLE [dbo].[Outbox] ADD  CONSTRAINT [DF_Outbox_Occured]  DEFAULT (sysdatetime()) FOR [Occured]
GO



EXEC sys.sp_cdc_enable_db;

EXEC sys.sp_cdc_enable_table 
    @source_schema = 'dbo', 
    @source_name = 'Outbox', 
    -- @captured_column_list = N'id,message',
    @role_name = NULL, 
    @supports_net_changes = 0;
GO
