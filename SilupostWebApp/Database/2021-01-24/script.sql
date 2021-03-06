USE [CrimeReportAppDB]
GO
/****** Object:  UserDefinedFunction [dbo].[LPAD]    Script Date: 1/24/2021 1:02:55 PM ******/
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
/****** Object:  UserDefinedFunction [dbo].[svf_getUserFullName]    Script Date: 1/24/2021 1:02:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		
-- Create date: Sept 25, 2020
-- Description:	FOR GENERATING SEQUENCE ID WITH PADDING LENGTH
-- =============================================
CREATE FUNCTION [dbo].[svf_getUserFullName]
(
    @SystemUserId NVARCHAR(100)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
    RETURN (SELECT TOP 1 CONCAT(ISNULL(le.[FirstName],''), ' ', ISNULL(le.[MiddleName],''), ' ', ISNULL(le.[LastName],'')) 
	FROM [dbo].[SystemUser] AS su
	LEFT JOIN [dbo].[LegalEntity] le ON su.LegalEntityId = le.LegalEntityId
	WHERE su.SystemUserId = @SystemUserId)
END


GO
/****** Object:  UserDefinedFunction [dbo].[tvf_SplitString]    Script Date: 1/24/2021 1:02:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[tvf_SplitString]
(    
      @Input NVARCHAR(MAX),
      @Character CHAR(1)
)
RETURNS @Output TABLE (
      Item NVARCHAR(1000)
)
AS
BEGIN
      DECLARE @StartIndex INT, @EndIndex INT
 
      SET @StartIndex = 1
      IF SUBSTRING(@Input, LEN(@Input) - 1, LEN(@Input)) <> @Character
      BEGIN
            SET @Input = @Input + @Character
      END
 
      WHILE CHARINDEX(@Character, @Input) > 0
      BEGIN
            SET @EndIndex = CHARINDEX(@Character, @Input)
           
            INSERT INTO @Output(Item)
            SELECT SUBSTRING(@Input, @StartIndex, @EndIndex - 1)
           
            SET @Input = SUBSTRING(@Input, @EndIndex + 1, LEN(@Input))
      END
		delete from @Output where Item = ''
      RETURN
END
GO
/****** Object:  Table [dbo].[CrimeIncidentCategory]    Script Date: 1/24/2021 1:02:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CrimeIncidentCategory](
	[CrimeIncidentCategoryId] [nvarchar](100) NOT NULL,
	[CrimeIncidentTypeId] [nvarchar](100) NOT NULL,
	[CrimeIncidentCategoryName] [nvarchar](250) NOT NULL,
	[CrimeIncidentCategoryDescription] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_CrimeIncidentCategory] PRIMARY KEY CLUSTERED 
(
	[CrimeIncidentCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CrimeIncidentType]    Script Date: 1/24/2021 1:02:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CrimeIncidentType](
	[CrimeIncidentTypeId] [nvarchar](100) NOT NULL,
	[CrimeIncidentTypeName] [nvarchar](250) NOT NULL,
	[CrimeIncidentTypeDescription] [nvarchar](max) NULL,
	[IconFileId] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_CrimeIncidentType] PRIMARY KEY CLUSTERED 
(
	[CrimeIncidentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EntityContactType]    Script Date: 1/24/2021 1:02:56 PM ******/
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
/****** Object:  Table [dbo].[EntityGender]    Script Date: 1/24/2021 1:02:56 PM ******/
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
/****** Object:  Table [dbo].[EntityStatus]    Script Date: 1/24/2021 1:02:56 PM ******/
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
/****** Object:  Table [dbo].[File]    Script Date: 1/24/2021 1:02:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[File](
	[FileId] [nvarchar](100) NOT NULL,
	[FileName] [nvarchar](250) NOT NULL,
	[MimeType] [nvarchar](100) NOT NULL,
	[FileSize] [bigint] NOT NULL,
	[FileContent] [varbinary](max) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LegalEntity]    Script Date: 1/24/2021 1:02:56 PM ******/
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
	[EmailAddress] [nvarchar](100) NOT NULL,
	[MobileNumber] [bigint] NOT NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [LegalEntityId] PRIMARY KEY CLUSTERED 
(
	[LegalEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LegalGeoAddress]    Script Date: 1/24/2021 1:02:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LegalGeoAddress](
	[LegalGeoAddressId] [nvarchar](100) NOT NULL,
	[LegalEntityId] [nvarchar](100) NOT NULL,
	[Latitude] [decimal](18, 8) NOT NULL,
	[Longitude] [decimal](18, 8) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_LegalGeoAddress] PRIMARY KEY CLUSTERED 
(
	[LegalGeoAddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sequence]    Script Date: 1/24/2021 1:02:56 PM ******/
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
/****** Object:  Table [dbo].[SystemConfig]    Script Date: 1/24/2021 1:02:56 PM ******/
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
/****** Object:  Table [dbo].[SystemConfigType]    Script Date: 1/24/2021 1:02:56 PM ******/
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
/****** Object:  Table [dbo].[SystemUser]    Script Date: 1/24/2021 1:02:56 PM ******/
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
/****** Object:  Table [dbo].[SystemUserConfig]    Script Date: 1/24/2021 1:02:56 PM ******/
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
/****** Object:  Table [dbo].[SystemUserContact]    Script Date: 1/24/2021 1:02:57 PM ******/
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
/****** Object:  Table [dbo].[SystemUserType]    Script Date: 1/24/2021 1:02:57 PM ******/
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
/****** Object:  Table [dbo].[SystemWebAdminMenu]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemWebAdminMenu](
	[SystemWebAdminMenuId] [bigint] NOT NULL,
	[SystemWebAdminModuleId] [bigint] NOT NULL,
	[SystemWebAdminMenuName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_SystemWebAdminMenu] PRIMARY KEY CLUSTERED 
(
	[SystemWebAdminMenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemWebAdminMenuRole]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemWebAdminMenuRole](
	[SystemWebAdminMenuRoleId] [nvarchar](100) NOT NULL,
	[SystemWebAdminMenuId] [nvarchar](100) NOT NULL,
	[SystemWebAdminRoleId] [nvarchar](100) NOT NULL,
	[IsAllowed] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_SystemWebAdminMenuRole] PRIMARY KEY CLUSTERED 
(
	[SystemWebAdminMenuRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemWebAdminModule]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemWebAdminModule](
	[SystemWebAdminModuleId] [bigint] NOT NULL,
	[SystemWebAdminModuleName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_SystemWebAdminModule] PRIMARY KEY CLUSTERED 
(
	[SystemWebAdminModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemWebAdminRole]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemWebAdminRole](
	[SystemWebAdminRoleId] [nvarchar](100) NOT NULL,
	[RoleName] [nvarchar](250) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_SystemRole] PRIMARY KEY CLUSTERED 
(
	[SystemWebAdminRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemWebAdminUserRole]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemWebAdminUserRole](
	[SystemWebAdminUserRoleId] [nvarchar](100) NOT NULL,
	[SystemUserId] [nvarchar](100) NOT NULL,
	[SystemWebAdminRoleId] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_SystemUserRoles] PRIMARY KEY CLUSTERED 
(
	[SystemWebAdminUserRoleId] ASC
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
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (4, N'SystemWebAdminUserRole', N'SWAUR-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (5, N'SystemWebAdminRole', N'SWAR-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (6, N'LegalEntity', N'LE-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (7, N'LegalGeoAddress', N'LGA-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (8, N'SystemWebAdminMenuRole', N'SWAMR-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (9, N'CrimeIncidentType', N'CIT-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (10, N'CrimeIncidentCategory', N'CIC-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (11, N'File', N'F-', 10, 0)
GO
INSERT [dbo].[SystemConfigType] ([SystemConfigTypeId], [ValueType]) VALUES (1, N'BOOLEAN')
GO
INSERT [dbo].[SystemConfigType] ([SystemConfigTypeId], [ValueType]) VALUES (2, N'TEXT')
GO
INSERT [dbo].[SystemUserType] ([SystemUserTypeId], [SystemUserTypeName]) VALUES (1, N'WebAppAdmin')
GO
INSERT [dbo].[SystemUserType] ([SystemUserTypeId], [SystemUserTypeName]) VALUES (2, N'MobileAppUser')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (1, 1, N'Dashboard')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (2, 2, N'Report Tracker')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (3, 3, N'Crime Report')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (4, 3, N'Incident Report')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (5, 4, N'Mobile User')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (7, 5, N'CrimeType')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (11, 5, N'Enforcement Station')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (9, 5, N'Enforcement Type')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (10, 5, N'Enforcement Unit')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (8, 5, N'IncidentType')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (6, 5, N'SystemConfiguration')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (14, 6, N'System Menu Roles')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (12, 6, N'System Role')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName]) VALUES (13, 6, N'System User')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (1, N'Dashboard')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (4, N'Mobile Entity')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (3, N'Report Library')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (2, N'Report Tracker')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (5, N'System Setup')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (6, N'Web Admin Security')
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_CrimeIncidentCategory]    Script Date: 1/24/2021 1:02:57 PM ******/
ALTER TABLE [dbo].[CrimeIncidentCategory] ADD  CONSTRAINT [UK_CrimeIncidentCategory] UNIQUE NONCLUSTERED 
(
	[CrimeIncidentTypeId] ASC,
	[CrimeIncidentCategoryName] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_CrimeIncidentType]    Script Date: 1/24/2021 1:02:57 PM ******/
ALTER TABLE [dbo].[CrimeIncidentType] ADD  CONSTRAINT [UK_CrimeIncidentType] UNIQUE NONCLUSTERED 
(
	[CrimeIncidentTypeName] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [U_Sequence]    Script Date: 1/24/2021 1:02:57 PM ******/
ALTER TABLE [dbo].[Sequence] ADD  CONSTRAINT [U_Sequence] UNIQUE NONCLUSTERED 
(
	[TableName] ASC,
	[Prefix] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_SystemUser]    Script Date: 1/24/2021 1:02:57 PM ******/
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [UK_SystemUser] UNIQUE NONCLUSTERED 
(
	[UserName] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_SystemWebAdminMenu]    Script Date: 1/24/2021 1:02:57 PM ******/
ALTER TABLE [dbo].[SystemWebAdminMenu] ADD  CONSTRAINT [UK_SystemWebAdminMenu] UNIQUE NONCLUSTERED 
(
	[SystemWebAdminModuleId] ASC,
	[SystemWebAdminMenuName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_SystemWebAdminMenuRole]    Script Date: 1/24/2021 1:02:57 PM ******/
ALTER TABLE [dbo].[SystemWebAdminMenuRole] ADD  CONSTRAINT [UK_SystemWebAdminMenuRole] UNIQUE NONCLUSTERED 
(
	[SystemWebAdminMenuId] ASC,
	[SystemWebAdminRoleId] ASC,
	[IsAllowed] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_SystemWebAdminModule]    Script Date: 1/24/2021 1:02:57 PM ******/
ALTER TABLE [dbo].[SystemWebAdminModule] ADD  CONSTRAINT [UK_SystemWebAdminModule] UNIQUE NONCLUSTERED 
(
	[SystemWebAdminModuleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UK_SystemRole]    Script Date: 1/24/2021 1:02:57 PM ******/
ALTER TABLE [dbo].[SystemWebAdminRole] ADD  CONSTRAINT [UK_SystemRole] UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[File] ADD  CONSTRAINT [DF_File_FileSize]  DEFAULT ((0)) FOR [FileSize]
GO
ALTER TABLE [dbo].[File] ADD  CONSTRAINT [DF_File_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[File] ADD  CONSTRAINT [DF_File_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
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
ALTER TABLE [dbo].[SystemWebAdminMenuRole] ADD  CONSTRAINT [DF_SystemWebAdminMenuRole_IsAllowed]  DEFAULT ((0)) FOR [IsAllowed]
GO
ALTER TABLE [dbo].[SystemWebAdminMenuRole] ADD  CONSTRAINT [DF_SystemWebAdminMenuRole_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SystemWebAdminMenuRole] ADD  CONSTRAINT [DF_SystemWebAdminMenuRole_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[SystemWebAdminRole] ADD  CONSTRAINT [DF_Role_IsPostedToMain]  DEFAULT ((0)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[SystemWebAdminRole] ADD  CONSTRAINT [DF_Role_IsFromMain]  DEFAULT ((0)) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SystemWebAdminRole] ADD  CONSTRAINT [DF_SystemRole_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[SystemWebAdminUserRole] ADD  CONSTRAINT [DF_SystemUserRoles_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SystemWebAdminUserRole] ADD  CONSTRAINT [DF_SystemUserRoles_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[CrimeIncidentCategory]  WITH CHECK ADD  CONSTRAINT [FK_CrimeIncidentCategory_CrimeIncidentType] FOREIGN KEY([CrimeIncidentTypeId])
REFERENCES [dbo].[CrimeIncidentType] ([CrimeIncidentTypeId])
GO
ALTER TABLE [dbo].[CrimeIncidentCategory] CHECK CONSTRAINT [FK_CrimeIncidentCategory_CrimeIncidentType]
GO
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_add]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentcategory_add]
	@CrimeIncidentTypeId				NVARCHAR(100),
	@CrimeIncidentCategoryName			NVARCHAR(250),
	@CrimeIncidentCategoryDescription	NVARCHAR(MAX),
	@CreatedBy							NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @CrimeIncidentCategoryId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'CrimeIncidentCategory', @Id = @CrimeIncidentCategoryId OUTPUT

		INSERT INTO [dbo].[CrimeIncidentCategory](
			[CrimeIncidentCategoryId],
			[CrimeIncidentTypeId],
			[CrimeIncidentCategoryName],
			[CrimeIncidentCategoryDescription],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@CrimeIncidentCategoryId,
			@CrimeIncidentTypeId,
			@CrimeIncidentCategoryName,
			@CrimeIncidentCategoryDescription,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @CrimeIncidentCategoryId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_delete]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentcategory_delete]
	@CrimeIncidentCategoryId		NVARCHAR(100),
	@LastUpdatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[CrimeIncidentCategory] WHERE [CrimeIncidentCategoryId] = @CrimeIncidentCategoryId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[CrimeIncidentCategory]
			SET 
			[CrimeIncidentCategoryName] = @CrimeIncidentCategoryId + ' - ' + [CrimeIncidentCategoryName] + '(DELETED - ' + CONVERT(VARCHAR(50),GETDATE())+ ')',
			EntityStatusId = 2,
			[LastUpdatedBy] = @LastUpdatedBy,
			[LastUpdatedAt] = GETDATE()
			WHERE [CrimeIncidentCategoryId] = @CrimeIncidentCategoryId;

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_getByID]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentcategory_getByID]
	@CrimeIncidentCategoryId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @CrimeIncidentTypeId			NVARCHAR(100);
		DECLARE @IconFileId						NVARCHAR(100);
		DECLARE @CreatedBy						NVARCHAR(100);
		DECLARE @CreatedAt						DATETIME;
		DECLARE @LastUpdatedBy					NVARCHAR(100);
		DECLARE @LastUpdatedAt					DATETIME;
		DECLARE @EntityStatusId					BIGINT;
		
		SELECT 
		@CrimeIncidentTypeId = cic.[CrimeIncidentTypeId],
		@CreatedBy = cic.[CreatedBy],
		@CreatedAt = cic.[CreatedAt],
		@LastUpdatedBy = cic.[LastUpdatedBy],
		@LastUpdatedAt = cic.[LastUpdatedAt],
		@EntityStatusId = cic.[EntityStatusId]
		FROM [dbo].[CrimeIncidentCategory] cic
		WHERE cic.[CrimeIncidentCategoryId] = @CrimeIncidentCategoryId
		AND cic.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[CrimeIncidentCategory] AS cic
		WHERE cic.[CrimeIncidentCategoryId] = @CrimeIncidentCategoryId
		AND cic.[EntityStatusId] = 1;
		
		SELECT 
		@IconFileId = cit.[IconFileId]
		FROM [dbo].[CrimeIncidentType] cit
		WHERE cit.[CrimeIncidentTypeId] = @CrimeIncidentTypeId
		AND cit.[EntityStatusId] = 1;

		SELECT  *
		FROM [dbo].[CrimeIncidentType] AS cit
		WHERE cit.[CrimeIncidentTypeId] = @CrimeIncidentTypeId
		AND cit.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[File] AS f
		WHERE f.[FileId] = @IconFileId
		AND f.[EntityStatusId] = 1;

		SELECT 
		@CreatedBy AS CreatedBy,
		@CreatedAt AS CreatedAt,
		[dbo].[svf_getUserFullName](@CreatedBy) AS CreatedByFullName,
		@LastUpdatedBy AS LastUpdatedBy,
		@LastUpdatedAt AS LastUpdatedAt,
		[dbo].[svf_getUserFullName](@LastUpdatedBy) AS LastUpdatedByFullName

		SELECT  
		es.[EntityStatusId],
		es.[EntityStatusName]
		FROM [dbo].[EntityStatus] AS es
		WHERE es.[EntityStatusId] = @EntityStatusId;

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_getPaged]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- =============================================
CREATE PROCEDURE [dbo].[usp_crimeincidentcategory_getPaged]
	@Search					NVARCHAR(50) = '',
	@CrimeIncidentTypeId	NVARCHAR(100),
	@PageNo					BIGINT = 1,
	@PageSize				BIGINT = 10,
	@OrderColumn		NVARCHAR(100),
	@OrderDir			NVARCHAR(5)
AS
BEGIN
	SET NOCOUNT ON;

	
		WITH DATA_CTE
		AS
		(
			Select tableSource.*, 
			(CASE @OrderDir
			 WHEN 'asc' THEN
				CASE @OrderColumn 
					WHEN 'CrimeIncidentCategoryId' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryId] ASC)
					WHEN 'CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] ASC)
					WHEN 'CrimeIncidentCategoryDescription' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryDescription] ASC)
				END
			WHEN 'desc' THEN
				CASE @OrderColumn 
					WHEN 'CrimeIncidentCategoryId' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryId] DESC)
					WHEN 'CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] DESC)
					WHEN 'CrimeIncidentCategoryDescription' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryDescription] DESC)
				END
			 END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 cic.[CrimeIncidentCategoryId],
			 MAX(cic.[CrimeIncidentCategoryName])[CrimeIncidentCategoryName],
			 MAX(cic.[CrimeIncidentCategoryDescription])[CrimeIncidentCategoryDescription]
			FROM [dbo].[CrimeIncidentCategory] AS cic
			WHERE cic.EntityStatusId = 1
			AND cic.CrimeIncidentTypeId = @CrimeIncidentTypeId
			AND (cic.[CrimeIncidentCategoryId] like '%' + @Search + '%' 
			OR cic.[CrimeIncidentCategoryName] like '%' + @Search + '%'
			OR cic.[CrimeIncidentCategoryDescription] like '%' + @Search + '%'
			)
			GROUP BY cic.[CrimeIncidentCategoryId]
			) tableSource
		)
		SELECT 
		src.*
		 FROM DATA_CTE src
		WHERE src.row_num between ((@PageNo - 1) * @PageSize + 1 ) 
		and (@PageNo * @PageSize)
		ORDER BY src.row_num 

END

GO
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_update]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentcategory_update]
	@CrimeIncidentCategoryId			NVARCHAR(100),
	@CrimeIncidentTypeId				NVARCHAR(100),
	@CrimeIncidentCategoryName			NVARCHAR(100),
	@CrimeIncidentCategoryDescription	NVARCHAR(MAX),
	@LastUpdatedBy						NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[CrimeIncidentCategory]
		SET 
		[CrimeIncidentTypeId] = @CrimeIncidentTypeId,
		[CrimeIncidentCategoryName] = @CrimeIncidentCategoryName,
		[CrimeIncidentCategoryDescription] = @CrimeIncidentCategoryDescription,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [CrimeIncidentCategoryId] = @CrimeIncidentCategoryId;

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidenttype_add]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidenttype_add]
	@CrimeIncidentTypeName			NVARCHAR(100),
	@CrimeIncidentTypeDescription	NVARCHAR(MAX),
	@IconFileId						NVARCHAR(100),
	@CreatedBy						NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @CrimeIncidentTypeId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'CrimeIncidentType', @Id = @CrimeIncidentTypeId OUTPUT

		INSERT INTO [dbo].[CrimeIncidentType](
			[CrimeIncidentTypeId],
			[CrimeIncidentTypeName],
			[CrimeIncidentTypeDescription],
			[IconFileId],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@CrimeIncidentTypeId,
			@CrimeIncidentTypeName,
			@CrimeIncidentTypeDescription,
			@IconFileId,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @CrimeIncidentTypeId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidenttype_delete]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidenttype_delete]
	@CrimeIncidentTypeId		NVARCHAR(100),
	@LastUpdatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[CrimeIncidentType] WHERE [CrimeIncidentTypeId] = @CrimeIncidentTypeId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[CrimeIncidentType]
			SET 
			[CrimeIncidentTypeName] = @CrimeIncidentTypeId + ' - ' + [CrimeIncidentTypeName] + '(DELETED - ' + CONVERT(VARCHAR(50),GETDATE())+ ')',
			EntityStatusId = 2,
			[LastUpdatedBy] = @LastUpdatedBy,
			[LastUpdatedAt] = GETDATE()
			WHERE [CrimeIncidentTypeId] = @CrimeIncidentTypeId;

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidenttype_getByID]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidenttype_getByID]
	@CrimeIncidentTypeId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @IconFileId			NVARCHAR(100);
		DECLARE @CreatedBy			NVARCHAR(100);
		DECLARE @CreatedAt			DATETIME;
		DECLARE @LastUpdatedBy		NVARCHAR(100);
		DECLARE @LastUpdatedAt		DATETIME;
		DECLARE @EntityStatusId		BIGINT;
		
		SELECT 
		@IconFileId = cit.[IconFileId],
		@CreatedBy = cit.[CreatedBy],
		@CreatedAt = cit.[CreatedAt],
		@LastUpdatedBy = cit.[LastUpdatedBy],
		@LastUpdatedAt = cit.[LastUpdatedAt],
		@EntityStatusId = cit.[EntityStatusId]
		FROM [dbo].[CrimeIncidentType] cit
		WHERE cit.[CrimeIncidentTypeId] = @CrimeIncidentTypeId
		AND cit.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[CrimeIncidentType] AS cit
		WHERE cit.[CrimeIncidentTypeId] = @CrimeIncidentTypeId
		AND cit.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[File] AS f
		WHERE f.[FileId] = @IconFileId
		AND f.[EntityStatusId] = 1;

		SELECT 
		@CreatedBy AS CreatedBy,
		@CreatedAt AS CreatedAt,
		[dbo].[svf_getUserFullName](@CreatedBy) AS CreatedByFullName,
		@LastUpdatedBy AS LastUpdatedBy,
		@LastUpdatedAt AS LastUpdatedAt,
		[dbo].[svf_getUserFullName](@LastUpdatedBy) AS LastUpdatedByFullName

		SELECT  
		es.[EntityStatusId],
		es.[EntityStatusName]
		FROM [dbo].[EntityStatus] AS es
		WHERE es.[EntityStatusId] = @EntityStatusId;

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidenttype_getPaged]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- =============================================
CREATE PROCEDURE [dbo].[usp_crimeincidenttype_getPaged]
	@Search			NVARCHAR(50) = '',
	@PageNo			BIGINT = 1,
	@PageSize		BIGINT = 10,
	@OrderColumn	NVARCHAR(100),
	@OrderDir		NVARCHAR(5)
AS
BEGIN
	SET NOCOUNT ON;

	
		WITH DATA_CTE
		AS
		(
			Select tableSource.*, 
			(CASE @OrderDir
			 WHEN 'asc' THEN
				CASE @OrderColumn 
					WHEN 'CrimeIncidentTypeId' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentTypeId] ASC)
					WHEN 'CrimeIncidentTypeName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentTypeName] ASC)
					WHEN 'CrimeIncidentTypeDescription' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentTypeDescription] ASC)
				END
			WHEN 'desc' THEN
				CASE @OrderColumn 
					WHEN 'CrimeIncidentTypeId' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentTypeId] DESC)
					WHEN 'Description' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentTypeName] DESC)
					WHEN 'CrimeIncidentTypeDescription' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentTypeDescription] DESC)
				END
			 END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 cit.[CrimeIncidentTypeId],
			 MAX(cit.[CrimeIncidentTypeName])[CrimeIncidentTypeName],
			 MAX(cit.[CrimeIncidentTypeDescription])[CrimeIncidentTypeDescription],
			 MAX(f.[FileId])[FileId],
			 MAX(f.[FileName])[FileName],
			 MAX(f.[MimeType])[MimeType],
			 MAX(f.[FileSize])[FileSize],
			 MAX(f.[FileContent])[FileContent]
			FROM [dbo].[CrimeIncidentType] AS cit
			LEFT JOIN [dbo].[File] f ON cit.IconFileId = f.FileId
			WHERE cit.EntityStatusId = 1
			AND (cit.[CrimeIncidentTypeId] like '%' + @Search + '%' 
			OR cit.[CrimeIncidentTypeName] like '%' + @Search + '%'
			OR cit.[CrimeIncidentTypeDescription] like '%' + @Search + '%'
			)
			GROUP BY cit.[CrimeIncidentTypeId]
			) tableSource
		)
		SELECT 
		src.*
		 FROM DATA_CTE src
		WHERE src.row_num between ((@PageNo - 1) * @PageSize + 1 ) 
		and (@PageNo * @PageSize)
		ORDER BY src.row_num 

END

GO
/****** Object:  StoredProcedure [dbo].[usp_crimeincidenttype_update]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidenttype_update]
	@CrimeIncidentTypeId			NVARCHAR(100),
	@CrimeIncidentTypeName			NVARCHAR(100),
	@CrimeIncidentTypeDescription	NVARCHAR(MAX),
	@LastUpdatedBy					NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[CrimeIncidentType]
		SET 
		[CrimeIncidentTypeName] = @CrimeIncidentTypeName,
		[CrimeIncidentTypeDescription] = @CrimeIncidentTypeDescription,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [CrimeIncidentTypeId] = @CrimeIncidentTypeId;

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
/****** Object:  StoredProcedure [dbo].[usp_file_add]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_file_add]
	@FileName			NVARCHAR(250),
	@MimeType			NVARCHAR(100),
	@FileSize			bigint,
	@FileContent		VARBINARY(MAX),
	@CreatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @FileId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'File', @Id = @FileId OUTPUT

		INSERT INTO [dbo].[File](
			FileId, 
			[FileName], 
			[MimeType],
			[FileSize],
			[FileContent],
			[CreatedBy], 
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@FileId,
			@FileName, 
			@MimeType,
			@FileSize,
			@FileContent,
			@CreatedBy, 
			GETDATE(),
			1
		);

		SELECT @FileId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_file_getById]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_file_getById]
	@FileId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @CreatedBy						NVARCHAR(100);
		DECLARE @CreatedAt						DATETIME;
		DECLARE @LastUpdatedBy					NVARCHAR(100);
		DECLARE @LastUpdatedAt					DATETIME;
		DECLARE @EntityStatusId					BIGINT;
		SELECT 
		@CreatedBy = f.[CreatedBy],
		@CreatedAt = f.[CreatedAt],
		@LastUpdatedBy = f.[LastUpdatedBy],
		@LastUpdatedAt = f.[LastUpdatedAt],
		@EntityStatusId = f.[EntityStatusId]
		FROM [dbo].[File] f
		WHERE f.[FileId] = @FileId
		AND f.[EntityStatusId] = 1;

		SELECT * FROM [dbo].[File] WHERE [FileId] = @FileId
		AND [EntityStatusId] = 1;

		SELECT 
		@CreatedBy AS CreatedBy,
		@CreatedAt AS CreatedAt,
		[dbo].[svf_getUserFullName](@CreatedBy) AS CreatedByFullName,
		@LastUpdatedBy AS LastUpdatedBy,
		@LastUpdatedAt AS LastUpdatedAt,
		[dbo].[svf_getUserFullName](@LastUpdatedBy) AS LastUpdatedByFullName

		SELECT  
		es.[EntityStatusId],
		es.[EntityStatusName]
		FROM [dbo].[EntityStatus] AS es
		WHERE es.[EntityStatusId] = @EntityStatusId;
		
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
/****** Object:  StoredProcedure [dbo].[usp_file_update]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_file_update]
	@FileId				varchar(100),
	@FileName			NVARCHAR(250),
	@MimeType			NVARCHAR(100),
	@FileSize			bigint,
	@FileContent		VARBINARY(MAX),
	@LastUpdatedBy		NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
	
		UPDATE [dbo].[File] SET 
		[FileName] = @FileName, 
		[MimeType] = @MimeType, 
		[FileSize] = @FileSize, 
		[FileContent] = @FileContent, 
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE() 
		WHERE FileId = @FileId;
		SELECT @@ROWCOUNT;
		RETURN 0
        
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
/****** Object:  StoredProcedure [dbo].[usp_legalentity_add]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalentity_add]
	@FirstName			NVARCHAR(100),
	@LastName			NVARCHAR(100),
	@MiddleName			NVARCHAR(100),
	@GenderId			NVARCHAR(100),
	@BirthDate			DATE,
	@EmailAddress		NVARCHAR(100),
	@MobileNumber		BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @LegalEntityId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'LegalEntity', @Id = @LegalEntityId OUTPUT

		INSERT INTO [dbo].[LegalEntity](
			[LegalEntityId], 
			[FirstName], 
			[LastName],
			[MiddleName],
			[GenderId],
			[BirthDate],
			[EmailAddress],
			[MobileNumber],
			[EntityStatusId]
		)
		VALUES(
			@LegalEntityId,
			@FirstName,
			@LastName,
			@MiddleName,
			@GenderId,
			@BirthDate,
			@EmailAddress,
			@MobileNumber,
			1
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
/****** Object:  StoredProcedure [dbo].[usp_legalentity_update]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalentity_update]
	@LegalEntityId		NVARCHAR(100),
	@FirstName			NVARCHAR(100),
	@LastName			NVARCHAR(100),
	@MiddleName			NVARCHAR(100),
	@GenderId			NVARCHAR(100),
	@BirthDate			DATE
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		UPDATE [dbo].[LegalEntity] SET
			[FirstName] = @FirstName, 
			[LastName] = @LastName,
			[MiddleName] = @MiddleName,
			[GenderId] = @GenderId,
			[BirthDate] = @BirthDate
		WHERE [LegalEntityId] = @LegalEntityId;
		
		SELECT 1;
        
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
/****** Object:  StoredProcedure [dbo].[usp_legalgeoaddress_add]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalgeoaddress_add]
	@LegalEntityId	NVARCHAR(100),
	@Latitude	DECIMAL(18,8),
	@Longitude	DECIMAL(18,8)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @LegalGeoAddressId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'LegalGeoAddress', @Id = @LegalGeoAddressId OUTPUT

		INSERT INTO [dbo].[LegalGeoAddress](
			[LegalGeoAddressId],
			[LegalEntityId],
			[Latitude],
			[Longitude],
			[EntityStatusId]
		)
		VALUES(
			@LegalGeoAddressId,
			@LegalEntityId,
			@Latitude,
			@Longitude,
			1
		);

		SELECT @LegalGeoAddressId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_legalgeoaddress_delete]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalgeoaddress_delete]
	@LegalGeoAddressId	NVARCHAR(100),
	@LastUpdatedBy		NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[LegalGeoAddress] WHERE [LegalGeoAddressId] = @LegalGeoAddressId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[LegalGeoAddress]
			SET 
			EntityStatusId = 2
			WHERE [LegalGeoAddressId] = @LegalGeoAddressId;

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
/****** Object:  StoredProcedure [dbo].[usp_legalgeoaddresss_getPaged]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- Description:	filter contract list by location
-- =============================================
CREATE PROCEDURE [dbo].[usp_legalgeoaddresss_getPaged]
	@LegalEntityId		NVARCHAR(100) = '',
	@Address			NVARCHAR(MAX) = '',
	@PageNo BIGINT = 1,
	@PageSize BIGINT = 10
AS
BEGIN
	SET NOCOUNT ON;

	
		WITH DATA_CTE
		AS
		(
			Select tableSource.*, ROW_NUMBER() OVER(ORDER BY [LegalGeoAddressId] ASC) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 lga.[LegalGeoAddressId],
			 MAX(lga.[Address])[Address],
			 MAX(lga.[Latitude])[Latitude],
			 MAX(lga.[Longitude])[Longitude]
			FROM [dbo].[LegalGeoAddress] AS lga
			WHERE lga.EntityStatusId = 1 AND lga.LegalEntityId = @LegalEntityId
			AND (lga.[Address] like '%' + @Address + '%' OR @Address= '' OR @Address IS NULL)
			GROUP BY lga.[LegalGeoAddressId]
			) tableSource
		)
		SELECT 
		src.*
		 FROM DATA_CTE src
		WHERE src.row_num between ((@PageNo - 1) * @PageSize + 1 ) 
		and (@PageNo * @PageSize)
		ORDER BY src.row_num 

END

GO
/****** Object:  StoredProcedure [dbo].[usp_lookuptable_getByTableNames]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_lookuptable_getByTableNames]
	@TableNames NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		
		SELECT 'EntityContactType' AS LookupName ,CAST([EntityContactTypeId] AS nvarchar(100)) AS Id,[EntityContactTypeName] AS Name FROM [dbo].[EntityContactType]
		WHERE 'EntityContactType' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )
		
		UNION ALL

		SELECT 'EntityGender' AS LookupName,CAST([GenderId] AS nvarchar(100)) AS Id,[GenderName] AS Name FROM [dbo].[EntityGender]
		WHERE 'EntityGender' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )

		UNION ALL

		SELECT 'EntityStatus' AS LookupName,CAST([EntityStatusId] AS nvarchar(100)) AS Id,[EntityStatusName] AS Name FROM [dbo].[EntityStatus]
		WHERE 'EntityStatus' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )

		UNION ALL

		SELECT 'SystemConfigType' AS LookupName,CAST([SystemConfigTypeId] AS nvarchar(100)) AS Id,[ValueType] AS Name FROM [dbo].[SystemConfigType]
		WHERE 'SystemConfigType' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )

		UNION ALL

		SELECT 'SystemUserType' AS LookupName,CAST([SystemUserTypeId] AS nvarchar(100)) AS Id,[SystemUserTypeName] AS Name FROM [dbo].[SystemUserType]
		WHERE 'SystemUserType' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )

		UNION ALL

		SELECT 'SystemWebAdminMenu' AS LookupName,CAST([SystemWebAdminMenuId] AS nvarchar(100)) AS Id,[SystemWebAdminMenuName] AS Name FROM [dbo].[SystemWebAdminMenu]
		WHERE 'SystemWebAdminMenu' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )

		UNION ALL

		SELECT 'SystemWebAdminModule' AS LookupName,CAST([SystemWebAdminModuleId] AS nvarchar(100)) AS Id,[SystemWebAdminModuleName] AS Name FROM [dbo].[SystemWebAdminModule]
		WHERE 'SystemWebAdminModule' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )
		
		UNION ALL

		SELECT 'SystemWebAdminRole' AS LookupName,[SystemWebAdminRoleId] AS Id,[RoleName] AS Name FROM [dbo].[SystemWebAdminRole]
		WHERE [EntityStatusId] = 1 AND 'SystemWebAdminRole' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )
		
		UNION ALL

		SELECT 'CrimeIncidentType' AS LookupName,[CrimeIncidentTypeId] AS Id,[CrimeIncidentTypeName] AS Name FROM [dbo].[CrimeIncidentType]
		WHERE [EntityStatusId] = 1 AND 'CrimeIncidentType' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )
		
		RETURN 0
        
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
/****** Object:  StoredProcedure [dbo].[usp_Reset]    Script Date: 1/24/2021 1:02:57 PM ******/
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
		
		DELETE FROM [dbo].[SystemWebAdminUserRole];
		DELETE FROM [dbo].[SystemWebAdminMenuRole];
		DELETE FROM [dbo].[SystemUserContact];
		DELETE FROM [dbo].[SystemUserConfig];
		DELETE FROM [dbo].[SystemWebAdminRole];
		DELETE FROM [dbo].[SystemUser];
		DELETE FROM [dbo].[LegalGeoAddress];
		DELETE FROM [dbo].[LegalEntity];
		DELETE FROM [dbo].[CrimeIncidentCategory];
		DELETE FROM [dbo].[CrimeIncidentType];
		DELETE FROM [dbo].[File];
		
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemWebAdminUserRole';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemWebAdminMenuRole';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemUserContact';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemUserConfig';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemWebAdminRole';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemUser';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'LegalGeoAddress';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'LegalEntity';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'CrimeIncidentCategory';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'CrimeIncidentType';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'File';
        
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
/****** Object:  StoredProcedure [dbo].[usp_sequence_getNextCode]    Script Date: 1/24/2021 1:02:57 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_add]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_add]
	@SystemUserTypeId		NVARCHAR(100),
	@LegalEntityId		NVARCHAR(100),
	@ProfilePictureFile	NVARCHAR(100),
	@UserName			NVARCHAR(100),
	@Password			NVARCHAR(100),
	@CreatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @SystemUserId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'SystemUser', @Id = @SystemUserId OUTPUT

		INSERT INTO [dbo].[SystemUser](
			[SystemUserId], 
			[SystemUserTypeId],
			[LegalEntityId],
			[ProfilePictureFile],
			[UserName],
			[Password],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@SystemUserId,
			@SystemUserTypeId,
			@LegalEntityId,
			@ProfilePictureFile,
			@UserName,
			HashBytes('MD5', @Password),
			@CreatedBy,
			GETDATE(),
			1
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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_delete]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_delete]
	@SystemUserId		NVARCHAR(100),
	@LastUpdatedBy		NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[SystemUser] WHERE [SystemUserId] = @SystemUserId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[SystemUser]
			SET 
			[UserName] = @SystemUserId + ' - ' + [UserName] + '(DELETED - ' + CONVERT(VARCHAR(50),GETDATE())+ ')',
			EntityStatusId = 2,
			[LastUpdatedBy] = @LastUpdatedBy,
			[LastUpdatedAt] = GETDATE()
			WHERE [SystemUserId] = @SystemUserId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getByCredentials]    Script Date: 1/24/2021 1:02:57 PM ******/
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
		
		DECLARE @SystemUserId			NVARCHAR(100);
		DECLARE @SystemUserTypeId		BIGINT;
		DECLARE @LegalEntityId			NVARCHAR(100);
		DECLARE @ProfilePictureFile		NVARCHAR(100);
		DECLARE @GenderId				BIGINT;
		DECLARE @CreatedBy				NVARCHAR(100);
		DECLARE @CreatedAt				DATETIME;
		DECLARE @LastUpdatedBy			NVARCHAR(100);
		DECLARE @LastUpdatedAt			DATETIME;
		DECLARE @EntityStatusId			BIGINT;
		
		SELECT 
		@SystemUserId = su.[SystemUserId], 
		@SystemUserTypeId = su.[SystemUserTypeId],
		@ProfilePictureFile = su.[ProfilePictureFile],
		@LegalEntityId = su.[LegalEntityId],
		@ProfilePictureFile = su.[ProfilePictureFile],
		@CreatedBy = su.[CreatedBy],
		@CreatedAt = su.[CreatedAt],
		@LastUpdatedBy = su.[LastUpdatedBy],
		@LastUpdatedAt = su.[LastUpdatedAt]
		FROM [dbo].[SystemUser] su
		WHERE su.[UserName] = @Username 
		AND su.[Password] = HashBytes('MD5', @Password)
		AND su.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[SystemUser] AS su
		WHERE su.[SystemUserId] = @SystemUserId
		AND su.[EntityStatusId] = 1;

		SELECT  
		sut.[SystemUserTypeId],
		sut.[SystemUserTypeName]
		FROM [dbo].[SystemUserType] AS sut
		WHERE sut.[SystemUserTypeId] = @SystemUserTypeId;
		
		SELECT  *
		FROM [dbo].[File] AS f
		WHERE f.[FileId] = @ProfilePictureFile
		AND f.[EntityStatusId] = 1;

		SELECT  
		@GenderId = le.[GenderId]
		FROM [dbo].[LegalEntity] AS le
		WHERE le.[LegalEntityId] = @LegalEntityId;

		SELECT  
		le.*
		FROM [dbo].[LegalEntity] AS le
		WHERE le.[LegalEntityId] = @LegalEntityId;

		SELECT  
		eg.[GenderId],
		eg.[GenderName]
		FROM [dbo].[EntityGender] AS eg
		WHERE eg.[GenderId] = @GenderId;

		exec [dbo].[usp_systemwebadminuserroles_getBySystemUserId] @SystemUserId

		SELECT 
		@CreatedBy AS CreatedBy,
		@CreatedAt AS CreatedAt,
		[dbo].[svf_getUserFullName](@CreatedBy) AS CreatedByFullName,
		@LastUpdatedBy AS LastUpdatedBy,
		@LastUpdatedAt AS LastUpdatedAt,
		[dbo].[svf_getUserFullName](@LastUpdatedBy) AS LastUpdatedByFullName

		SELECT  
		es.[EntityStatusId],
		es.[EntityStatusName]
		FROM [dbo].[EntityStatus] AS es
		WHERE es.[EntityStatusId] = @EntityStatusId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getByID]    Script Date: 1/24/2021 1:02:57 PM ******/
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
		
		DECLARE @SystemUserTypeId		BIGINT;
		DECLARE @LegalEntityId			NVARCHAR(100);
		DECLARE @ProfilePictureFile		NVARCHAR(100);
		DECLARE @GenderId				BIGINT;
		DECLARE @CreatedBy				NVARCHAR(100);
		DECLARE @CreatedAt				DATETIME;
		DECLARE @LastUpdatedBy			NVARCHAR(100);
		DECLARE @LastUpdatedAt			DATETIME;
		DECLARE @EntityStatusId			BIGINT;
		
		SELECT 
		@SystemUserTypeId = su.[SystemUserTypeId],
		@LegalEntityId = su.[LegalEntityId],
		@ProfilePictureFile = su.[ProfilePictureFile],
		@CreatedBy = su.[CreatedBy],
		@CreatedAt = su.[CreatedAt],
		@LastUpdatedBy = su.[LastUpdatedBy],
		@LastUpdatedAt = su.[LastUpdatedAt],
		@EntityStatusId = su.[EntityStatusId]
		FROM [dbo].[SystemUser] su
		WHERE su.[SystemUserId] = @SystemUserId
		AND su.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[SystemUser] AS su
		WHERE su.[SystemUserId] = @SystemUserId
		AND su.[EntityStatusId] = 1;

		SELECT  
		sut.[SystemUserTypeId],
		sut.[SystemUserTypeName]
		FROM [dbo].[SystemUserType] AS sut
		WHERE sut.[SystemUserTypeId] = @SystemUserTypeId;
		
		SELECT  *
		FROM [dbo].[File] AS f
		WHERE f.[FileId] = @ProfilePictureFile
		AND f.[EntityStatusId] = 1;

		SELECT  
		@GenderId = le.[GenderId]
		FROM [dbo].[LegalEntity] AS le
		WHERE le.[LegalEntityId] = @LegalEntityId;

		SELECT  
		le.*
		FROM [dbo].[LegalEntity] AS le
		WHERE le.[LegalEntityId] = @LegalEntityId;

		SELECT  
		eg.[GenderId],
		eg.[GenderName]
		FROM [dbo].[EntityGender] AS eg
		WHERE eg.[GenderId] = @GenderId;

		exec [dbo].[usp_systemwebadminuserroles_getBySystemUserId] @SystemUserId

		SELECT 
		@CreatedBy AS CreatedBy,
		@CreatedAt AS CreatedAt,
		[dbo].[svf_getUserFullName](@CreatedBy) AS CreatedByFullName,
		@LastUpdatedBy AS LastUpdatedBy,
		@LastUpdatedAt AS LastUpdatedAt,
		[dbo].[svf_getUserFullName](@LastUpdatedBy) AS LastUpdatedByFullName

		SELECT  
		es.[EntityStatusId],
		es.[EntityStatusName]
		FROM [dbo].[EntityStatus] AS es
		WHERE es.[EntityStatusId] = @EntityStatusId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getPaged]    Script Date: 1/24/2021 1:02:57 PM ******/
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
	@Search			NVARCHAR(50) = '',
	@SystemUserType BIGINT = 1,
	@PageNo			BIGINT = 1,
	@PageSize		BIGINT = 10,
	@OrderColumn	NVARCHAR(100),
	@OrderDir		NVARCHAR(5)
AS
BEGIN
	SET NOCOUNT ON;

	
		WITH DATA_CTE
		AS
		(
			Select tableSource.*,
			(CASE @OrderDir
			 WHEN 'asc' THEN
				CASE @OrderColumn 
					WHEN 'SystemUserId' THEN ROW_NUMBER() OVER(ORDER BY [SystemUserId] ASC)
					WHEN 'UserName' THEN ROW_NUMBER() OVER(ORDER BY [UserName] ASC)
					WHEN 'LegalEntity.FullName' THEN ROW_NUMBER() OVER(ORDER BY [FullName] ASC)
					WHEN 'LegalEntity.Gender.GenderName' THEN ROW_NUMBER() OVER(ORDER BY [GenderName] ASC)
					WHEN 'LegalEntity.Age' THEN ROW_NUMBER() OVER(ORDER BY [Age] ASC)
					WHEN 'LegalEntity.EmailAddress' THEN ROW_NUMBER() OVER(ORDER BY [EmailAddress] ASC)
					WHEN 'LegalEntity.MobileNumber' THEN ROW_NUMBER() OVER(ORDER BY [MobileNumber] ASC)
				END
			WHEN 'desc' THEN
				CASE @OrderColumn 
					WHEN 'SystemUserId' THEN ROW_NUMBER() OVER(ORDER BY [SystemUserId] DESC)
					WHEN 'UserName' THEN ROW_NUMBER() OVER(ORDER BY [UserName] DESC)
					WHEN 'LegalEntity.FullName' THEN ROW_NUMBER() OVER(ORDER BY [FullName] DESC)
					WHEN 'LegalEntity.Gender.GenderName' THEN ROW_NUMBER() OVER(ORDER BY [GenderName] DESC)
					WHEN 'LegalEntity.Age' THEN ROW_NUMBER() OVER(ORDER BY [Age] DESC)
					WHEN 'LegalEntity.EmailAddress' THEN ROW_NUMBER() OVER(ORDER BY [EmailAddress] DESC)
					WHEN 'LegalEntity.MobileNumber' THEN ROW_NUMBER() OVER(ORDER BY [MobileNumber] DESC)
				END
			 END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 su.[SystemUserId],
			 MAX(su.[UserName])[UserName],
			 MAX(su.[Password])[Password],
			 MAX(sut.[SystemUserTypeId])[SystemUserTypeId],
			 MAX(sut.[SystemUserTypeName])[SystemUserTypeName],
			 MAX(le.[LegalEntityId])[LegalEntityId],
			 MAX(le.[FullName])[FullName],
			 MAX(le.[BirthDate])[BirthDate],
			 MAX(le.[Age])[Age],
			 MAX(le.[EmailAddress])[EmailAddress],
			 MAX(le.[MobileNumber])[MobileNumber],
			 MAX(eg.[GenderId])[GenderId],
			 MAX(eg.[GenderName])[GenderName]
			FROM [dbo].[SystemUser] AS su
			LEFT JOIN (SELECT * FROM [dbo].[SystemWebAdminUserRole] WHERE [EntityStatusId] = 1) sur ON su.SystemUserId = sur.SystemUserId
			LEFT JOIN (SELECT * FROM [dbo].[SystemWebAdminRole] WHERE [EntityStatusId] = 1) sr ON sur.[SystemWebAdminRoleId] = sr.[SystemWebAdminRoleId]
			LEFT JOIN (SELECT *,ISNULL([FirstName],'') + ' ' + ISNULL([MiddleName],'') + ' ' + ISNULL([LastName],'') AS [FullName] FROM [dbo].[LegalEntity] WHERE EntityStatusId = 1) AS le ON su.LegalEntityId = le.LegalEntityId
			LEFT JOIN [dbo].[SystemUserType] AS sut ON su.SystemUserTypeId = sut.SystemUserTypeId
			LEFT JOIN [dbo].[EntityGender] AS eg ON le.GenderId = eg.GenderId
			WHERE su.EntityStatusId = 1
			AND su.SystemUserTypeId = @SystemUserType
			AND (su.SystemUserId like '%' + @Search + '%' 
			OR su.[UserName] like '%' + @Search + '%' 
			OR sr.[RoleName] like '%' + @Search + '%' 
			OR le.[FullName] like '%' + @Search + '%' 
			OR eg.[GenderName] like '%' + @Search + '%'
			OR le.[Age] like '%' + @Search + '%' 
			OR le.[EmailAddress] like '%' + @Search + '%'
			OR le.[MobileNumber] like '%' + @Search + '%' )
			GROUP BY su.SystemUserId
			) tableSource
		)
		SELECT 
		src.*,
		sur.[SystemWebAdminUserRoleId],
		sr.[SystemWebAdminRoleId],
		sr.[RoleName],
		src.row_num,
		src.TotalRows
		FROM DATA_CTE src
		 
		LEFT JOIN (SELECT * FROM [dbo].[SystemWebAdminUserRole] WHERE [EntityStatusId] = 1) sur ON src.SystemUserId = sur.SystemUserId
		LEFT JOIN (SELECT * FROM [dbo].[SystemWebAdminRole] WHERE [EntityStatusId] = 1) sr ON sur.[SystemWebAdminRoleId] = sr.[SystemWebAdminRoleId]
		WHERE src.row_num between ((@PageNo - 1) * @PageSize + 1 ) 
		and (@PageNo * @PageSize)
		ORDER BY src.row_num 

END
GO
/****** Object:  StoredProcedure [dbo].[usp_systemuser_update]    Script Date: 1/24/2021 1:02:57 PM ******/
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
	@LastUpdatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[SystemUser]
		SET 
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_add]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminmenuroles_add]
	@SystemWebAdminMenuId	NVARCHAR(100),
	@IsAllowed				BIT,
	@SystemWebAdminRoleId	NVARCHAR(100),
	@CreatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @SystemWebAdminMenuRoleId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'SystemWebAdminMenuRole', @Id = @SystemWebAdminMenuRoleId OUTPUT

		INSERT INTO [dbo].[SystemWebAdminMenuRole](
			[SystemWebAdminMenuRoleId],
			[SystemWebAdminMenuId],
			[SystemWebAdminRoleId],
			[IsAllowed],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@SystemWebAdminMenuRoleId,
			@SystemWebAdminMenuId,
			@SystemWebAdminRoleId,
			@IsAllowed,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @SystemWebAdminMenuRoleId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_delete]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminmenuroles_delete]
	@SystemWebAdminMenuRoleId	NVARCHAR(100),
	@LastUpdatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[SystemWebAdminMenuRole] WHERE [SystemWebAdminMenuRoleId] = @SystemWebAdminMenuRoleId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[SystemWebAdminMenuRole]
			SET 
			EntityStatusId = 2,
			LastUpdatedBy = @LastUpdatedBy,
			LastUpdatedAt = GETDATE()
			WHERE [SystemWebAdminMenuRoleId] = @SystemWebAdminMenuRoleId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_getBySystemWebAdminMenuIdAndSystemWebAdminRoleId]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminmenuroles_getBySystemWebAdminMenuIdAndSystemWebAdminRoleId]
	@SystemWebAdminMenuId	BIGINT,
	@SystemWebAdminRoleId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @SystemWebAdminMenuRoleId	NVARCHAR(100);
		DECLARE @CreatedBy			NVARCHAR(100);
		DECLARE @CreatedAt			DATETIME;
		DECLARE @LastUpdatedBy		NVARCHAR(100);
		DECLARE @LastUpdatedAt		DATETIME;
		DECLARE @EntityStatusId		BIGINT;
		
		SELECT 
		@SystemWebAdminMenuRoleId = swamr.[SystemWebAdminMenuRoleId],
		@SystemWebAdminRoleId = swamr.[SystemWebAdminRoleId],
		@SystemWebAdminMenuId = swamr.[SystemWebAdminMenuId],
		@CreatedBy = swamr.[CreatedBy],
		@CreatedAt = swamr.[CreatedAt],
		@LastUpdatedBy = swamr.[LastUpdatedBy],
		@LastUpdatedAt = swamr.[LastUpdatedAt],
		@EntityStatusId = swamr.[EntityStatusId]
		FROM [dbo].[SystemWebAdminMenuRole] swamr
		WHERE swamr.[SystemWebAdminRoleId] = @SystemWebAdminRoleId
		AND swamr.[SystemWebAdminMenuId] = @SystemWebAdminMenuId
		AND swamr.[EntityStatusId] = 1;
		
		SELECT *
		FROM [dbo].[SystemWebAdminMenuRole] swamr
		WHERE swamr.[SystemWebAdminMenuRoleId] = @SystemWebAdminMenuRoleId
		AND swamr.[EntityStatusId] = 1;

		SELECT *
		FROM [dbo].[SystemWebAdminRole] AS swar
		WHERE swar.[SystemWebAdminRoleId] = @SystemWebAdminRoleId;
		
		SELECT *
		FROM [dbo].[SystemWebAdminMenu] AS swam
		WHERE swam.[SystemWebAdminMenuId] = @SystemWebAdminMenuId;

		SELECT 
		@CreatedBy AS CreatedBy,
		@CreatedAt AS CreatedAt,
		[dbo].[svf_getUserFullName](@CreatedBy) AS CreatedByFullName,
		@LastUpdatedBy AS LastUpdatedBy,
		@LastUpdatedAt AS LastUpdatedAt,
		[dbo].[svf_getUserFullName](@LastUpdatedBy) AS LastUpdatedByFullName

		SELECT  
		es.[EntityStatusId],
		es.[EntityStatusName]
		FROM [dbo].[EntityStatus] AS es
		WHERE es.[EntityStatusId] = @EntityStatusId;
		
		RETURN 0
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_getBySystemWebAdminRoleId]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminmenuroles_getBySystemWebAdminRoleId]
	@SystemWebAdminRoleId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		SELECT 
		swamr.SystemWebAdminMenuRoleId,
		swamr.IsAllowed,
		swam.*,
		swar.*
		FROM [dbo].[SystemWebAdminMenuRole] swamr
		LEFT JOIN [dbo].[SystemWebAdminRole] swar ON swamr.[SystemWebAdminRoleId] = swar.[SystemWebAdminRoleId]
		LEFT JOIN [dbo].[SystemWebAdminMenu] swam ON swamr.[SystemWebAdminMenuId] = swam.[SystemWebAdminMenuId]
		WHERE swamr.[SystemWebAdminRoleId] = @SystemWebAdminRoleId
		AND swamr.[EntityStatusId] = 1;
		
		RETURN 0
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_getBySystemWebAdminRoleIdandSystemWebAdminModuleId]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminmenuroles_getBySystemWebAdminRoleIdandSystemWebAdminModuleId]
	@SystemWebAdminRoleId	NVARCHAR(100),
	@SystemWebAdminModuleId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		SELECT 
		MAX(systemWebAdminMenuRole.[SystemWebAdminMenuRoleId])[SystemWebAdminMenuRoleId], 
		MAX(systemWebAdminMenuRole.[IsAllowed])[IsAllowed], 
		systemWebAdminMenuRole.[SystemWebAdminMenuId], 
		MAX(systemWebAdminMenuRole.[SystemWebAdminMenuName])[SystemWebAdminMenuName], 
		MAX(systemWebAdminMenuRole.[SystemWebAdminModuleId])[SystemWebAdminModuleId], 
		MAX(systemWebAdminMenuRole.[SystemWebAdminModuleName])[SystemWebAdminModuleName], 
		MAX(systemWebAdminMenuRole.[SystemWebAdminRoleId])[SystemWebAdminRoleId], 
		MAX(systemWebAdminMenuRole.[RoleName])[RoleName]
		FROM (
			SELECT 
			swamr.[SystemWebAdminMenuRoleId],
			swamr.[IsAllowed],
			swam.[SystemWebAdminMenuId],
			swam.[SystemWebAdminMenuName],
			swamod.[SystemWebAdminModuleId],
			swamod.[SystemWebAdminModuleName],
			swar.[SystemWebAdminRoleId],
			swar.[RoleName]
			FROM [dbo].[SystemWebAdminMenuRole] swamr
			LEFT JOIN [dbo].[SystemWebAdminRole] swar ON swamr.[SystemWebAdminRoleId] = swar.[SystemWebAdminRoleId]
			LEFT JOIN [dbo].[SystemWebAdminMenu] swam ON swamr.[SystemWebAdminMenuId] = swam.[SystemWebAdminMenuId]
			LEFT JOIN [dbo].[SystemWebAdminModule] swamod ON swam.[SystemWebAdminModuleId] = swamod.[SystemWebAdminModuleId]
			WHERE swamr.[SystemWebAdminRoleId] = @SystemWebAdminRoleId
			AND swam.SystemWebAdminModuleId = @SystemWebAdminModuleId
			AND swamr.[EntityStatusId] = 1

			UNION ALL

			SELECT 
			CAST(swam.[SystemWebAdminMenuId] AS NVARCHAR(100)) AS [SystemWebAdminMenuRoleId],
			0 [IsAllowed],
			swam.[SystemWebAdminMenuId],
			swam.[SystemWebAdminMenuName],
			swamod.[SystemWebAdminModuleId],
			swamod.[SystemWebAdminModuleName],
			'' [SystemWebAdminRoleId],
			'' [RoleName]
			FROM [dbo].[SystemWebAdminMenu] swam
			LEFT JOIN [dbo].[SystemWebAdminModule] swamod ON swam.[SystemWebAdminModuleId] = swamod.[SystemWebAdminModuleId]
			WHERE swam.SystemWebAdminModuleId = @SystemWebAdminModuleId
		) systemWebAdminMenuRole
		GROUP BY systemWebAdminMenuRole.SystemWebAdminMenuId
		RETURN 0
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_update]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminmenuroles_update]
	@SystemWebAdminMenuRoleId		NVARCHAR(100),
	@IsAllowed						BIT,
	@LastUpdatedBy					NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[SystemWebAdminMenuRole]
		SET 
		[IsAllowed] = @IsAllowed,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [SystemWebAdminMenuRoleId] = @SystemWebAdminMenuRoleId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminrole_add]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminrole_add]
	@RoleName			NVARCHAR(100),
	@CreatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @SystemWebAdminRoleId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'SystemWebAdminRole', @Id = @SystemWebAdminRoleId OUTPUT

		INSERT INTO [dbo].[SystemWebAdminRole](
			[SystemWebAdminRoleId],
			[RoleName],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@SystemWebAdminRoleId,
			@RoleName,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @SystemWebAdminRoleId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminrole_delete]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminrole_delete]
	@SystemWebAdminRoleId		NVARCHAR(100),
	@LastUpdatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[SystemWebAdminRole] WHERE [SystemWebAdminRoleId] = @SystemWebAdminRoleId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[SystemWebAdminRole]
			SET 
			[RoleName] = @SystemWebAdminRoleId + ' - ' + [RoleName] + '(DELETED - ' + CONVERT(VARCHAR(50),GETDATE())+ ')',
			EntityStatusId = 2,
			[LastUpdatedBy] = @LastUpdatedBy,
			[LastUpdatedAt] = GETDATE()
			WHERE [SystemWebAdminRoleId] = @SystemWebAdminRoleId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminrole_getByID]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminrole_getByID]
	@SystemWebAdminRoleId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @CreatedBy			NVARCHAR(100);
		DECLARE @CreatedAt			DATETIME;
		DECLARE @LastUpdatedBy		NVARCHAR(100);
		DECLARE @LastUpdatedAt		DATETIME;
		DECLARE @EntityStatusId		BIGINT;
		
		SELECT 
		@CreatedBy = swar.[CreatedBy],
		@CreatedAt = swar.[CreatedAt],
		@LastUpdatedBy = swar.[LastUpdatedBy],
		@LastUpdatedAt = swar.[LastUpdatedAt],
		@EntityStatusId = swar.[EntityStatusId]
		FROM [dbo].[SystemWebAdminRole] swar
		WHERE swar.[SystemWebAdminRoleId] = @SystemWebAdminRoleId
		AND swar.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[SystemWebAdminRole] AS swar
		WHERE swar.[SystemWebAdminRoleId] = @SystemWebAdminRoleId
		AND swar.[EntityStatusId] = 1;

		SELECT 
		@CreatedBy AS CreatedBy,
		@CreatedAt AS CreatedAt,
		[dbo].[svf_getUserFullName](@CreatedBy) AS CreatedByFullName,
		@LastUpdatedBy AS LastUpdatedBy,
		@LastUpdatedAt AS LastUpdatedAt,
		[dbo].[svf_getUserFullName](@LastUpdatedBy) AS LastUpdatedByFullName

		SELECT  
		es.[EntityStatusId],
		es.[EntityStatusName]
		FROM [dbo].[EntityStatus] AS es
		WHERE es.[EntityStatusId] = @EntityStatusId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminrole_getPaged]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- Description:	filter contract list by location
-- =============================================
CREATE PROCEDURE [dbo].[usp_systemwebadminrole_getPaged]
	@Search			NVARCHAR(50) = '',
	@PageNo			BIGINT = 1,
	@PageSize		BIGINT = 10,
	@OrderColumn	NVARCHAR(100),
	@OrderDir		NVARCHAR(5)
AS
BEGIN
	SET NOCOUNT ON;

	
		WITH DATA_CTE
		AS
		(
			Select tableSource.*, 
			(CASE @OrderDir
			 WHEN 'asc' THEN
				CASE @OrderColumn 
					WHEN 'SystemWebAdminRoleId' THEN ROW_NUMBER() OVER(ORDER BY [SystemWebAdminRoleId] ASC)
					WHEN 'RoleName' THEN ROW_NUMBER() OVER(ORDER BY [RoleName] ASC)
				END
			WHEN 'desc' THEN
				CASE @OrderColumn 
					WHEN 'SystemWebAdminRoleId' THEN ROW_NUMBER() OVER(ORDER BY [SystemWebAdminRoleId] DESC)
					WHEN 'RoleName' THEN ROW_NUMBER() OVER(ORDER BY [RoleName] DESC)
				END
			 END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 swar.[SystemWebAdminRoleId],
			 MAX(swar.[RoleName])[RoleName]
			FROM [dbo].[SystemWebAdminRole] AS swar
			WHERE swar.EntityStatusId = 1
			AND (swar.[SystemWebAdminRoleId] like '%' + @Search + '%' OR swar.RoleName like '%' + @Search + '%')
			GROUP BY swar.[SystemWebAdminRoleId]
			) tableSource
		)
		SELECT 
		src.*
		 FROM DATA_CTE src
		WHERE src.row_num between ((@PageNo - 1) * @PageSize + 1 ) 
		and (@PageNo * @PageSize)
		ORDER BY src.row_num 

END

GO
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminrole_update]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminrole_update]
	@SystemWebAdminRoleId		NVARCHAR(100),
	@RoleName				NVARCHAR(100),
	@LastUpdatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[SystemWebAdminRole]
		SET 
		[RoleName] = @RoleName,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [SystemWebAdminRoleId] = @SystemWebAdminRoleId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_add]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminuserroles_add]
	@SystemUserId			NVARCHAR(100),
	@SystemWebAdminRoleId	NVARCHAR(100),
	@CreatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @SystemWebAdminUserRoleId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'SystemWebAdminUserRole', @Id = @SystemWebAdminUserRoleId OUTPUT

		INSERT INTO [dbo].[SystemWebAdminUserRole](
			[SystemWebAdminUserRoleId],
			[SystemUserId],
			[SystemWebAdminRoleId],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@SystemWebAdminUserRoleId,
			@SystemUserId,
			@SystemWebAdminRoleId,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @SystemWebAdminUserRoleId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_delete]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminuserroles_delete]
	@SystemWebAdminUserRoleId	NVARCHAR(100),
	@LastUpdatedBy	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[SystemWebAdminUserRole] WHERE [SystemWebAdminUserRoleId] = @SystemWebAdminUserRoleId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[SystemWebAdminUserRole]
			SET 
			EntityStatusId = 2,
			LastUpdatedBy = @LastUpdatedBy,
			LastUpdatedAt = GETDATE()
			WHERE [SystemWebAdminUserRoleId] = @SystemWebAdminUserRoleId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_getBySystemUserId]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminuserroles_getBySystemUserId]
	@SystemUserId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		SELECT 
		swaur.[SystemWebAdminUserRoleId],
		swar.*
		FROM [dbo].[SystemWebAdminUserRole] swaur
		LEFT JOIN [dbo].[SystemWebAdminRole] swar ON swaur.[SystemWebAdminRoleId] = swar.[SystemWebAdminRoleId]
		WHERE swaur.[SystemUserId] = @SystemUserId
		AND swaur.[EntityStatusId] = 1;
		
		RETURN 0
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_getBySystemWebAdminRoleIdAndSystemUserId]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminuserroles_getBySystemWebAdminRoleIdAndSystemUserId]
	@SystemWebAdminRoleId	NVARCHAR(100),
	@SystemUserId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @SystemWebAdminUserRoleId	NVARCHAR(100);
		DECLARE @CreatedBy			NVARCHAR(100);
		DECLARE @CreatedAt			DATETIME;
		DECLARE @LastUpdatedBy		NVARCHAR(100);
		DECLARE @LastUpdatedAt		DATETIME;
		DECLARE @EntityStatusId		BIGINT;
		
		SELECT 
		@SystemWebAdminUserRoleId = swaur.[SystemWebAdminUserRoleId],
		@SystemUserId = swaur.[SystemUserId],
		@SystemWebAdminRoleId = swaur.[SystemWebAdminRoleId],
		@CreatedBy = swaur.[CreatedBy],
		@CreatedAt = swaur.[CreatedAt],
		@LastUpdatedBy = swaur.[LastUpdatedBy],
		@LastUpdatedAt = swaur.[LastUpdatedAt],
		@EntityStatusId = swaur.[EntityStatusId]
		FROM [dbo].[SystemWebAdminUserRole] swaur
		WHERE swaur.[SystemUserId] = @SystemUserId
		AND swaur.[SystemWebAdminRoleId] = @SystemWebAdminRoleId
		AND swaur.[EntityStatusId] = 1;
		
		SELECT *
		FROM [dbo].[SystemWebAdminUserRole] swaur
		WHERE swaur.[SystemWebAdminUserRoleId] = @SystemWebAdminUserRoleId
		AND swaur.[EntityStatusId] = 1;

		SELECT *
		FROM [dbo].[SystemWebAdminRole] AS swar
		WHERE swar.[SystemWebAdminRoleId] = @SystemWebAdminRoleId;
		
		SELECT *
		FROM [dbo].[SystemUser] AS su
		WHERE su.[SystemUserId] = @SystemUserId;

		SELECT 
		@CreatedBy AS CreatedBy,
		@CreatedAt AS CreatedAt,
		[dbo].[svf_getUserFullName](@CreatedBy) AS CreatedByFullName,
		@LastUpdatedBy AS LastUpdatedBy,
		@LastUpdatedAt AS LastUpdatedAt,
		[dbo].[svf_getUserFullName](@LastUpdatedBy) AS LastUpdatedByFullName

		SELECT  
		es.[EntityStatusId],
		es.[EntityStatusName]
		FROM [dbo].[EntityStatus] AS es
		WHERE es.[EntityStatusId] = @EntityStatusId;
		
		RETURN 0
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_getBySystemWebAdminUserRoleId]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminuserroles_getBySystemWebAdminUserRoleId]
	@SystemWebAdminUserRoleId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @SystemUsersId	NVARCHAR(100);
		DECLARE @SystemWebAdminRoleId	NVARCHAR(100);
		DECLARE @CreatedBy			NVARCHAR(100);
		DECLARE @CreatedAt			DATETIME;
		DECLARE @LastUpdatedBy		NVARCHAR(100);
		DECLARE @LastUpdatedAt		DATETIME;
		DECLARE @EntityStatusId		BIGINT;
		
		SELECT 
		@SystemUsersId = swaur.[SystemUserId],
		@SystemWebAdminRoleId = swaur.[SystemWebAdminRoleId],
		@CreatedBy = swaur.[CreatedBy],
		@CreatedAt = swaur.[CreatedAt],
		@LastUpdatedBy = swaur.[LastUpdatedBy],
		@LastUpdatedAt = swaur.[LastUpdatedAt],
		@EntityStatusId = swaur.[EntityStatusId]
		FROM [dbo].[SystemWebAdminUserRole] swaur
		WHERE swaur.[SystemWebAdminUserRoleId] = @SystemWebAdminUserRoleId
		AND swaur.[EntityStatusId] = 1;
		
		SELECT *
		FROM [dbo].[SystemWebAdminUserRole] swaur
		WHERE swaur.[SystemWebAdminUserRoleId] = @SystemWebAdminUserRoleId
		AND swaur.[EntityStatusId] = 1;

		SELECT *
		FROM [dbo].[SystemWebAdminRole] AS swar
		WHERE swar.[SystemWebAdminRoleId] = @SystemWebAdminRoleId;
		
		SELECT *
		FROM [dbo].[SystemUser] AS su
		WHERE su.[SystemUserId] = @SystemUsersId;

		SELECT 
		@CreatedBy AS CreatedBy,
		@CreatedAt AS CreatedAt,
		[dbo].[svf_getUserFullName](@CreatedBy) AS CreatedByFullName,
		@LastUpdatedBy AS LastUpdatedBy,
		@LastUpdatedAt AS LastUpdatedAt,
		[dbo].[svf_getUserFullName](@LastUpdatedBy) AS LastUpdatedByFullName

		SELECT  
		es.[EntityStatusId],
		es.[EntityStatusName]
		FROM [dbo].[EntityStatus] AS es
		WHERE es.[EntityStatusId] = @EntityStatusId;
		
		RETURN 0
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_getPaged]    Script Date: 1/24/2021 1:02:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- Description:	filter contract list by location
-- =============================================
CREATE PROCEDURE [dbo].[usp_systemwebadminuserroles_getPaged]
	@SystemUserId	NVARCHAR(100) = '',
	@Search			NVARCHAR(50) = '',
	@PageNo			BIGINT = 1,
	@PageSize		BIGINT = 10,
	@OrderColumn	NVARCHAR(100),
	@OrderDir		NVARCHAR(5)
AS
BEGIN
	SET NOCOUNT ON;

	
		WITH DATA_CTE
		AS
		(
			Select tableSource.*, 
			(CASE @OrderDir
			 WHEN 'asc' THEN
				CASE @OrderColumn 
					WHEN 'SystemWebAdminUserRoleId' THEN ROW_NUMBER() OVER(ORDER BY [SystemWebAdminUserRoleId] ASC)
					WHEN 'SystemRole.RoleName' THEN ROW_NUMBER() OVER(ORDER BY [RoleName] ASC)
				END
			WHEN 'desc' THEN
				CASE @OrderColumn 
					WHEN 'SystemWebAdminUserRoleId' THEN ROW_NUMBER() OVER(ORDER BY [SystemWebAdminUserRoleId] DESC)
					WHEN 'SystemRole.RoleName' THEN ROW_NUMBER() OVER(ORDER BY [RoleName] DESC)
				END
			 END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 swaur.[SystemWebAdminUserRoleId],
			 MAX(swar.[SystemWebAdminRoleId])[SystemWebAdminRoleId],
			 MAX(swar.RoleName)[RoleName]
			FROM [dbo].[SystemWebAdminUserRole] AS swaur
			LEFT JOIN [dbo].[SystemWebAdminRole] swar ON swaur.[SystemWebAdminRoleId] = swar.[SystemWebAdminRoleId]
			WHERE swaur.EntityStatusId = 1 AND swaur.SystemUserId = @SystemUserId
			AND (swar.RoleName like '%' + @Search + '%')
			GROUP BY swaur.[SystemWebAdminUserRoleId]
			) tableSource
		)
		SELECT 
		src.*
		 FROM DATA_CTE src
		WHERE src.row_num between ((@PageNo - 1) * @PageSize + 1 ) 
		and (@PageNo * @PageSize)
		ORDER BY src.row_num 

END

GO
