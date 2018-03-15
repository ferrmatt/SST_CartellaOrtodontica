﻿BEGIN TRANSACTION
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	SET QUOTED_IDENTIFIER ON
	SET ANSI_NULLS ON
	SET ANSI_PADDING ON
	SET ANSI_WARNINGS ON
	SET ARITHABORT ON
	SET NUMERIC_ROUNDABORT OFF
	SET CONCAT_NULL_YIELDS_NULL ON
	SET XACT_ABORT ON
COMMIT TRANSACTION
GO

IF EXISTS (select * from tempdb..sysobjects where id = OBJECT_ID('tempdb..#ErrorLog')) 
	DROP TABLE #ErrorLog 
CREATE TABLE #ErrorLog 
( pkid [int] IDENTITY(1,1) NOT NULL, errno [int] NOT NULL, errdescr [varchar](100) NULL )
GO

IF @@TRANCOUNT=0 BEGIN TRANSACTION
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

IF @@TRANCOUNT=0 BEGIN TRANSACTION
GO

CREATE TABLE [dbo].[T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI]
(
	[CodRaggruppamento] int NOT NULL,
	[In_Uso] bit NOT NULL
) ON [PRIMARY]

-- -Primary Key constraint - On Left Object
ALTER TABLE [dbo].[T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI] ADD 
	CONSTRAINT [PK_T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI] PRIMARY KEY CLUSTERED 
	(
		[CodRaggruppamento]
	) ON [PRIMARY];
GO

GO
IF @@ERROR<>0 
Begin
	IF @@TRANCOUNT>0 ROLLBACK TRANSACTION
	INSERT INTO #ErrorLog (errno,errdescr) values(@@ERROR,'Failed to add table T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI')
END
GO

IF @@TRANCOUNT=0 BEGIN TRANSACTION
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI] WITH NOCHECK ADD 
	CONSTRAINT [FK_T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI_T_LISTINO_RAGGRUPPAMENTI] FOREIGN KEY 
	(
		[CodRaggruppamento]
	)REFERENCES [dbo].[T_LISTINO_RAGGRUPPAMENTI](
		[CodRaggruppamento]
	)ON UPDATE NO ACTION ON DELETE NO ACTION NOT FOR REPLICATION 
ALTER TABLE [dbo].[T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI] NOCHECK CONSTRAINT [FK_T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI_T_LISTINO_RAGGRUPPAMENTI]
GO
IF @@ERROR<>0 
Begin
	IF @@TRANCOUNT>0 ROLLBACK TRANSACTION
	INSERT INTO #ErrorLog (errno,errdescr) values(@@ERROR,'Failed to add constraint FK_T_CARTELLA_ORTODONTICA_RAGGRUPPAMENTI_T_LISTINO_RAGGRUPPAMENTI')
END
GO

-- log --

IF EXISTS (Select * from #ErrorLog)
BEGIN
	IF @@TRANCOUNT>0 ROLLBACK TRANSACTION
END
ELSE
BEGIN
	IF @@TRANCOUNT>0 COMMIT TRANSACTION
END
IF EXISTS (Select * from #ErrorLog)
BEGIN
	Print 'Database synchronization script failed'
	GOTO QuitWithErrors
END
ELSE
BEGIN
	Print 'Database synchronization completed successfully'
END



QuitWithErrors: