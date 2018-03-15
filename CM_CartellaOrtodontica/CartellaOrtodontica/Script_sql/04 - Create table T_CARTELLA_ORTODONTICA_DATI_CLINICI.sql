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

CREATE TABLE [dbo].[T_CARTELLA_ORTODONTICA_DATI_CLINICI]
(
	[CodDato] [varchar](20) NOT NULL,
	[Descrizione] [varchar](200) NOT NULL,
	[In_Uso] bit  NOT NULL
) ON [PRIMARY]

ALTER TABLE [dbo].[T_CARTELLA_ORTODONTICA_DATI_CLINICI] ADD 
	CONSTRAINT [PK_T_CARTELLA_ORTODONTICA_DATI_CLINICI] PRIMARY KEY CLUSTERED 
	(
		[CodDato]
	) ON [PRIMARY];

GO
GO
IF @@ERROR<>0 
Begin
	IF @@TRANCOUNT>0 ROLLBACK TRANSACTION
	INSERT INTO #ErrorLog (errno,errdescr) values(@@ERROR,'Failed to add table T_CARTELLA_ORTODONTICA_DATI_CLINICI')
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