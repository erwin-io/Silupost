USE [CrimeReportAppDB]
GO
/****** Object:  UserDefinedFunction [dbo].[LPAD]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		
-- Create date: Sept 25, 2020
-- Description:	FOR GENERATING SEQUENCE ID WITH PADDING LENGTH
-- =============================================
CREATE FUNCTION [dbo].[LPAD]
(
    @string VARCHAR(MAX), -- Initial string
    @length INT,          -- Size of final string
    @pad CHAR             -- Pad character
)
RETURNS VARCHAR(MAX)
AS
BEGIN
    RETURN REPLICATE(@pad, @length - LEN(@string)) + @string;
END

GO
/****** Object:  Table [dbo].[EntityContactType]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityContactType](
	[EntityContactTypeId] [bigint] NOT NULL,
	[EntityContactTypeName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_EntityContactType] PRIMARY KEY CLUSTERED 
(
	[EntityContactTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EntityGender]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityGender](
	[GenderId] [bigint] NOT NULL,
	[GenderName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_EntityGender] PRIMARY KEY CLUSTERED 
(
	[GenderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EntityStatus]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityStatus](
	[EntityStatusId] [bigint] NOT NULL,
	[EntityStatusName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_EntityStatus] PRIMARY KEY CLUSTERED 
(
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LegalEntity]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LegalEntity](
	[LegalEntityId] [nvarchar](250) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[GenderId] [nvarchar](100) NOT NULL,
	[BirthDate] [date] NOT NULL,
	[Age]  AS ((CONVERT([int],CONVERT([char](8),getdate(),(112)))-CONVERT([char](8),[BirthDate],(112)))/(10000)),
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [LegalEntityId] PRIMARY KEY CLUSTERED 
(
	[LegalEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LegalGeoAddress]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LegalGeoAddress](
	[LegalGeoAddressId] [nvarchar](100) NOT NULL,
	[LegalEntityId] [nvarchar](100) NOT NULL,
	[Latitude] [decimal](18, 8) NOT NULL,
	[Longitude] [decimal](18, 8) NOT NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_LegalGeoAddress] PRIMARY KEY CLUSTERED 
(
	[LegalGeoAddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sequence]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sequence](
	[SequenceId] [bigint] NOT NULL,
	[TableName] [nvarchar](100) NOT NULL,
	[Prefix] [nvarchar](10) NOT NULL,
	[Length] [int] NOT NULL,
	[LastNumber] [bigint] NOT NULL,
 CONSTRAINT [PK_Sequence] PRIMARY KEY CLUSTERED 
(
	[SequenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemConfig]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfig](
	[SystemConfigId] [bigint] NOT NULL,
	[ConfigName] [nvarchar](100) NOT NULL,
	[ConfigGroup] [nvarchar](100) NOT NULL,
	[ConfigKey] [nvarchar](100) NOT NULL,
	[ConfigValue] [varchar](max) NOT NULL,
	[SystemConfigTypeId] [bigint] NOT NULL,
	[IsUserConfigurable] [bit] NOT NULL,
 CONSTRAINT [PK_SystemConfig] PRIMARY KEY CLUSTERED 
(
	[SystemConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemConfigType]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfigType](
	[SystemConfigTypeId] [bigint] NOT NULL,
	[ValueType] [nvarchar](100) NULL,
 CONSTRAINT [PK_SystemConfigType] PRIMARY KEY CLUSTERED 
(
	[SystemConfigTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemRole]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemRole](
	[SystemRoleId] [nvarchar](100) NOT NULL,
	[RoleName] [nvarchar](250) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_SystemRole] PRIMARY KEY CLUSTERED 
(
	[SystemRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemUser]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUser](
	[SystemUserId] [nvarchar](100) NOT NULL,
	[SystemUserTypeId] [bigint] NOT NULL,
	[LegalEntityId] [nvarchar](100) NOT NULL,
	[ProfilePictureFile] [nvarchar](100) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[LocationId] [bigint] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[SystemUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemUserConfig]    Script Date: 1/10/2021 10:03:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUserConfig](
	[SystemUserConfigId] [nvarchar](100) NOT NULL,
	[SystemUsersId] [nvarchar](100) NOT NULL,
	[IsNextReportPublic] [bit] NOT NULL,
	[IsAnonymousNextReport] [bit] NOT NULL,
	[AllowReviewActionNextPost] [bit] NOT NULL,
	[AllowReviewCommentNextPost] [bit] NOT NULL,
	[IsAllReportPublic] [bit] NOT NULL,
	[IsAnonymousAllReport] [bit] NOT NULL,
	[AllowReviewActionAllReport] [bit] NOT NULL,
	[AllowReviewCommentAllReport] [bit] NOT NULL,
 CONSTRAINT [PK_SystemUserConfig] PRIMARY KEY CLUSTERED 
(
	[SystemUserConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemUserContact]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUserContact](
	[SystemUserContactId] [nvarchar](100) NOT NULL,
	[SystemUsersId] [nvarchar](100) NOT NULL,
	[ContactTypeId] [bigint] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_SystemUserContact] PRIMARY KEY CLUSTERED 
(
	[SystemUserContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemUserRoles]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUserRoles](
	[SystemUserRoleId] [nvarchar](100) NOT NULL,
	[SystemUserId] [nvarchar](100) NOT NULL,
	[SystemRoleId] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_SystemUserRoles] PRIMARY KEY CLUSTERED 
(
	[SystemUserRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemUserType]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUserType](
	[SystemUserTypeId] [bigint] NOT NULL,
	[SystemUserTypeName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_SystemUserType] PRIMARY KEY CLUSTERED 
(
	[SystemUserTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[EntityContactType] ([EntityContactTypeId], [EntityContactTypeName]) VALUES (1, N'Mobile')
GO
INSERT [dbo].[EntityContactType] ([EntityContactTypeId], [EntityContactTypeName]) VALUES (2, N'Telephone')
GO
INSERT [dbo].[EntityContactType] ([EntityContactTypeId], [EntityContactTypeName]) VALUES (3, N'Email')
GO
INSERT [dbo].[EntityGender] ([GenderId], [GenderName]) VALUES (1, N'Male')
GO
INSERT [dbo].[EntityGender] ([GenderId], [GenderName]) VALUES (2, N'Female')
GO
INSERT [dbo].[EntityStatus] ([EntityStatusId], [EntityStatusName]) VALUES (1, N'Active')
GO
INSERT [dbo].[EntityStatus] ([EntityStatusId], [EntityStatusName]) VALUES (2, N'Deleted')
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (1, N'SystemUser', N'SU-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (2, N'SystemUserConfig', N'SUC-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (3, N'SystemUserContact', N'SUCON-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (4, N'SystemUserRoles', N'SUR-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (5, N'SystemRole', N'SR-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (6, N'LegalEntity', N'LE-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (7, N'LegalGeoAddress', N'LGA-', 10, 0)
GO
INSERT [dbo].[SystemConfigType] ([SystemConfigTypeId], [ValueType]) VALUES (1, N'BOOLEAN')
GO
INSERT [dbo].[SystemConfigType] ([SystemConfigTypeId], [ValueType]) VALUES (2, N'TEXT')
GO
INSERT [dbo].[SystemUserType] ([SystemUserTypeId], [SystemUserTypeName]) VALUES (1, N'WebAppAdmin')
GO
INSERT [dbo].[SystemUserType] ([SystemUserTypeId], [SystemUserTypeName]) VALUES (2, N'MobileAppUser')
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [U_Sequence]    Script Date: 1/10/2021 10:03:03 PM ******/
ALTER TABLE [dbo].[Sequence] ADD  CONSTRAINT [U_Sequence] UNIQUE NONCLUSTERED 
(
	[TableName] ASC,
	[Prefix] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_SystemRole]    Script Date: 1/10/2021 10:03:03 PM ******/
ALTER TABLE [dbo].[SystemRole] ADD  CONSTRAINT [UK_SystemRole] UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_SystemUser]    Script Date: 1/10/2021 10:03:03 PM ******/
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [UK_SystemUser] UNIQUE NONCLUSTERED 
(
	[UserName] ASC,
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LegalEntity] ADD  CONSTRAINT [DF_LegalEntity_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[LegalEntity] ADD  CONSTRAINT [DF_LegalEntity_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[LegalGeoAddress] ADD  CONSTRAINT [DF_LegalGeoAddress_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[Sequence] ADD  CONSTRAINT [DF_Sequence_SequenceLength]  DEFAULT ((0)) FOR [Length]
GO
ALTER TABLE [dbo].[Sequence] ADD  CONSTRAINT [DF_Sequence_LastNumber]  DEFAULT ((0)) FOR [LastNumber]
GO
ALTER TABLE [dbo].[SystemConfig] ADD  CONSTRAINT [DF_SystemConfig_IsUserConfigurable]  DEFAULT ((1)) FOR [IsUserConfigurable]
GO
ALTER TABLE [dbo].[SystemRole] ADD  CONSTRAINT [DF_Role_IsPostedToMain]  DEFAULT ((0)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[SystemRole] ADD  CONSTRAINT [DF_Role_IsFromMain]  DEFAULT ((0)) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SystemRole] ADD  CONSTRAINT [DF_SystemRole_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[SystemUserConfig] ADD  CONSTRAINT [DF_SystemUserConfig_IsNextReportPublic]  DEFAULT ((1)) FOR [IsNextReportPublic]
GO
ALTER TABLE [dbo].[SystemUserConfig] ADD  CONSTRAINT [DF_SystemUserConfig_IsAnonymousNextReport]  DEFAULT ((0)) FOR [IsAnonymousNextReport]
GO
ALTER TABLE [dbo].[SystemUserConfig] ADD  CONSTRAINT [DF_SystemUserConfig_AllowReviewActionNextPost]  DEFAULT ((1)) FOR [AllowReviewActionNextPost]
GO
ALTER TABLE [dbo].[SystemUserConfig] ADD  CONSTRAINT [DF_SystemUserConfig_AllowReviewCommentNextPost]  DEFAULT ((1)) FOR [AllowReviewCommentNextPost]
GO
ALTER TABLE [dbo].[SystemUserConfig] ADD  CONSTRAINT [DF_SystemUserConfig_IsAllReportPublic]  DEFAULT ((1)) FOR [IsAllReportPublic]
GO
ALTER TABLE [dbo].[SystemUserConfig] ADD  CONSTRAINT [DF_SystemUserConfig_IsAnonymousAllReport]  DEFAULT ((0)) FOR [IsAnonymousAllReport]
GO
ALTER TABLE [dbo].[SystemUserConfig] ADD  CONSTRAINT [DF_SystemUserConfig_AllowReviewActionAllReport]  DEFAULT ((1)) FOR [AllowReviewActionAllReport]
GO
ALTER TABLE [dbo].[SystemUserConfig] ADD  CONSTRAINT [DF_SystemUserConfig_AllowReviewCommentAllReport]  DEFAULT ((1)) FOR [AllowReviewCommentAllReport]
GO
ALTER TABLE [dbo].[SystemUserContact] ADD  CONSTRAINT [DF_SystemUserContact_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[SystemUserRoles] ADD  CONSTRAINT [DF_SystemUserRoles_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SystemUserRoles] ADD  CONSTRAINT [DF_SystemUserRoles_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
/****** Object:  StoredProcedure [dbo].[usp_entity_information_add]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_entity_information_add]
	@FirstName			NVARCHAR(100),
	@LastName			NVARCHAR(100),
	@MiddleName			NVARCHAR(100),
	@GenderId			NVARCHAR(100),
	@BirthDate			DATE,
	@CivilStatusTypeId	NVARCHAR(100),
	@LocationId			BIGINT,
	@CreatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @LegalEntityId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'Entity_Information', @Id = @LegalEntityId OUTPUT

		INSERT INTO [dbo].[Entity_Information](
			[LegalEntityId], 
			[FirstName], 
			[LastName],
			[MiddleName],
			[GenderId],
			[BirthDate],
			[CivilStatusTypeId],
			[LocationId],
			[IsActive],
			[CreatedBy],
			[CreatedAt]
		)
		VALUES(
			@LegalEntityId,
			@FirstName,
			@LastName,
			@MiddleName,
			@GenderId,
			@BirthDate,
			@CivilStatusTypeId,
			@LocationId,
			1,
			@CreatedBy,
			GETDATE()
		);

		SELECT @LegalEntityId;
        
    END TRY
    BEGIN CATCH

        SELECT
			'Error - ' + ERROR_MESSAGE()   AS ErrorMessage,
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_entity_information_update]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_entity_information_update]
	@LegalEntityId		NVARCHAR(100),
	@FirstName			NVARCHAR(100),
	@LastName			NVARCHAR(100),
	@MiddleName			NVARCHAR(100),
	@GenderId			NVARCHAR(100),
	@BirthDate			DATE,
	@CivilStatusTypeId	NVARCHAR(100),
	@UpdatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		UPDATE [dbo].[Entity_Information] SET
			[FirstName] = @FirstName, 
			[LastName] = @LastName,
			[MiddleName] = @MiddleName,
			[GenderId] = @GenderId,
			[BirthDate] = @BirthDate,
			[CivilStatusTypeId] = @CivilStatusTypeId,
			[UpdatedBy] = @UpdatedBy,
			[UpdatedAt] = GETDATE()
		WHERE [LegalEntityId] = @LegalEntityId;
		
		SELECT @@ROWCOUNT;
        
    END TRY
    BEGIN CATCH

        SELECT
			'Error - ' + ERROR_MESSAGE()   AS ErrorMessage,
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_location_add]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_location_add]
	@LocationId			BIGINT,
	@Name				NVARCHAR(100),
	@CreatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		INSERT INTO [dbo].[Location](
			[LocationId],
			[Name],
			[IsActive],
			[CreatedBy],
			[CreatedAt]
		)
		VALUES(
			@LocationId,
			@Name,
			1,
			@CreatedBy,
			GETDATE()
		);

		SELECT @LocationId;
        
    END TRY
    BEGIN CATCH

        SELECT
			'Error - ' + ERROR_MESSAGE()   AS ErrorMessage,
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_Reset]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_Reset]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
		SET NOCOUNT ON;
		
		DELETE FROM [dbo].[SystemUserRoles];
		DELETE FROM [dbo].[SystemUserContact];
		DELETE FROM [dbo].[SystemUserConfig];
		DELETE FROM [dbo].[SystemRole];
		DELETE FROM [dbo].[SystemUser];
		DELETE FROM [dbo].[LegalGeoAddress];
		DELETE FROM [dbo].[LegalEntity];
		
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemUserRoles';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemUserContact';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemUserConfig';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemRole';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemUser';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'LegalGeoAddress';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'LegalEntity';
        
    END TRY
    BEGIN CATCH

        SELECT
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_sequence_getNextCode]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_sequence_getNextCode]
	@TableName	NVARCHAR(250),
    @Id NVARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @LastNumber INT;
		DECLARE @SequenceLength BIGINT;
		DECLARE @GeneratedId NVARCHAR(100);
		DECLARE @Prefix NVARCHAR(100);
		SELECT @LastNumber = [LastNumber] + 1,@SequenceLength=[Length],@Prefix=[Prefix] FROM [Sequence] WHERE TableName = @TableName;
		SELECT @GeneratedId = CONCAT(@Prefix, [dbo].LPAD(@LastNumber, @SequenceLength, 0));
		SELECT @Id = IIF(datalength(@GeneratedId)=0,NULL,@GeneratedId);

		UPDATE [dbo].[Sequence] SET [LastNumber] = @LastNumber WHERE [TableName] = @TableName;
		
		RETURN 0
        
    END TRY
    BEGIN CATCH

        SELECT
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemrole_add]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemrole_add]
	@Name				NVARCHAR(100),
	@CreatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @SystemRoleId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'SystemRole', @Id = @SystemRoleId OUTPUT

		INSERT INTO [dbo].[SystemRole](
			[SystemRoleId],
			[RoleName],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@SystemRoleId,
			@Name,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @SystemRoleId;
        
    END TRY
    BEGIN CATCH

        SELECT
			'Error - ' + ERROR_MESSAGE()   AS ErrorMessage,
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemrole_delete]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemrole_delete]
	@SystemRoleId		NVARCHAR(100),
	@UpdatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[SystemRole] WHERE [SystemRoleId] = @SystemRoleId AND IsActive = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[SystemRole]
			SET 
			[Name] = @SystemRoleId + ' - ' + [Name] + '(DELETED - ' + CONVERT(VARCHAR(50),GETDATE())+ ')',
			[IsActive] = 0,
			[UpdatedBy] = @UpdatedBy,
			[UpdatedAt] = GETDATE()
			WHERE [SystemRoleId] = @SystemRoleId;

			SELECT @@ROWCOUNT;
		END
		ELSE
		BEGIN
			SELECT 1;
		END
        
    END TRY
    BEGIN CATCH

        SELECT
			'Error - ' + ERROR_MESSAGE()   AS ErrorMessage,
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemrole_getByID]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemrole_getByID]
	@SystemRoleId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @LocationId		BIGINT;
		DECLARE @CreatedBy		NVARCHAR(100);
		DECLARE @UpdatedBy		NVARCHAR(100);
		
		SELECT @LocationId = sr.[LocationId], @CreatedBy = sr.CreatedBy, @UpdatedBy = sr.UpdatedBy
		FROM [dbo].[SystemRole] sr
		WHERE sr.[SystemRoleId] = @SystemRoleId
		AND [sr].IsActive = 1;

		SELECT 
		sr.[SystemRoleId],
		sr.[Name],
		sr.[CreatedAt],
		sr.[UpdatedAt]
		FROM [dbo].[SystemRole] AS sr
		WHERE sr.[SystemRoleId] = @SystemRoleId
		AND [sr].IsActive = 1;

		SELECT *
		FROM [dbo].[Location] AS l
		WHERE l.[LocationId] = @LocationId;

		SELECT  su.SystemUserId,ISNULL(ei.FirstName, '') + ' ' + ISNULL(ei.MiddleName, '') + ' ' + ISNULL(ei.LastName, '') AS FullName
		FROM [dbo].[SystemUser] AS su
		LEFT JOIN [dbo].[Entity_Information] ei ON su.LegalEntityId = ei.LegalEntityId
		WHERE su.[SystemUserId] = @CreatedBy;
		
		SELECT  su.SystemUserId,ISNULL(ei.FirstName, '') + ' ' + ISNULL(ei.MiddleName, '') + ' ' + ISNULL(ei.LastName, '') AS FullName
		FROM [dbo].[SystemUser] AS su
		LEFT JOIN [dbo].[Entity_Information] ei ON su.LegalEntityId = ei.LegalEntityId
		WHERE su.[SystemUserId] = @UpdatedBy;

		RETURN 0
        
    END TRY
    BEGIN CATCH

        SELECT
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemrole_getPaged]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- Description:	filter contract list by location
-- =============================================
CREATE PROCEDURE [dbo].[usp_systemrole_getPaged]
	@SystemRoleId NVARCHAR(100) = '',
	@Name NVARCHAR(100) = '',
	@CreatedAt NVARCHAR(100) = '',
	@UpdatedAt NVARCHAR(100) = '',
	@PageNo BIGINT = 1,
	@PageSize BIGINT = 10,
	@LocationId NVARCHAR(100) = 0
AS
BEGIN
	SET NOCOUNT ON;

	
		WITH DATA_CTE
		AS
		(
			Select tableSource.*, ROW_NUMBER() OVER(ORDER BY SystemRoleId ASC) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 sr.SystemRoleId,
			 MAX(sr.[Name])[Name],
			 MAX(sr.[CreatedAt])[CreatedAt],
			 MAX(sr.[UpdatedAt])[UpdatedAt],
			 MAX(l.[LocationId])[LocationId],
			 MAX(l.[Name])[LocationName],
			 MAX(cb.[SystemUserId])[CreatedBy],
			 MAX(ISNULL(cb_ei.FirstName, '') + ' ' + ISNULL(cb_ei.MiddleName, '') + ' ' + ISNULL(cb_ei.LastName, ''))[CreatedByFullName],
			 MAX(ub.[SystemUserId])[UpdatedBy],
			 MAX(ISNULL(cb_ei.FirstName, '') + ' ' + ISNULL(cb_ei.MiddleName, '') + ' ' + ISNULL(cb_ei.LastName, ''))[UpdatedByFullName]
			FROM [dbo].[SystemRole] AS sr
			LEFT JOIN [dbo].[Location] l ON sr.[LocationId] = l.[LocationId]
			LEFT JOIN [dbo].[SystemUser] cb ON sr.[CreatedBy] = cb.SystemUserId
			LEFT JOIN [dbo].[Entity_Information] cb_ei ON cb.LegalEntityId = cb_ei.LegalEntityId
			LEFT JOIN [dbo].[SystemUser] ub ON sr.[UpdatedBy] = ub.SystemUserId
			LEFT JOIN [dbo].[Entity_Information] ub_ei ON ub.LegalEntityId = ub_ei.LegalEntityId
			WHERE sr.IsActive = 1
			AND sr.LocationId = @LocationId
			AND (sr.SystemRoleId like '%' + @SystemRoleId + '%' OR @SystemRoleId= '' OR @SystemRoleId IS NULL)
			AND (sr.CreatedAt like '%' + @CreatedAt + '%' OR @CreatedAt= '' OR @CreatedAt IS NULL)
			AND (ISNULL(sr.UpdatedAt, '') like '%' + @UpdatedAt + '%' OR @UpdatedAt= '' OR @UpdatedAt IS NULL)
			GROUP BY sr.SystemRoleId
			) tableSource
		)
		SELECT 
		src.[SystemRoleId],
		src.[Name],
		src.[CreatedAt],
		src.[UpdatedAt],
		src.[LocationId],
		src.[LocationName] AS [Name],
		src.[CreatedBy] AS [SystemUserId],
		src.[CreatedByFullName] AS [FullName],
		src.[UpdatedBy] AS [SystemUserId],
		src.[UpdatedByFullName] AS [FullName],
		src.TotalRows
		 FROM DATA_CTE src
		WHERE src.row_num between ((@PageNo - 1) * @PageSize + 1 ) 
		and (@PageNo * @PageSize)
		ORDER BY src.row_num 

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemrole_update]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemrole_update]
	@SystemRoleId		NVARCHAR(100),
	@Name				NVARCHAR(100),
	@UpdatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[SystemRole]
		SET 
		[Name] = @Name,
		[UpdatedBy] = @UpdatedBy,
		[UpdatedAt] = GETDATE()
		WHERE [SystemRoleId] = @SystemRoleId;

		SELECT @@ROWCOUNT;
        
    END TRY
    BEGIN CATCH

        SELECT
			'Error - ' + ERROR_MESSAGE()   AS ErrorMessage,
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemuser_add]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_add]
	@LegalEntityId		NVARCHAR(100),
	@UserName			NVARCHAR(100),
	@Password			NVARCHAR(100),
	@LocationId			BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @SystemUserId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'SystemUser', @Id = @SystemUserId OUTPUT

		INSERT INTO [dbo].[SystemUser](
			[SystemUserId], 
			[LegalEntityId], 
			[UserName],
			[Password],
			[LocationId],
			[IsActive],
			[CreatedAt]
		)
		VALUES(
			@SystemUserId,
			@LegalEntityId,
			@UserName,
			HashBytes('MD5', @Password),
			@LocationId,
			1,
			GETDATE()
		);

		SELECT @SystemUserId;
        
    END TRY
    BEGIN CATCH

        SELECT
			'Error - ' + ERROR_MESSAGE()   AS ErrorMessage,
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getByCredentials]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_getByCredentials]
	@Username	NVARCHAR(100),
	@Password	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @SystemUserId		NVARCHAR(100);
		DECLARE @LocationId			BIGINT;
		DECLARE @LegalEntityId		NVARCHAR(100);
		DECLARE @GenderId			NVARCHAR(100);
		DECLARE @CivilStatusTypeId	NVARCHAR(100);
		
		SELECT  @LegalEntityId = su.LegalEntityId, @SystemUserId = su.[SystemUserId],@LocationId = su.[LocationId]
		FROM [dbo].[SystemUser] su
		WHERE su.Username = @Username AND su.[Password] = HashBytes('MD5', @Password) AND IsActive = 1;

		SELECT  *
		FROM [dbo].[SystemUser] AS su
		WHERE su.[SystemUserId] = @SystemUserId;
		
		SELECT *
		FROM [dbo].[Location] AS l
		WHERE l.[LocationId] = @LocationId;
		
		SELECT  
		@GenderId = ei.[GenderId],
		@CivilStatusTypeId = ei.[CivilStatusTypeId]
		FROM [dbo].[Entity_Information] AS ei
		WHERE ei.[LegalEntityId] = @LegalEntityId;

		SELECT  
		ei.[LegalEntityId],
		ei.[FirstName],
		ei.[LastName],
		ei.[MiddleName],
		ei.[BirthDate],
		ei.[Age],
		ei.[CreatedAt],
		ei.[UpdatedAt]
		FROM [dbo].[Entity_Information] AS ei
		WHERE ei.[LegalEntityId] = @LegalEntityId;

		SELECT  
		egt.[GenderId],
		egt.[Name]
		FROM [dbo].[EntityGenderType] AS egt
		WHERE egt.[GenderId] = @GenderId;
		
		SELECT  
		ecst.[CivilStatusTypeId],
		ecst.[Name]
		FROM [dbo].[EntityCivilStatusType] AS ecst
		WHERE ecst.[CivilStatusTypeId] = @CivilStatusTypeId;

		
		SELECT
		eci.[LegalEntityContactId],
		eci.[Value]
		FROM [dbo].[EntityContact_Information] AS eci
		WHERE eci.[LegalEntityId] = @LegalEntityId;

		RETURN 0
        
    END TRY
    BEGIN CATCH

        SELECT
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getByID]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_getByID]
	@SystemUserId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @LocationId			BIGINT;
		DECLARE @LegalEntityId		NVARCHAR(100);
		DECLARE @GenderId			NVARCHAR(100);
		DECLARE @CivilStatusTypeId	NVARCHAR(100);
		
		SELECT  @LegalEntityId = su.LegalEntityId, @LocationId = su.[LocationId]
		FROM [dbo].[SystemUser] su
		WHERE su.[SystemUserId] = @SystemUserId;
		
		SELECT  *
		FROM [dbo].[SystemUser] AS su
		WHERE su.[SystemUserId] = @SystemUserId;
		
		SELECT *
		FROM [dbo].[Location] AS l
		WHERE l.[LocationId] = @LocationId;
		
		SELECT  
		@GenderId = ei.[GenderId],
		@CivilStatusTypeId = ei.[CivilStatusTypeId]
		FROM [dbo].[Entity_Information] AS ei
		WHERE ei.[LegalEntityId] = @LegalEntityId;

		SELECT  
		ei.[LegalEntityId],
		ei.[FirstName],
		ei.[LastName],
		ei.[MiddleName],
		ei.[BirthDate],
		ei.[Age],
		ei.[CreatedAt],
		ei.[UpdatedAt]
		FROM [dbo].[Entity_Information] AS ei
		WHERE ei.[LegalEntityId] = @LegalEntityId;

		SELECT  
		egt.[GenderId],
		egt.[Name]
		FROM [dbo].[EntityGenderType] AS egt
		WHERE egt.[GenderId] = @GenderId;
		
		SELECT  
		ecst.[CivilStatusTypeId],
		ecst.[Name]
		FROM [dbo].[EntityCivilStatusType] AS ecst
		WHERE ecst.[CivilStatusTypeId] = @CivilStatusTypeId;

		SELECT  
		l.LocationId,
		l.[Name]
		FROM [dbo].[Location] AS l
		WHERE l.LocationId = @LocationId;

		
		SELECT
		eci.[LegalEntityContactId],
		eci.[Value]
		FROM [dbo].[EntityContact_Information] AS eci
		WHERE eci.[LegalEntityId] = @LegalEntityId;


		RETURN 0
        
    END TRY
    BEGIN CATCH

        SELECT
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getPaged]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- Description:	filter contract list by location
-- =============================================
CREATE PROCEDURE [dbo].[usp_systemuser_getPaged]
	@SystemUserId NVARCHAR(100) = '',
	@UserName NVARCHAR(100) = '',
	@FullName NVARCHAR(300) = '',
	@Gender NVARCHAR(100) = '',
	@Role NVARCHAR(100) = '',
	@CreatedAt NVARCHAR(100) = '',
	@UpdatedAt NVARCHAR(100) = '',
	@PageNo BIGINT = 1,
	@PageSize BIGINT = 10,
	@LocationId NVARCHAR(100) = 0
AS
BEGIN
	SET NOCOUNT ON;

	
		WITH DATA_CTE
		AS
		(
			Select tableSource.*, ROW_NUMBER() OVER(ORDER BY SystemUserId ASC) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 su.SystemUserId,
			 MAX(su.[UserName])[UserName],
			 MAX(su.[Password])[Password],
			 MAX(su.[CreatedAt])[CreatedAt],
			 MAX(su.[UpdatedAt])[UpdatedAt],
			 MAX(ei.[LegalEntityId])[LegalEntityId],
			 MAX(ei.[FullName])[FullName],
			 MAX(ei.[BirthDate])[BirthDate],
			 MAX(ei.[LocationId])[LocationId],
			 MAX(eg.[GenderId])[GenderId],
			 MAX(eg.[Name])[GenderName],
			 MAX(ecst.[CivilStatusTypeId])[CivilStatusTypeId],
			 MAX(ecst.[Name])[CivilStatusName]
			FROM (SELECT * FROM [dbo].[SystemUser] WHERE [IsActive] = 1) AS su
			LEFT JOIN (SELECT * FROM [dbo].[SystemUserRoles] WHERE [IsActive] = 1) sur ON su.SystemUserId = sur.SystemUserId
			LEFT JOIN (SELECT * FROM [dbo].[SystemRole] WHERE [IsActive] = 1) sr ON sur.SystemRoleId = sr.SystemRoleId
			LEFT JOIN (SELECT *,ISNULL([FirstName],'') + ' ' + ISNULL([MiddleName],'') + ' ' + ISNULL([LastName],'') AS [FullName] FROM [dbo].[Entity_Information] WHERE [IsActive] = 1) AS ei ON su.LegalEntityId = ei.LegalEntityId
			LEFT JOIN (SELECT * FROM [dbo].[EntityGenderType] WHERE [IsActive] = 1) AS eg ON ei.GenderId = eg.GenderId
			LEFT JOIN (SELECT * FROM [dbo].[EntityCivilStatusType] WHERE [IsActive] = 1) AS ecst ON ei.CivilStatusTypeId = ecst.CivilStatusTypeId
			WHERE su.LocationId = @LocationId
			AND (su.SystemUserId like '%' + @SystemUserId + '%' OR @SystemUserId= '' OR @SystemUserId IS NULL)
			AND (su.[UserName] like '%' + @UserName + '%' OR @UserName= '' OR @UserName IS NULL)
			AND (su.[CreatedAt] like '%' + @CreatedAt + '%' OR @CreatedAt= '' OR @CreatedAt IS NULL)
			AND (su.[UpdatedAt] like '%' + @UpdatedAt + '%' OR @UpdatedAt= '' OR @UpdatedAt IS NULL)
			AND (sr.[Name] like '%' + @Role + '%' OR @Role= '' OR @Role IS NULL)
			AND (ei.[FullName] like '%' + @FullName + '%' OR @FullName= '' OR @FullName IS NULL)
			AND (eg.[Name] like '%' + @Gender + '%' OR @Gender= '' OR @Gender IS NULL)
			GROUP BY su.SystemUserId
			) tableSource
		)
		SELECT 
		src.SystemUserId,
		src.[UserName],
		src.[Password],
		src.[CreatedAt],
		src.[UpdatedAt],
		src.[LegalEntityId],
		src.[FullName],
		src.[BirthDate],
		src.[LocationId],
		src.[GenderId],
		src.[GenderName] AS [Name],
		src.[CivilStatusTypeId],
		src.[CivilStatusName] AS [Name],
		src.TotalRows
		 FROM DATA_CTE src
		WHERE src.row_num between ((@PageNo - 1) * @PageSize + 1 ) 
		and (@PageNo * @PageSize)
		ORDER BY src.row_num 

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemuser_update]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_update]
	@SystemUserId		NVARCHAR(100),
	@UpdatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[SystemUser]
		SET 
		[UpdatedBy] = @UpdatedBy,
		[UpdatedAt] = GETDATE()
		WHERE [SystemUserId] = @SystemUserId;

		SELECT @@ROWCOUNT;
        
    END TRY
    BEGIN CATCH

        SELECT
			'Error - ' + ERROR_MESSAGE()   AS ErrorMessage,
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemuserroles_add]    Script Date: 1/10/2021 10:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuserroles_add]
	@SystemUserId		NVARCHAR(100),
	@SystemRoleId		NVARCHAR(100),
	@LocationId			BIGINT,
	@CreatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @SystemUserRoleId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'SystemUserRoles', @Id = @SystemUserRoleId OUTPUT

		INSERT INTO [dbo].[SystemUserRoles](
			[SystemUserRoleId],
			[SystemUserId],
			[SystemRoleId],
			[LocationId],
			[IsActive],
			[CreatedBy],
			[CreatedAt]
		)
		VALUES(
			@SystemUserRoleId,
			@SystemUserId,
			@SystemRoleId,
			@LocationId,
			1,
			@CreatedBy,
			GETDATE()
		);

		SELECT @SystemUserRoleId;
        
    END TRY
    BEGIN CATCH

        SELECT
			'Error - ' + ERROR_MESSAGE()   AS ErrorMessage,
            'Error'           AS Status,
            ERROR_NUMBER()    AS ErrorNumber,
            ERROR_SEVERITY()  AS ErrorSeverity,
            ERROR_STATE()     AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE()      AS ErrorLine,
            ERROR_MESSAGE()   AS ErrorMessage;

    END CATCH

END
GO
