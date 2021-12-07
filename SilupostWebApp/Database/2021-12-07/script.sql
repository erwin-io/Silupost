USE [silupostbase]
GO
/****** Object:  UserDefinedFunction [dbo].[GetDistanceInKilometersByCoordinates]    Script Date: 12/7/2021 1:28:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		
-- Create date: Sept 25, 2020
-- =============================================
CREATE FUNCTION [dbo].[GetDistanceInKilometersByCoordinates]
(
	@LatitudeOrigin float,
	@LongitudeOrigin float,
	@LatitudeDistination float,
	@LongitudeDistination float
)
RETURNS float
AS
BEGIN
	-- CONSTANTS
	DECLARE @EarthRadiusInKM float,
	@PI float,
	@LatitudeOriginRadians float,
	@LongitudeOriginRadians float,
	@LatitudeDistinationRadians float,
	@LongitudeDistinationRadians float,
	@Distance decimal;

	SET @EarthRadiusInKM = 6371;
	SET @PI = PI();
	
	-- RADIANS conversion
	SET @LatitudeOriginRadians = @LatitudeOrigin * @PI / 180;
	SET @LongitudeOriginRadians = @LongitudeOrigin * @PI / 180;
	SET @LatitudeDistinationRadians = @LatitudeDistination * @PI / 180;
	SET @LongitudeDistinationRadians = @LongitudeDistination * @PI / 180;
	SET @Distance = ROUND(Acos(
	Cos(@LatitudeOriginRadians) * Cos(@LongitudeOriginRadians) * Cos(@LatitudeDistinationRadians) * Cos(@LongitudeDistinationRadians) +
	Cos(@LatitudeOriginRadians) * Sin(@LongitudeOriginRadians) * Cos(@LatitudeDistinationRadians) * Sin(@LongitudeDistinationRadians) +
	Sin(@LatitudeOriginRadians) * Sin(@LatitudeDistinationRadians)
	) * @EarthRadiusInKM, 2);
	RETURN @Distance;
END

GO
/****** Object:  UserDefinedFunction [dbo].[LPAD]    Script Date: 12/7/2021 1:28:33 PM ******/
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
/****** Object:  UserDefinedFunction [dbo].[svf_getUserFullName]    Script Date: 12/7/2021 1:28:33 PM ******/
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
/****** Object:  UserDefinedFunction [dbo].[tvf_SplitString]    Script Date: 12/7/2021 1:28:33 PM ******/
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
/****** Object:  Table [dbo].[CrimeIncidentCategory]    Script Date: 12/7/2021 1:28:33 PM ******/
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
/****** Object:  Table [dbo].[CrimeIncidentReport]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CrimeIncidentReport](
	[CrimeIncidentReportId] [nvarchar](100) NOT NULL,
	[CrimeIncidentCategoryId] [nvarchar](100) NOT NULL,
	[PostedBySystemUserId] [nvarchar](100) NOT NULL,
	[DateReported] [datetime] NOT NULL,
	[PossibleDate] [date] NOT NULL,
	[PossibleTime] [time](7) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[GeoStreet] [nvarchar](250) NULL,
	[GeoDistrict] [nvarchar](100) NULL,
	[GeoCityMun] [nvarchar](100) NULL,
	[GeoProvince] [nvarchar](100) NULL,
	[GeoCountry] [nvarchar](100) NULL,
	[GeoTrackerLatitude] [float] NOT NULL,
	[GeoTrackerLongitude] [float] NOT NULL,
	[ApprovalStatusId] [bigint] NOT NULL,
	[IsReviewActionEnable] [bit] NOT NULL,
	[IsReviewCommentEnable] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_CrimeIncidentReport] PRIMARY KEY CLUSTERED 
(
	[CrimeIncidentReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CrimeIncidentReportMedia]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CrimeIncidentReportMedia](
	[CrimeIncidentReportMediaId] [nvarchar](100) NOT NULL,
	[DocReportMediaTypeId] [bigint] NOT NULL,
	[FileId] [nvarchar](100) NOT NULL,
	[CrimeIncidentReportId] [nvarchar](100) NOT NULL,
	[Caption] [nvarchar](250) NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_CrimeIncidentReportMedia] PRIMARY KEY CLUSTERED 
(
	[CrimeIncidentReportMediaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CrimeIncidentType]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[DocReportMediaType]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocReportMediaType](
	[DocReportMediaTypeId] [bigint] NOT NULL,
	[DocReportMediaTypeName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ReportMediaType] PRIMARY KEY CLUSTERED 
(
	[DocReportMediaTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnforcementReportValidation]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnforcementReportValidation](
	[EnforcementReportValidationId] [nvarchar](100) NOT NULL,
	[CrimeIncidentReportId] [nvarchar](100) NOT NULL,
	[EnforcementUnitId] [nvarchar](100) NOT NULL,
	[DateSubmitted] [datetime] NOT NULL,
	[ReportNotes] [nvarchar](max) NULL,
	[ValidationNotes] [nvarchar](max) NULL,
	[ReportValidationStatusId] [bigint] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_EnforcementReportValidation] PRIMARY KEY CLUSTERED 
(
	[EnforcementReportValidationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnforcementStation]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnforcementStation](
	[EnforcementStationId] [nvarchar](100) NOT NULL,
	[EnforcementStationName] [nvarchar](250) NOT NULL,
	[EnforcementStationGuestCode] [nvarchar](100) NOT NULL,
	[IconFileId] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_EnforcementStation] PRIMARY KEY CLUSTERED 
(
	[EnforcementStationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnforcementType]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnforcementType](
	[EnforcementTypeId] [nvarchar](100) NOT NULL,
	[EnforcementTypeName] [nvarchar](250) NOT NULL,
	[IconFileId] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_EnforcementType] PRIMARY KEY CLUSTERED 
(
	[EnforcementTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnforcementUnit]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnforcementUnit](
	[EnforcementUnitId] [nvarchar](100) NOT NULL,
	[EnforcementTypeId] [nvarchar](100) NOT NULL,
	[EnforcementStationId] [nvarchar](100) NOT NULL,
	[LegalEntityId] [nvarchar](100) NOT NULL,
	[ProfilePictureFile] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_EnforcementUnit] PRIMARY KEY CLUSTERED 
(
	[EnforcementUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityApprovalStatus]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityApprovalStatus](
	[ApprovalStatusId] [bigint] NOT NULL,
	[ApprovalStatusName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_EntityApprovalStatus] PRIMARY KEY CLUSTERED 
(
	[ApprovalStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityGender]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[EntityStatus]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[File]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[File](
	[FileId] [nvarchar](100) NOT NULL,
	[FileName] [nvarchar](250) NOT NULL,
	[MimeType] [nvarchar](100) NOT NULL,
	[FileSize] [bigint] NOT NULL,
	[FileContent] [varbinary](max) NULL,
	[IsFromStorage] [bit] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedAt] [datetime] NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LegalEntity]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LegalEntity](
	[LegalEntityId] [nvarchar](100) NOT NULL,
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
/****** Object:  Table [dbo].[LegalEntityAddress]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LegalEntityAddress](
	[LegalEntityAddressId] [nvarchar](100) NOT NULL,
	[LegalEntityId] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_LegalGeoAddress] PRIMARY KEY CLUSTERED 
(
	[LegalEntityAddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportValidationStatus]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportValidationStatus](
	[ReportValidationStatusId] [bigint] NOT NULL,
	[ReportValidationStatusName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ReportValidationStatus] PRIMARY KEY CLUSTERED 
(
	[ReportValidationStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sequence]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[SystemConfig]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[SystemConfigType]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[SystemToken]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemToken](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TokenId] [nvarchar](max) NOT NULL,
	[SystemUserId] [nvarchar](100) NOT NULL,
	[ClientId] [nvarchar](250) NOT NULL,
	[Subject] [nvarchar](250) NOT NULL,
	[IssuedUtc] [datetime] NOT NULL,
	[ExpiresUtc] [datetime] NOT NULL,
	[ProtectedTicket] [nvarchar](max) NOT NULL,
	[TokenType] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_SystemToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemUser]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUser](
	[SystemUserId] [nvarchar](100) NOT NULL,
	[SystemUserTypeId] [bigint] NOT NULL,
	[LegalEntityId] [nvarchar](100) NOT NULL,
	[ProfilePictureFile] [nvarchar](100) NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[EnforcementStationId] [nvarchar](100) NULL,
	[IsEnforcementUnit] [bit] NOT NULL,
	[EnforcementUnitId] [nvarchar](100) NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[IsWebAdminGuestUser] [bit] NULL,
	[HasFirstLogin] [bit] NULL,
	[LasteDateTimeActive] [datetime] NULL,
	[LasteDateTimeLogin] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[SystemUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemUserConfig]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUserConfig](
	[SystemUserConfigId] [nvarchar](100) NOT NULL,
	[SystemUserId] [nvarchar](100) NOT NULL,
	[IsUserEnable] [bit] NOT NULL,
	[IsUserAllowToPostNextReport] [bit] NOT NULL,
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
/****** Object:  Table [dbo].[SystemUserContact]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[SystemUserType]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[SystemUserVerification]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUserVerification](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[VerificationSender] [nvarchar](100) NOT NULL,
	[VerificationTypeId] [bigint] NOT NULL,
	[VerificationCode] [nvarchar](50) NOT NULL,
	[IsVerified] [bit] NOT NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_SystemUserVerification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemUserVerificationType]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemUserVerificationType](
	[VerificationTypeId] [bigint] NOT NULL,
	[VerificationTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SystemUserVerificationType] PRIMARY KEY CLUSTERED 
(
	[VerificationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemWebAdminMenu]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemWebAdminMenu](
	[SystemWebAdminMenuId] [bigint] NOT NULL,
	[SystemWebAdminModuleId] [bigint] NOT NULL,
	[SystemWebAdminMenuName] [nvarchar](100) NOT NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_SystemWebAdminMenu] PRIMARY KEY CLUSTERED 
(
	[SystemWebAdminMenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemWebAdminMenuRole]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[SystemWebAdminModule]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[SystemWebAdminPrivileges]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemWebAdminPrivileges](
	[SystemWebAdminPrivilegeId] [bigint] NOT NULL,
	[SystemWebAdminPrivilegeName] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_SystemWebAdminPrivileges] PRIMARY KEY CLUSTERED 
(
	[SystemWebAdminPrivilegeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemWebAdminRole]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Table [dbo].[SystemWebAdminRolePrivileges]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemWebAdminRolePrivileges](
	[SystemWebAdminRolePrivilegeId] [nvarchar](100) NOT NULL,
	[SystemWebAdminRoleId] [nvarchar](100) NOT NULL,
	[SystemWebAdminPrivilegeId] [bigint] NOT NULL,
	[IsAllowed] [bit] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
	[LastUpdatedAt] [datetime] NULL,
	[EntityStatusId] [bigint] NOT NULL,
 CONSTRAINT [PK_SystemWebAdminRolePrivileges] PRIMARY KEY CLUSTERED 
(
	[SystemWebAdminRolePrivilegeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemWebAdminUserRole]    Script Date: 12/7/2021 1:28:34 PM ******/
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
INSERT [dbo].[DocReportMediaType] ([DocReportMediaTypeId], [DocReportMediaTypeName]) VALUES (1, N'Image')
GO
INSERT [dbo].[DocReportMediaType] ([DocReportMediaTypeId], [DocReportMediaTypeName]) VALUES (2, N'Video')
GO
INSERT [dbo].[DocReportMediaType] ([DocReportMediaTypeId], [DocReportMediaTypeName]) VALUES (3, N'Audio')
GO
INSERT [dbo].[EntityApprovalStatus] ([ApprovalStatusId], [ApprovalStatusName]) VALUES (1, N'Approved')
GO
INSERT [dbo].[EntityApprovalStatus] ([ApprovalStatusId], [ApprovalStatusName]) VALUES (2, N'Declined')
GO
INSERT [dbo].[EntityApprovalStatus] ([ApprovalStatusId], [ApprovalStatusName]) VALUES (3, N'Pending')
GO
INSERT [dbo].[EntityGender] ([GenderId], [GenderName]) VALUES (1, N'Male')
GO
INSERT [dbo].[EntityGender] ([GenderId], [GenderName]) VALUES (2, N'Female')
GO
INSERT [dbo].[EntityStatus] ([EntityStatusId], [EntityStatusName]) VALUES (1, N'Active')
GO
INSERT [dbo].[EntityStatus] ([EntityStatusId], [EntityStatusName]) VALUES (2, N'Deleted')
GO
INSERT [dbo].[ReportValidationStatus] ([ReportValidationStatusId], [ReportValidationStatusName]) VALUES (1, N'Validated')
GO
INSERT [dbo].[ReportValidationStatus] ([ReportValidationStatusId], [ReportValidationStatusName]) VALUES (2, N'Rejected')
GO
INSERT [dbo].[ReportValidationStatus] ([ReportValidationStatusId], [ReportValidationStatusName]) VALUES (3, N'Pending')
GO
INSERT [dbo].[ReportValidationStatus] ([ReportValidationStatusId], [ReportValidationStatusName]) VALUES (4, N'Canceled')
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
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (12, N'LegalEntityAddress', N'LEA-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (13, N'EnforcementType', N'ET-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (14, N'EnforcementStation', N'ES-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (15, N'EnforcementUnit', N'EU-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (16, N'CrimeIncidentReport', N'CIR-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (17, N'CrimeIncidentReportMedia', N'CIRM-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (18, N'EnforcementReportValidation', N'ERV-', 10, 0)
GO
INSERT [dbo].[Sequence] ([SequenceId], [TableName], [Prefix], [Length], [LastNumber]) VALUES (19, N'SystemWebAdminRolePrivileges', N'SWARP-', 10, 0)
GO
INSERT [dbo].[SystemConfig] ([SystemConfigId], [ConfigName], [ConfigGroup], [ConfigKey], [ConfigValue], [SystemConfigTypeId], [IsUserConfigurable]) VALUES (1, N'System Version', N'System Version', N'SYSTEM_VERSION', N'1', 2, 0)
GO
INSERT [dbo].[SystemConfig] ([SystemConfigId], [ConfigName], [ConfigGroup], [ConfigKey], [ConfigValue], [SystemConfigTypeId], [IsUserConfigurable]) VALUES (2, N'API Version', N'System Version', N'API_VERSION', N'1', 2, 0)
GO
INSERT [dbo].[SystemConfig] ([SystemConfigId], [ConfigName], [ConfigGroup], [ConfigKey], [ConfigValue], [SystemConfigTypeId], [IsUserConfigurable]) VALUES (3, N'Android App Version', N'System Version', N'ANDROID_APP_BUILD_VERSION', N'1', 2, 0)
GO
INSERT [dbo].[SystemConfig] ([SystemConfigId], [ConfigName], [ConfigGroup], [ConfigKey], [ConfigValue], [SystemConfigTypeId], [IsUserConfigurable]) VALUES (4, N'Allow Mobile User Guest Membership', N'System Configuration', N'ALLOW_MOBILE_MEMBERSHIP', N'true', 2, 1)
GO
INSERT [dbo].[SystemConfigType] ([SystemConfigTypeId], [ValueType]) VALUES (1, N'BOOLEAN')
GO
INSERT [dbo].[SystemConfigType] ([SystemConfigTypeId], [ValueType]) VALUES (2, N'TEXT')
GO
INSERT [dbo].[SystemUserType] ([SystemUserTypeId], [SystemUserTypeName]) VALUES (1, N'WebAppAdmin')
GO
INSERT [dbo].[SystemUserType] ([SystemUserTypeId], [SystemUserTypeName]) VALUES (2, N'MobileAppUser')
GO
INSERT [dbo].[SystemUserVerificationType] ([VerificationTypeId], [VerificationTypeName]) VALUES (1, N'MobileNumber')
GO
INSERT [dbo].[SystemUserVerificationType] ([VerificationTypeId], [VerificationTypeName]) VALUES (2, N'Email')
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (1, 1, N'Dashboard', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (2, 2, N'Report Tracker', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (3, 3, N'Crime Incident Report', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (5, 4, N'Mobile Controller', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (6, 5, N'System Configuration', 2)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (7, 5, N'Crime Incident Type', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (8, 5, N'Crime Incident Category', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (9, 5, N'Enforcement Type', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (10, 5, N'Enforcement Unit', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (11, 5, N'Enforcement Station', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (12, 6, N'System Role', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (13, 6, N'System User', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (14, 6, N'System Menu Roles', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (15, 3, N'Enforcement Report Validation', 1)
GO
INSERT [dbo].[SystemWebAdminMenu] ([SystemWebAdminMenuId], [SystemWebAdminModuleId], [SystemWebAdminMenuName], [EntityStatusId]) VALUES (16, 6, N'System Role Privileges', 1)
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (1, N'Dashboard')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (4, N'Mobile Controller')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (3, N'Report Library')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (2, N'Report Tracker')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (6, N'System Admin Security')
GO
INSERT [dbo].[SystemWebAdminModule] ([SystemWebAdminModuleId], [SystemWebAdminModuleName]) VALUES (5, N'System Setup')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (1, N'Allowed to validate other enforcement''s pending report validation')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (2, N'Allowed to approve crime/incident report')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (3, N'Allowed to decline crime/incident report')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (4, N'Allowed to validate enforcement report validation')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (5, N'Allowed to reject enforcement report validation')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (6, N'Allowed to cancel enforcement report validation')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (7, N'Allowed to add user')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (8, N'Allowed to update user')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (9, N'Allowed to delete user')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (10, N'Allowed to add system web admin role')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (11, N'Allowed to update system web admin role')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (12, N'Allowed to delete system web admin role')
GO
INSERT [dbo].[SystemWebAdminPrivileges] ([SystemWebAdminPrivilegeId], [SystemWebAdminPrivilegeName]) VALUES (13, N'Allowed to update system web admin menu role')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_CrimeIncidentCategory]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[CrimeIncidentCategory] ADD  CONSTRAINT [UK_CrimeIncidentCategory] UNIQUE NONCLUSTERED 
(
	[CrimeIncidentTypeId] ASC,
	[CrimeIncidentCategoryName] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_CrimeIncidentReportMedia]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[CrimeIncidentReportMedia] ADD  CONSTRAINT [UK_CrimeIncidentReportMedia] UNIQUE NONCLUSTERED 
(
	[CrimeIncidentReportId] ASC,
	[CrimeIncidentReportMediaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_CrimeIncidentType]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[CrimeIncidentType] ADD  CONSTRAINT [UK_CrimeIncidentType] UNIQUE NONCLUSTERED 
(
	[CrimeIncidentTypeName] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_EnforcementStation]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[EnforcementStation] ADD  CONSTRAINT [UK_EnforcementStation] UNIQUE NONCLUSTERED 
(
	[EnforcementStationName] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_EnforcementType]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[EnforcementType] ADD  CONSTRAINT [UK_EnforcementType] UNIQUE NONCLUSTERED 
(
	[EnforcementTypeName] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_EnforcementUnit]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[EnforcementUnit] ADD  CONSTRAINT [UK_EnforcementUnit] UNIQUE NONCLUSTERED 
(
	[EnforcementStationId] ASC,
	[LegalEntityId] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [U_Sequence]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[Sequence] ADD  CONSTRAINT [U_Sequence] UNIQUE NONCLUSTERED 
(
	[TableName] ASC,
	[Prefix] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_SystemUser]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [UK_SystemUser] UNIQUE NONCLUSTERED 
(
	[UserName] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_SystemWebAdminMenu]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[SystemWebAdminMenu] ADD  CONSTRAINT [UK_SystemWebAdminMenu] UNIQUE NONCLUSTERED 
(
	[SystemWebAdminModuleId] ASC,
	[SystemWebAdminMenuName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_SystemWebAdminMenuRole]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  Index [UK_SystemWebAdminModule]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[SystemWebAdminModule] ADD  CONSTRAINT [UK_SystemWebAdminModule] UNIQUE NONCLUSTERED 
(
	[SystemWebAdminModuleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UK_SystemRole]    Script Date: 12/7/2021 1:28:34 PM ******/
ALTER TABLE [dbo].[SystemWebAdminRole] ADD  CONSTRAINT [UK_SystemRole] UNIQUE NONCLUSTERED 
(
	[RoleName] ASC,
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CrimeIncidentReport] ADD  CONSTRAINT [DF_CrimeIncidentReport_DateReported]  DEFAULT (getdate()) FOR [DateReported]
GO
ALTER TABLE [dbo].[CrimeIncidentReport] ADD  CONSTRAINT [DF_CrimeIncidentReport_PossibleDate]  DEFAULT (getdate()) FOR [PossibleDate]
GO
ALTER TABLE [dbo].[CrimeIncidentReport] ADD  CONSTRAINT [DF_CrimeIncidentReport_GeoTrackerLatitude]  DEFAULT ((0)) FOR [GeoTrackerLatitude]
GO
ALTER TABLE [dbo].[CrimeIncidentReport] ADD  CONSTRAINT [DF_CrimeIncidentReport_GeoTrackerLongitude]  DEFAULT ((0)) FOR [GeoTrackerLongitude]
GO
ALTER TABLE [dbo].[CrimeIncidentReport] ADD  CONSTRAINT [DF_CrimeIncidentReport_ApprovalStatusId]  DEFAULT ((3)) FOR [ApprovalStatusId]
GO
ALTER TABLE [dbo].[CrimeIncidentReport] ADD  CONSTRAINT [DF_CrimeIncidentReport_IsReviewActionEnable]  DEFAULT ((0)) FOR [IsReviewActionEnable]
GO
ALTER TABLE [dbo].[CrimeIncidentReport] ADD  CONSTRAINT [DF_CrimeIncidentReport_IsReviewCommentEnable]  DEFAULT ((0)) FOR [IsReviewCommentEnable]
GO
ALTER TABLE [dbo].[CrimeIncidentReport] ADD  CONSTRAINT [DF_CrimeIncidentReport_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[CrimeIncidentReport] ADD  CONSTRAINT [DF_CrimeIncidentReport_EntityStatusId]  DEFAULT ((0)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[EnforcementReportValidation] ADD  CONSTRAINT [DF_EnforcementReportValidation_DateSubmitted]  DEFAULT (getdate()) FOR [DateSubmitted]
GO
ALTER TABLE [dbo].[EnforcementReportValidation] ADD  CONSTRAINT [DF_EnforcementReportValidation_ReportValidationStatusId]  DEFAULT ((3)) FOR [ReportValidationStatusId]
GO
ALTER TABLE [dbo].[EnforcementStation] ADD  CONSTRAINT [DF_EnforcementStation_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[EnforcementType] ADD  CONSTRAINT [DF_EnforcementType_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[EnforcementType] ADD  CONSTRAINT [DF_EnforcementType_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[EnforcementUnit] ADD  CONSTRAINT [DF_EnforcementUnit_EntityStatusId]  DEFAULT ((0)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[File] ADD  CONSTRAINT [DF_File_FileSize]  DEFAULT ((0)) FOR [FileSize]
GO
ALTER TABLE [dbo].[File] ADD  CONSTRAINT [DF_File_IsFromStorage]  DEFAULT ((0)) FOR [IsFromStorage]
GO
ALTER TABLE [dbo].[File] ADD  CONSTRAINT [DF_File_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[File] ADD  CONSTRAINT [DF_File_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[LegalEntity] ADD  CONSTRAINT [DF_LegalEntity_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[LegalEntityAddress] ADD  CONSTRAINT [DF_LegalGeoAddress_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[Sequence] ADD  CONSTRAINT [DF_Sequence_SequenceLength]  DEFAULT ((0)) FOR [Length]
GO
ALTER TABLE [dbo].[Sequence] ADD  CONSTRAINT [DF_Sequence_LastNumber]  DEFAULT ((0)) FOR [LastNumber]
GO
ALTER TABLE [dbo].[SystemConfig] ADD  CONSTRAINT [DF_SystemConfig_IsUserConfigurable]  DEFAULT ((1)) FOR [IsUserConfigurable]
GO
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_IsEnforcementUnit]  DEFAULT ((0)) FOR [IsEnforcementUnit]
GO
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_IsWebAdminGuestUser]  DEFAULT ((0)) FOR [IsWebAdminGuestUser]
GO
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_HasFirstLogin]  DEFAULT ((0)) FOR [HasFirstLogin]
GO
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF_SystemUser_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
GO
ALTER TABLE [dbo].[SystemUserConfig] ADD  CONSTRAINT [DF_SystemUserConfig_IsUserEnable]  DEFAULT ((1)) FOR [IsUserEnable]
GO
ALTER TABLE [dbo].[SystemUserConfig] ADD  CONSTRAINT [DF_SystemUserConfig_IsUserAllowToPostNextReport]  DEFAULT ((1)) FOR [IsUserAllowToPostNextReport]
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
ALTER TABLE [dbo].[SystemUserVerification] ADD  CONSTRAINT [DF_SystemUserVerification_IsVerifed]  DEFAULT ((0)) FOR [IsVerified]
GO
ALTER TABLE [dbo].[SystemWebAdminMenu] ADD  CONSTRAINT [DF_SystemWebAdminMenu_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
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
ALTER TABLE [dbo].[SystemWebAdminRolePrivileges] ADD  CONSTRAINT [DF_SystemWebAdminRolePrivileges_IsAllowed]  DEFAULT ((0)) FOR [IsAllowed]
GO
ALTER TABLE [dbo].[SystemWebAdminRolePrivileges] ADD  CONSTRAINT [DF_SystemWebAdminRolePrivileges_EntityStatusId]  DEFAULT ((1)) FOR [EntityStatusId]
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
ALTER TABLE [dbo].[CrimeIncidentReport]  WITH CHECK ADD  CONSTRAINT [FK_CrimeIncidentReport_CrimeIncidentCategory] FOREIGN KEY([CrimeIncidentCategoryId])
REFERENCES [dbo].[CrimeIncidentCategory] ([CrimeIncidentCategoryId])
GO
ALTER TABLE [dbo].[CrimeIncidentReport] CHECK CONSTRAINT [FK_CrimeIncidentReport_CrimeIncidentCategory]
GO
ALTER TABLE [dbo].[CrimeIncidentReport]  WITH CHECK ADD  CONSTRAINT [FK_CrimeIncidentReport_EntityApprovalStatus] FOREIGN KEY([ApprovalStatusId])
REFERENCES [dbo].[EntityApprovalStatus] ([ApprovalStatusId])
GO
ALTER TABLE [dbo].[CrimeIncidentReport] CHECK CONSTRAINT [FK_CrimeIncidentReport_EntityApprovalStatus]
GO
ALTER TABLE [dbo].[CrimeIncidentReport]  WITH CHECK ADD  CONSTRAINT [FK_CrimeIncidentReport_SystemUser] FOREIGN KEY([PostedBySystemUserId])
REFERENCES [dbo].[SystemUser] ([SystemUserId])
GO
ALTER TABLE [dbo].[CrimeIncidentReport] CHECK CONSTRAINT [FK_CrimeIncidentReport_SystemUser]
GO
ALTER TABLE [dbo].[CrimeIncidentReportMedia]  WITH CHECK ADD  CONSTRAINT [FK_CrimeIncidentReportMedia_CrimeIncidentReport] FOREIGN KEY([CrimeIncidentReportId])
REFERENCES [dbo].[CrimeIncidentReport] ([CrimeIncidentReportId])
GO
ALTER TABLE [dbo].[CrimeIncidentReportMedia] CHECK CONSTRAINT [FK_CrimeIncidentReportMedia_CrimeIncidentReport]
GO
ALTER TABLE [dbo].[CrimeIncidentReportMedia]  WITH CHECK ADD  CONSTRAINT [FK_CrimeIncidentReportMedia_DocReportMediaType] FOREIGN KEY([DocReportMediaTypeId])
REFERENCES [dbo].[DocReportMediaType] ([DocReportMediaTypeId])
GO
ALTER TABLE [dbo].[CrimeIncidentReportMedia] CHECK CONSTRAINT [FK_CrimeIncidentReportMedia_DocReportMediaType]
GO
ALTER TABLE [dbo].[EnforcementReportValidation]  WITH CHECK ADD  CONSTRAINT [FK_EnforcementReportValidation_CrimeIncidentReport] FOREIGN KEY([CrimeIncidentReportId])
REFERENCES [dbo].[CrimeIncidentReport] ([CrimeIncidentReportId])
GO
ALTER TABLE [dbo].[EnforcementReportValidation] CHECK CONSTRAINT [FK_EnforcementReportValidation_CrimeIncidentReport]
GO
ALTER TABLE [dbo].[EnforcementReportValidation]  WITH CHECK ADD  CONSTRAINT [FK_EnforcementReportValidation_EnforcementUnit] FOREIGN KEY([EnforcementUnitId])
REFERENCES [dbo].[EnforcementUnit] ([EnforcementUnitId])
GO
ALTER TABLE [dbo].[EnforcementReportValidation] CHECK CONSTRAINT [FK_EnforcementReportValidation_EnforcementUnit]
GO
ALTER TABLE [dbo].[EnforcementReportValidation]  WITH CHECK ADD  CONSTRAINT [FK_EnforcementReportValidation_ReportValidationStatus] FOREIGN KEY([ReportValidationStatusId])
REFERENCES [dbo].[ReportValidationStatus] ([ReportValidationStatusId])
GO
ALTER TABLE [dbo].[EnforcementReportValidation] CHECK CONSTRAINT [FK_EnforcementReportValidation_ReportValidationStatus]
GO
ALTER TABLE [dbo].[SystemUser]  WITH CHECK ADD  CONSTRAINT [FK_SystemUser_LegalEntity] FOREIGN KEY([LegalEntityId])
REFERENCES [dbo].[LegalEntity] ([LegalEntityId])
GO
ALTER TABLE [dbo].[SystemUser] CHECK CONSTRAINT [FK_SystemUser_LegalEntity]
GO
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_add]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
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
		IF EXISTS(SELECT * FROM [dbo].[CrimeIncidentReport] WHERE [CrimeIncidentCategoryId] = @CrimeIncidentCategoryId AND [EntityStatusId] = 1)
		BEGIN
			RAISERROR('Cannot delete CrimeIncidentCategory, data is used in CrimeIncidentReport', 16, 1)
		END

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_getAll]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- =============================================
CREATE PROCEDURE [dbo].[usp_crimeincidentcategory_getAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
	cic.[CrimeIncidentCategoryId],
	MAX(cic.[CrimeIncidentCategoryName])[CrimeIncidentCategoryName],
	MAX(cic.[CrimeIncidentCategoryDescription])[CrimeIncidentCategoryDescription],
	MAX(cit.[CrimeIncidentTypeId])[CrimeIncidentTypeId],
	MAX(cit.[CrimeIncidentTypeName])[CrimeIncidentTypeName],
	MAX(cit.[CrimeIncidentTypeDescription])[CrimeIncidentTypeDescription],
	MAX(citf.[FileId])[FileId],
	MAX(citf.[FileName])[FileName],
	MAX(citf.[FileContent])[FileContent]
	FROM [dbo].[CrimeIncidentCategory] AS cic
	LEFT JOIN [dbo].[CrimeIncidentType] cit ON cic.CrimeIncidentTypeId = cit.CrimeIncidentTypeId
	LEFT JOIN [dbo].[File] citf ON cit.[IconFileId] = citf.[FileId]
	WHERE cic.EntityStatusId = 1
	GROUP BY cic.CrimeIncidentCategoryId

END


GO
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_getByID]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_getPaged]    Script Date: 12/7/2021 1:28:34 PM ******/
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
	@OrderColumn			NVARCHAR(100) = 'CrimeIncidentCategoryId',
	@OrderDir				NVARCHAR(5) = 'ASC'
AS
BEGIN
	SET NOCOUNT ON;

	
		WITH DATA_CTE
		AS
		(
			Select tableSource.*, 
			(CASE IIF(ISNULL(@OrderDir, 'asc') = '', 'asc', @OrderDir)
			 WHEN 'asc' THEN
				CASE IIF(ISNULL(@OrderColumn, 'CrimeIncidentCategoryId') = '', 'CrimeIncidentCategoryId', @OrderColumn) 
					WHEN 'CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] ASC)
					WHEN 'CrimeIncidentCategoryDescription' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryDescription] ASC)
					ELSE ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryId] ASC)
				END
			WHEN 'desc' THEN
				CASE IIF(ISNULL(@OrderColumn, 'CrimeIncidentCategoryId') = '', 'CrimeIncidentCategoryId', @OrderColumn) 
					WHEN 'CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] DESC)
					WHEN 'CrimeIncidentCategoryDescription' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryDescription] DESC)
					ELSE ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryId] DESC)
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentcategory_update]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreport_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreport_add]
	@CrimeIncidentCategoryId	NVARCHAR(100),
	@PostedBySystemUserId		NVARCHAR(100),
	@DateReported				DATETIME,
	@PossibleDate				DATE,
	@PossibleTime				TIME(7),
	@Description				NVARCHAR(MAX),
	@GeoStreet					NVARCHAR(250),
	@GeoDistrict				NVARCHAR(100),
	@GeoCityMun					NVARCHAR(100),
	@GeoProvince				NVARCHAR(100),
	@GeoCountry					NVARCHAR(100),
	@GeoTrackerLatitude			FLOAT,
	@GeoTrackerLongitude		FLOAT,
	@IsReviewActionEnable		BIT,
	@IsReviewCommentEnable		BIT,
	@CreatedBy					NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @CrimeIncidentReportId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'CrimeIncidentReport', @Id = @CrimeIncidentReportId OUTPUT

		INSERT INTO [dbo].[CrimeIncidentReport](
			[CrimeIncidentReportId],
			[CrimeIncidentCategoryId],
			[PostedBySystemUserId],
			[DateReported],
			[PossibleDate],
			[PossibleTime],
			[Description],
			[GeoStreet],
			[GeoDistrict],
			[GeoCityMun],
			[GeoProvince],
			[GeoCountry],
			[GeoTrackerLatitude],
			[GeoTrackerLongitude],
			[IsReviewActionEnable],
			[IsReviewCommentEnable],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@CrimeIncidentReportId,
			@CrimeIncidentCategoryId,
			@PostedBySystemUserId,
			@DateReported,
			@PossibleDate,
			@PossibleTime,
			@Description,
			@GeoStreet,
			@GeoDistrict,
			@GeoCityMun,
			@GeoProvince,
			@GeoCountry,
			@GeoTrackerLatitude,
			@GeoTrackerLongitude,
			@IsReviewActionEnable,
			@IsReviewCommentEnable,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @CrimeIncidentReportId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreport_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreport_delete]
	@CrimeIncidentReportId		NVARCHAR(100),
	@LastUpdatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[CrimeIncidentReport] WHERE [CrimeIncidentReportId] = @CrimeIncidentReportId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[CrimeIncidentReport]
			SET 
			EntityStatusId = 2,
			[LastUpdatedBy] = @LastUpdatedBy,
			[LastUpdatedAt] = GETDATE()
			WHERE [CrimeIncidentReportId] = @CrimeIncidentReportId;

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreport_getByID]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreport_getByID]
	@CrimeIncidentReportId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @CrimeIncidentCategoryId	NVARCHAR(100);
		DECLARE @CrimeIncidentTypeId		NVARCHAR(100);
		DECLARE @PostedBySystemUserId		NVARCHAR(100);
		DECLARE @LegalEntityId				NVARCHAR(100);
		DECLARE @ApprovalStatusId			BIGINT;
		DECLARE @CreatedBy					NVARCHAR(100);
		DECLARE @CreatedAt					DATETIME;
		DECLARE @LastUpdatedBy				NVARCHAR(100);
		DECLARE @LastUpdatedAt				DATETIME;
		DECLARE @EntityStatusId				BIGINT;
		DECLARE @Validated					BIT;
		
		SELECT 
		@CrimeIncidentCategoryId = cir.[CrimeIncidentCategoryId],
		@PostedBySystemUserId = cir.[PostedBySystemUserId],
		@ApprovalStatusId = cir.[ApprovalStatusId],
		@CreatedBy = cir.[CreatedBy],
		@CreatedAt = cir.[CreatedAt],
		@LastUpdatedBy = cir.[LastUpdatedBy],
		@LastUpdatedAt = cir.[LastUpdatedAt],
		@EntityStatusId = cir.[EntityStatusId]
		FROM [dbo].[CrimeIncidentReport] cir
		WHERE cir.[CrimeIncidentReportId] = @CrimeIncidentReportId
		AND cir.[EntityStatusId] = 1;

		SELECT @Validated = COUNT(*) FROM [dbo].[EnforcementReportValidation] WHERE [CrimeIncidentReportId] = @CrimeIncidentReportId AND [ReportValidationStatusId] = 1;

		SELECT 
		cir.[CrimeIncidentReportId],
		cir.[CrimeIncidentCategoryId],
		cir.[PostedBySystemUserId],
		cir.[DateReported],
		cir.[PossibleDate],
		--CAST(cir.[PossibleTime] AS NVARCHAR(10))[PossibleTime],
		RIGHT(Convert(VARCHAR(20), [PossibleTime],100),7)[PossibleTime],
		cir.[Description],
		CONCAT(ISNULL(cir.[GeoStreet], ''), ', ',ISNULL(cir.[GeoDistrict], ''), ', ',ISNULL(cir.[GeoCityMun], ''), ', ',ISNULL(cir.[GeoProvince], ''), ', ',ISNULL(cir.[GeoCountry], ''))[GeoAddress],
		cir.[GeoStreet],
		cir.[GeoDistrict],
		cir.[GeoCityMun],
		cir.[GeoProvince],
		cir.[GeoCountry],
		cir.[GeoTrackerLatitude],
		cir.[GeoTrackerLongitude],
		cir.[ApprovalStatusId],
		cir.[IsReviewActionEnable],
		cir.[IsReviewCommentEnable],
		@Validated AS Validated,
		cir.[CreatedBy],
		cir.[CreatedAt],
		cir.[LastUpdatedBy],
		cir.[LastUpdatedAt],
		cir.[EntityStatusId]
		FROM [dbo].[CrimeIncidentReport] AS cir
		WHERE cir.[CrimeIncidentReportId] = @CrimeIncidentReportId
		AND cir.[EntityStatusId] = 1;
		
		SELECT @CrimeIncidentTypeId = cic.[CrimeIncidentTypeId]
		FROM [dbo].[CrimeIncidentCategory] AS cic
		WHERE cic.[CrimeIncidentCategoryId] = @CrimeIncidentCategoryId
		AND cic.[EntityStatusId] = 1;
		
		SELECT *
		FROM [dbo].[CrimeIncidentCategory] AS cic
		WHERE cic.[CrimeIncidentCategoryId] = @CrimeIncidentCategoryId
		AND cic.[EntityStatusId] = 1;
		
		SELECT *
		FROM [dbo].[CrimeIncidentType] AS cit
		WHERE cit.[CrimeIncidentTypeId] = @CrimeIncidentTypeId
		AND cit.[EntityStatusId] = 1;

		SELECT @LegalEntityId = su.[LegalEntityId]
		FROM [dbo].[SystemUser] AS su
		WHERE su.[SystemUserId] = @PostedBySystemUserId
		AND su.[EntityStatusId] = 1;

		SELECT *
		FROM [dbo].[SystemUser] AS su
		WHERE su.[SystemUserId] = @PostedBySystemUserId
		AND su.[EntityStatusId] = 1;

		SELECT *,CONCAT(ISNULL(le.FirstName, ''), ' ',ISNULL(le.MiddleName, ''), ' ',ISNULL(le.LastName, '')) AS FullName
		FROM [dbo].[LegalEntity] AS le
		WHERE le.[LegalEntityId] = @LegalEntityId
		AND le.[EntityStatusId] = 1;
		
		SELECT *
		FROM [dbo].[EntityApprovalStatus] AS eas
		WHERE eas.[ApprovalStatusId] = @ApprovalStatusId

		exec [dbo].[usp_crimeincidentreportmedia_getByCrimeIncidentReportId] @CrimeIncidentReportId

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreport_getByTracker]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- =============================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreport_getByTracker]
	@TrackerRadiusInKM			DECIMAL,
	@TrackerPointLatitude		FLOAT,
	@TrackerPointLongitude		FLOAT,
	@ApprovalStatusId			BIGINT = 3,
	@CrimeIncidentCategoryIds	NVARCHAR(MAX) = '',
	@DateReportedFrom			DATETIME = GETDATE,
	@DateReportedTo				DATETIME = GETDATE,
	@PossibleDateFrom			DATE = GETDATE,
	@PossibleDateTo				DATE = GETDATE,
	@PossibleTimeFrom			TIME(7) = GETDATE,
	@PossibleTimeTo				TIME(7) = GETDATE
AS
BEGIN
	SET NOCOUNT ON;

	;WITH DATA_CTE
	AS
	(
		Select tableSource.*, ROW_NUMBER() OVER(ORDER BY [CrimeIncidentReportId] DESC) AS row_num ,
		count(*) over() as TotalRows
		FROM (
				SELECT 
				cir.[CrimeIncidentReportId],
				MAX(cir.[DateReported])[DateReported],
				MAX(cir.[PossibleDate])[PossibleDate],
				--MAX(CAST(cir.[PossibleTime] as NVARCHAR(10)))[PossibleTime],
				MAX(RIGHT(Convert(VARCHAR(20), cir.[PossibleTime],100),7))[PossibleTime],
				MAX(cir.[Description])[Description],
				MAX(CONCAT(ISNULL(cir.[GeoStreet], ''), ', ',ISNULL(cir.[GeoDistrict], ''), ', ',ISNULL(cir.[GeoCityMun], ''), ', ',ISNULL(cir.[GeoProvince], ''), ', ',ISNULL(cir.[GeoCountry], '')))[GeoAddress],
				MAX(cir.[GeoStreet])[GeoStreet],
				MAX(cir.[GeoDistrict])[GeoDistrict],
				MAX(cir.[GeoCityMun])[GeoCityMun],
				MAX(cir.[GeoProvince])[GeoProvince],
				MAX(cir.[GeoCountry])[GeoCountry],
				MAX(cir.[GeoTrackerLatitude])[GeoTrackerLatitude],
				MAX(cir.[GeoTrackerLongitude])[GeoTrackerLongitude],
				MAX(cic.[CrimeIncidentCategoryId])[CrimeIncidentCategoryId],
				MAX(cic.[CrimeIncidentCategoryName])[CrimeIncidentCategoryName],
				MAX(su.[SystemUserId])[SystemUserId],
				MAX(su.[SystemUserTypeId])[SystemUserTypeId],
				MAX(su.[UserName])[UserName],
				MAX(le.[LegalEntityId])[LegalEntityId],
				MAX(le.[FullName])[FullName],
				MAX(eas.[ApprovalStatusId])[ApprovalStatusId],
				MAX(eas.[ApprovalStatusName])[ApprovalStatusName]
				FROM
				(
					SELECT *,dbo.GetDistanceInKilometersByCoordinates(@TrackerPointLatitude,@TrackerPointLongitude,GeoTrackerLatitude,GeoTrackerLongitude)[DistanceInKilometers] FROM [dbo].[CrimeIncidentReport]
					WHERE EntityStatusId = 1 AND ApprovalStatusId = @ApprovalStatusId
				) cir
				LEFT JOIN [dbo].[CrimeIncidentCategory] cic ON cir.[CrimeIncidentCategoryId] = cic.[CrimeIncidentCategoryId]
				LEFT JOIN [dbo].[SystemUser] su ON cir.[PostedBySystemUserId] = su.[SystemUserId]
				LEFT JOIN (SELECT [LegalEntityId],CONCAT(ISNULL([FirstName], ''), ' ', ISNULL([MiddleName], ''), ' ', ISNULL([LastName], ''), ' ') AS [FullName] FROM [dbo].[LegalEntity] WHERE [EntityStatusId] = 1) le ON su.[LegalEntityId] = le.[LegalEntityId]
				LEFT JOIN [dbo].[EntityApprovalStatus] eas ON cir.[ApprovalStatusId] = eas.[ApprovalStatusId]
	
				WHERE 
				cir.[DistanceInKilometers] <= @TrackerRadiusInKM
				AND 
				cir.CrimeIncidentCategoryId IN (select Item from [dbo].[tvf_SplitString](@CrimeIncidentCategoryIds,','))
				AND CAST(cir.[DateReported] as date) BETWEEN @DateReportedFrom AND @DateReportedTo
				AND cir.[PossibleDate] BETWEEN @PossibleDateFrom AND @PossibleDateTo
				AND (CAST(@PossibleTimeTo as time) >= cir.[PossibleTime] AND CAST(@PossibleTimeFrom as time) <= cir.[PossibleTime])
				GROUP BY cir.[CrimeIncidentReportId]
		) tableSource
	)
	SELECT 
	src.* FROM DATA_CTE src
	ORDER BY src.row_num 

END
GO
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreport_getPaged]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- =============================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreport_getPaged]
	@Search						NVARCHAR(100) = '',
	@IsAdvanceSearchMode		BIT = 0,
	@ApprovalStatusId			BIGINT = 3,
	@CrimeIncidentReportId		NVARCHAR(100) = '',
	@CrimeIncidentCategoryName	NVARCHAR(100) = '',
	@PostedByFullName			NVARCHAR(250) = '',
	@DateReportedFrom			DATETIME = GETDATE,
	@DateReportedTo				DATETIME = GETDATE,
	@PossibleDateFrom			DATE = GETDATE,
	@PossibleDateTo				DATE = GETDATE,
	@PossibleTimeFrom			TIME(7) = GETDATE,
	@PossibleTimeTo				TIME(7) = GETDATE,
	@Description				NVARCHAR(250) = '',
	@GeoStreet					NVARCHAR(250) = '',
	@GeoDistrict				NVARCHAR(100) = '',
	@GeoCityMun					NVARCHAR(100) = '',
	@GeoProvince				NVARCHAR(100) = '',
	@GeoCountry					NVARCHAR(100) = '',
	@PageNo						BIGINT = 1,
	@PageSize					BIGINT = 10,
	@OrderColumn				NVARCHAR(100) = 'CrimeIncidentReportId',
	@OrderDir					NVARCHAR(5) = 'ASC'
AS
BEGIN
	SET NOCOUNT ON;

		WITH DATA_CTE
		AS
		(
			Select tableSource.*, 
			(CASE IIF(ISNULL(@OrderDir, 'asc') = '', 'asc', @OrderDir)
				WHEN 'asc' THEN
				CASE IIF(ISNULL(@OrderColumn, 'CrimeIncidentReportId') = '', 'CrimeIncidentReportId', @OrderColumn) 
					WHEN 'DateReported' THEN ROW_NUMBER() OVER(ORDER BY [DateReported] ASC)
					WHEN 'PossibleDate' THEN ROW_NUMBER() OVER(ORDER BY [PossibleDate] ASC)
					WHEN 'PossibleTime' THEN ROW_NUMBER() OVER(ORDER BY [PossibleTime] ASC)
					WHEN 'GeoAddress' THEN ROW_NUMBER() OVER(ORDER BY [GeoAddress] ASC)
					WHEN 'GeoStreet' THEN ROW_NUMBER() OVER(ORDER BY [GeoStreet] ASC)
					WHEN 'GeoDistrict' THEN ROW_NUMBER() OVER(ORDER BY [GeoDistrict] ASC)
					WHEN 'GeoCityMun' THEN ROW_NUMBER() OVER(ORDER BY [GeoCityMun] ASC)
					WHEN 'GeoProvince' THEN ROW_NUMBER() OVER(ORDER BY [GeoProvince] ASC)
					WHEN 'GeoCountry' THEN ROW_NUMBER() OVER(ORDER BY [GeoCountry] ASC)
					WHEN 'ApprovalStatus.ApprovalStatusName' THEN ROW_NUMBER() OVER(ORDER BY [ApprovalStatusName] ASC)
					WHEN 'CrimeIncidentCategory.CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] ASC)
					WHEN 'PostedBySystemUser.LegalEntity.FullName' THEN ROW_NUMBER() OVER(ORDER BY [FullName] ASC)
					ELSE ROW_NUMBER() OVER(ORDER BY [CrimeIncidentReportId] DESC)
				END
			ELSE
				CASE IIF(ISNULL(@OrderColumn, 'CrimeIncidentReportId') = '', 'CrimeIncidentReportId', @OrderColumn) 
					WHEN 'DateReported' THEN ROW_NUMBER() OVER(ORDER BY [DateReported] DESC)
					WHEN 'PossibleDate' THEN ROW_NUMBER() OVER(ORDER BY [PossibleDate] DESC)
					WHEN 'PossibleTime' THEN ROW_NUMBER() OVER(ORDER BY [PossibleTime] DESC)
					WHEN 'GeoAddress' THEN ROW_NUMBER() OVER(ORDER BY [GeoAddress] ASC)
					WHEN 'GeoStreet' THEN ROW_NUMBER() OVER(ORDER BY [GeoStreet] DESC)
					WHEN 'GeoDistrict' THEN ROW_NUMBER() OVER(ORDER BY [GeoDistrict] DESC)
					WHEN 'GeoCityMun' THEN ROW_NUMBER() OVER(ORDER BY [GeoCityMun] DESC)
					WHEN 'GeoProvince' THEN ROW_NUMBER() OVER(ORDER BY [GeoProvince] DESC)
					WHEN 'GeoCountry' THEN ROW_NUMBER() OVER(ORDER BY [GeoCountry] DESC)
					WHEN 'ApprovalStatus.ApprovalStatusName' THEN ROW_NUMBER() OVER(ORDER BY [ApprovalStatusName] DESC)
					WHEN 'CrimeIncidentCategory.CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] DESC)
					WHEN 'PostedBySystemUser.LegalEntity.FullName' THEN ROW_NUMBER() OVER(ORDER BY [FullName] DESC)
					ELSE ROW_NUMBER() OVER(ORDER BY [CrimeIncidentReportId] DESC)
				END
				END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
				SELECT 
				cir.[CrimeIncidentReportId],
				MAX(cir.[DateReported])[DateReported],
				MAX(cir.[PossibleDate])[PossibleDate],
				--MAX(CAST(cir.[PossibleTime] as NVARCHAR(10)))[PossibleTime],
				MAX(RIGHT(Convert(VARCHAR(20), cir.[PossibleTime],100),7))[PossibleTime],
				MAX(cir.[Description])[Description],
				MAX(CONCAT(ISNULL(cir.[GeoStreet], ''), ', ',ISNULL(cir.[GeoDistrict], ''), ', ',ISNULL(cir.[GeoCityMun], ''), ', ',ISNULL(cir.[GeoProvince], ''), ', ',ISNULL(cir.[GeoCountry], '')))[GeoAddress],
				MAX(cir.[GeoStreet])[GeoStreet],
				MAX(cir.[GeoDistrict])[GeoDistrict],
				MAX(cir.[GeoCityMun])[GeoCityMun],
				MAX(cir.[GeoProvince])[GeoProvince],
				MAX(cir.[GeoCountry])[GeoCountry],
				MAX(cic.[CrimeIncidentCategoryId])[CrimeIncidentCategoryId],
				MAX(cic.[CrimeIncidentCategoryName])[CrimeIncidentCategoryName],
				MAX(su.[SystemUserId])[SystemUserId],
				MAX(su.[SystemUserTypeId])[SystemUserTypeId],
				MAX(su.[UserName])[UserName],
				MAX(le.[LegalEntityId])[LegalEntityId],
				MAX(le.[FullName])[FullName],
				MAX(eas.[ApprovalStatusId])[ApprovalStatusId],
				MAX(eas.[ApprovalStatusName])[ApprovalStatusName]
			FROM [dbo].[CrimeIncidentReport] AS cir
			LEFT JOIN [dbo].[CrimeIncidentCategory] cic ON cir.[CrimeIncidentCategoryId] = cic.[CrimeIncidentCategoryId]
			LEFT JOIN [dbo].[SystemUser] su ON cir.[PostedBySystemUserId] = su.[SystemUserId]
			LEFT JOIN (SELECT [LegalEntityId],CONCAT(ISNULL([FirstName], ''), ' ', ISNULL([MiddleName], ''), ' ', ISNULL([LastName], ''), ' ') AS [FullName] FROM [dbo].[LegalEntity] WHERE [EntityStatusId] = 1) le ON su.[LegalEntityId] = le.[LegalEntityId]
			LEFT JOIN [dbo].[EntityApprovalStatus] eas ON cir.[ApprovalStatusId] = eas.[ApprovalStatusId]
			WHERE 
			(
				@IsAdvanceSearchMode = 1
					AND cir.ApprovalStatusId = @ApprovalStatusId
					AND cir.[CrimeIncidentReportId] LIKE '%' + ISNULL(@CrimeIncidentReportId, '')  + '%'  
					AND cic.[CrimeIncidentCategoryName] LIKE '%' + ISNULL(@CrimeIncidentCategoryName, '') + '%' 
					AND le.[FullName] LIKE '%' + ISNULL(@PostedByFullName, '') + '%' 
					AND cir.[Description] LIKE '%' + ISNULL(@Description, '') + '%'
					AND cir.[GeoStreet] LIKE '%' + ISNULL(@GeoStreet, '')  + '%'
					AND cir.[GeoDistrict] LIKE '%' + ISNULL(@GeoDistrict, '')  + '%'
					AND cir.[GeoCityMun] LIKE '%' + ISNULL(@GeoCityMun, '')  + '%'
					AND cir.[GeoProvince] LIKE '%' + ISNULL(@GeoProvince, '')  + '%'
					AND cir.[GeoCountry] LIKE '%' +  ISNULL(@GeoCountry, '') + '%'
					AND CAST(cir.[DateReported] as date) BETWEEN @DateReportedFrom AND @DateReportedTo
					AND cir.[PossibleDate] BETWEEN @PossibleDateFrom AND @PossibleDateTo
					AND (CAST(@PossibleTimeTo as time) >= cir.[PossibleTime] AND CAST(@PossibleTimeFrom as time) <= cir.[PossibleTime])
					AND cir.EntityStatusId = 1
					AND cir.ApprovalStatusId = @ApprovalStatusId
				)
			OR ( 
				@IsAdvanceSearchMode = 0 
				AND (
					cir.[CrimeIncidentReportId] LIKE '%' + ISNULL(@Search, '') + '%' 
					OR cic.[CrimeIncidentCategoryName] LIKE '%' + ISNULL(@Search, '') + '%' 
					OR le.[FullName] LIKE '%' + ISNULL(@Search, '') + '%' 
					OR cir.[DateReported] LIKE '%' + ISNULL(@Search, '') + '%'
					OR cir.[PossibleDate] LIKE '%' + ISNULL(@Search, '') + '%'
					OR cir.[PossibleTime] LIKE '%' + ISNULL(@Search, '') + '%'
					OR cir.[Description] LIKE '%' + ISNULL(@Search, '') + '%'
					OR cir.[GeoStreet] LIKE '%' + ISNULL(@Search, '') + '%'
					OR cir.[GeoDistrict] LIKE '%' + ISNULL(@Search, '') + '%'
					OR cir.[GeoCityMun] LIKE '%' + ISNULL(@Search, '') + '%'
					OR cir.[GeoProvince] LIKE '%' + ISNULL(@Search, '') + '%'
					OR cir.[GeoCountry] LIKE '%' + ISNULL(@Search, '') + '%'
					)
					AND cir.EntityStatusId = 1
					AND cir.ApprovalStatusId = @ApprovalStatusId
				)

			GROUP BY cir.[CrimeIncidentReportId]
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreport_getPagedByPostedBySystemUserId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	-- =============================================
	-- Author:		erwin
	-- Create date: 2020-09-16
	-- =============================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreport_getPagedByPostedBySystemUserId]
	@PostedBySystemUserId		NVARCHAR(100) = '',
	@PageNo						BIGINT = 1,
	@PageSize					BIGINT = 10
AS
	BEGIN
		SET NOCOUNT ON;

			WITH DATA_CTE
			AS
			(
				Select tableSource.*, ROW_NUMBER() OVER(ORDER BY [DateReported] DESC)  AS row_num ,
				count(*) over() as TotalRows
				FROM (
				 SELECT 
				 cir.[CrimeIncidentReportId],
				 MAX(cir.[DateReported])[DateReported],
				 MAX(cir.[PossibleDate])[PossibleDate],
				 --MAX(CAST(cir.[PossibleTime] as NVARCHAR(10)))[PossibleTime],
				 MAX(RIGHT(Convert(VARCHAR(20), cir.[PossibleTime],100),7))[PossibleTime],
				 MAX(cir.[Description])[Description],
				 MAX(CONCAT(ISNULL(cir.[GeoStreet], ''), ', ',ISNULL(cir.[GeoDistrict], ''), ', ',ISNULL(cir.[GeoCityMun], ''), ', ',ISNULL(cir.[GeoProvince], ''), ', ',ISNULL(cir.[GeoCountry], '')))[GeoAddress],
				 MAX(cir.[GeoStreet])[GeoStreet],
				 MAX(cir.[GeoDistrict])[GeoDistrict],
				 MAX(cir.[GeoCityMun])[GeoCityMun],
				 MAX(cir.[GeoProvince])[GeoProvince],
				 MAX(cir.[GeoCountry])[GeoCountry],
				 MAX(cic.[CrimeIncidentCategoryId])[CrimeIncidentCategoryId],
				 MAX(cic.[CrimeIncidentCategoryName])[CrimeIncidentCategoryName],
				 MAX(cit.[CrimeIncidentTypeId])[CrimeIncidentTypeId],
				 MAX(cit.[CrimeIncidentTypeName])[CrimeIncidentTypeName],
				 MAX(citi.[FileId])[FileId],
				 MAX(citi.[FileName])[FileName],
				 MAX(citi.[MimeType])[MimeType],
				 MAX(citi.[FileSize])[FileSize],
				 MAX(citi.[FileContent])[FileContent],
				 MAX(su.[SystemUserId])[SystemUserId],
				 MAX(su.[SystemUserTypeId])[SystemUserTypeId],
				 MAX(su.[UserName])[UserName],
				 MAX(le.[LegalEntityId])[LegalEntityId],
				 MAX(le.[FullName])[FullName],
				 MAX(eas.[ApprovalStatusId])[ApprovalStatusId],
				 MAX(eas.[ApprovalStatusName])[ApprovalStatusName]
				FROM [dbo].[CrimeIncidentReport] AS cir
				LEFT JOIN [dbo].[CrimeIncidentCategory] cic ON cir.[CrimeIncidentCategoryId] = cic.[CrimeIncidentCategoryId]
				LEFT JOIN [dbo].[CrimeIncidentType] cit ON cic.[CrimeIncidentTypeId] = cit.[CrimeIncidentTypeId]
				LEFT JOIN [dbo].[File] citi ON cit.IconFileId = citi.FileId
				LEFT JOIN [dbo].[SystemUser] su ON cir.[PostedBySystemUserId] = su.[SystemUserId]
				LEFT JOIN [dbo].[File] suf ON su.ProfilePictureFile = suf.FileId
				LEFT JOIN (SELECT [LegalEntityId],CONCAT(ISNULL([FirstName], ''), ' ', ISNULL([MiddleName], ''), ' ', ISNULL([LastName], ''), ' ') AS [FullName] FROM [dbo].[LegalEntity] WHERE [EntityStatusId] = 1) le ON su.[LegalEntityId] = le.[LegalEntityId]
				LEFT JOIN [dbo].[EntityApprovalStatus] eas ON cir.[ApprovalStatusId] = eas.[ApprovalStatusId]
				WHERE cir.PostedBySystemUserId = @PostedBySystemUserId
				AND cir.EntityStatusId = 1
				GROUP BY cir.[CrimeIncidentReportId]
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreport_update]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreport_update]
	@CrimeIncidentReportId		NVARCHAR(100),
	@CrimeIncidentCategoryId	NVARCHAR(100),
	@PossibleDate				DATE,
	@PossibleTime				TIME(7),
	@Description				NVARCHAR(MAX),
	@GeoStreet					NVARCHAR(250),
	@GeoDistrict				NVARCHAR(100),
	@GeoCityMun					NVARCHAR(100),
	@GeoProvince				NVARCHAR(100),
	@GeoCountry					NVARCHAR(100),
	@GeoTrackerLatitude			FLOAT,
	@GeoTrackerLongitude		FLOAT,
	@LastUpdatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[CrimeIncidentReport]
		SET 
		[CrimeIncidentCategoryId] = @CrimeIncidentCategoryId,
		[PossibleDate] = @PossibleDate,
		[PossibleTime] = @PossibleTime,
		[Description] = @Description,
		[GeoStreet] = @GeoStreet,
		[GeoDistrict] = @GeoDistrict,
		[GeoCityMun] = @GeoCityMun,
		[GeoProvince] = @GeoProvince,
		[GeoCountry] = @GeoCountry,
		[GeoTrackerLatitude] = @GeoTrackerLatitude,
		[GeoTrackerLongitude] = @GeoTrackerLongitude,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [CrimeIncidentReportId] = @CrimeIncidentReportId;

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreport_updateStatus]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreport_updateStatus]
	@CrimeIncidentReportId		NVARCHAR(100),
	@ApprovalStatusId			BIGINT,
	@LastUpdatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE


		IF @ApprovalStatusId = 2
		BEGIN
			UPDATE [dbo].[EnforcementReportValidation] SET ReportValidationStatusId = 4 WHERE [EnforcementReportValidationId] IN
			(
				SELECT [EnforcementReportValidationId] FROM [dbo].[EnforcementReportValidation] WHERE [CrimeIncidentReportId] = @CrimeIncidentReportId AND [ReportValidationStatusId] = 3
			)
		END;

		UPDATE [dbo].[CrimeIncidentReport]
		SET 
		[ApprovalStatusId] = @ApprovalStatusId,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [CrimeIncidentReportId] = @CrimeIncidentReportId;

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreportmedia_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreportmedia_add]
	@DocReportMediaTypeId	BIGINT,
	@FileId					NVARCHAR(250),
	@CrimeIncidentReportId	NVARCHAR(100),
	@Caption				NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @CrimeIncidentReportMediaId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'CrimeIncidentReportMedia', @Id = @CrimeIncidentReportMediaId OUTPUT

		INSERT INTO [dbo].[CrimeIncidentReportMedia](
			[CrimeIncidentReportMediaId],
			[DocReportMediaTypeId],
			[FileId],
			[CrimeIncidentReportId],
			[Caption],
			[EntityStatusId]
		)
		VALUES(
			@CrimeIncidentReportMediaId,
			@DocReportMediaTypeId,
			@FileId,
			@CrimeIncidentReportId,
			@Caption,
			1
		);

		SELECT @CrimeIncidentReportMediaId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreportmedia_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreportmedia_delete]
	@CrimeIncidentReportMediaId		NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[CrimeIncidentReportMedia] WHERE [CrimeIncidentReportMediaId] = @CrimeIncidentReportMediaId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[CrimeIncidentReportMedia]
			SET 
			EntityStatusId = 2
			WHERE [CrimeIncidentReportMediaId] = @CrimeIncidentReportMediaId;

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreportmedia_getByCrimeIncidentReportId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreportmedia_getByCrimeIncidentReportId]
	@CrimeIncidentReportId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		SELECT 
		cirm.[CrimeIncidentReportMediaId],
		cirm.[Caption],
		--cirm.[EntityStatusId],
		drmt.[DocReportMediaTypeId],
		drmt.[DocReportMediaTypeName],
		f.[FileId],
		f.[FileName],
		f.[MimeType],
		f.[FileSize],
		f.[FileContent],
		cir.[CrimeIncidentReportId],
		cir.[PostedBySystemUserId],
		cir.[DateReported]
		FROM [dbo].[CrimeIncidentReportMedia] cirm
		LEFT JOIN [dbo].[DocReportMediaType] drmt ON cirm.[DocReportMediaTypeId] = drmt.[DocReportMediaTypeId]
		LEFT JOIN [dbo].[File] f ON cirm.[FileId] = f.[FileId]
		LEFT JOIN [dbo].[CrimeIncidentReport] cir ON cirm.[CrimeIncidentReportId] = cir.[CrimeIncidentReportId]
		WHERE cirm.[CrimeIncidentReportId] = @CrimeIncidentReportId
		AND cirm.[EntityStatusId] = 1;
		
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreportmedia_getById]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreportmedia_getById]
	@CrimeIncidentReportMediaId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @DocReportMediaTypeId	BIGINT;
		DECLARE @FileId					NVARCHAR(100);
		DECLARE @CrimeIncidentReportId	NVARCHAR(100);
		DECLARE @EntityStatusId			BIGINT;
		
		SELECT 
		@DocReportMediaTypeId = cirm.[DocReportMediaTypeId],
		@FileId = cirm.[FileId],
		@CrimeIncidentReportId = cirm.[CrimeIncidentReportId],
		@EntityStatusId = cirm.[EntityStatusId]
		FROM [dbo].[CrimeIncidentReportMedia] cirm
		WHERE cirm.[CrimeIncidentReportMediaId] = @CrimeIncidentReportMediaId
		AND cirm.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[CrimeIncidentReportMedia] AS cirm
		WHERE cirm.[CrimeIncidentReportMediaId] = @CrimeIncidentReportMediaId
		AND cirm.[EntityStatusId] = 1;
		
		SELECT drmt.*
		FROM [dbo].[DocReportMediaType] drmt
		WHERE drmt.[DocReportMediaTypeId] = @DocReportMediaTypeId	

		SELECT f.*
		FROM [dbo].[File] f
		WHERE f.[FileId] = @FileId	

		SELECT 
		cir.[CrimeIncidentReportId],
		cir.[CrimeIncidentCategoryId],
		cir.[PostedBySystemUserId],
		cir.[DateReported],
		cir.[PossibleDate],
		--CAST(cir.[PossibleTime] AS NVARCHAR(10))[PossibleTime],
		RIGHT(Convert(VARCHAR(20), cir.[PossibleTime],100),7)[PossibleTime],
		cir.[Description],
		cir.[GeoStreet],
		cir.[GeoDistrict],
		cir.[GeoCityMun],
		cir.[GeoProvince],
		cir.[GeoCountry],
		cir.[GeoTrackerLatitude],
		cir.[GeoTrackerLongitude],
		cir.[ApprovalStatusId],
		cir.[IsReviewActionEnable],
		cir.[IsReviewCommentEnable],
		cir.[CreatedBy],
		cir.[CreatedAt],
		cir.[LastUpdatedBy],
		cir.[LastUpdatedAt],
		cir.[EntityStatusId]
		FROM [dbo].[CrimeIncidentReport] cir
		WHERE cir.[CrimeIncidentReportId] = @CrimeIncidentReportId

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidentreportmedia_update]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_crimeincidentreportmedia_update]
	@CrimeIncidentReportMediaId	NVARCHAR(250),
	@DocReportMediaTypeId		BIGINT,
	@Caption					NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[CrimeIncidentReportMedia]
		SET 
		[Caption] = @Caption,
		[DocReportMediaTypeId ]= @DocReportMediaTypeId
		WHERE [CrimeIncidentReportMediaId] = @CrimeIncidentReportMediaId;

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidenttype_add]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidenttype_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
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
		IF EXISTS(SELECT * FROM [dbo].[CrimeIncidentCategory] WHERE [CrimeIncidentTypeId] = @CrimeIncidentTypeId AND [EntityStatusId] = 1)
		BEGIN
			RAISERROR('Cannot delete CrimeIncidentType, data is used in CrimeIncidentCategory', 16, 1)
		END

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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidenttype_getByID]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidenttype_getPaged]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_crimeincidenttype_update]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementreportvalidation_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementreportvalidation_add]
	@CrimeIncidentReportId	NVARCHAR(100),
	@EnforcementUnitId		NVARCHAR(100),
	@ReportNotes			NVARCHAR(MAX),
	@CreatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;


		IF EXISTS(SELECT * FROM [dbo].[EnforcementReportValidation] WHERE [CrimeIncidentReportId] = @CrimeIncidentReportId AND [EnforcementUnitId] = @EnforcementUnitId AND [ReportValidationStatusId] = 3)
		BEGIN
			RAISERROR('Cannot insert duplicate Pending EnforcementReportValidation', 16, 1)
		END
		DECLARE @EnforcementReportValidationId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'EnforcementReportValidation', @Id = @EnforcementReportValidationId OUTPUT

		INSERT INTO [dbo].[EnforcementReportValidation](
			[EnforcementReportValidationId],
			[CrimeIncidentReportId],
			[EnforcementUnitId],
			[DateSubmitted],
			[ReportNotes],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@EnforcementReportValidationId,
			@CrimeIncidentReportId,
			@EnforcementUnitId,
			GETDATE(),
			@ReportNotes,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @EnforcementReportValidationId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementreportvalidation_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementreportvalidation_delete]
	@EnforcementReportValidationId	NVARCHAR(100),
	@LastUpdatedBy					NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[EnforcementReportValidation] WHERE [EnforcementReportValidationId] = @EnforcementReportValidationId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[EnforcementReportValidation]
			SET 
			EntityStatusId = 2,
			[LastUpdatedBy] = @LastUpdatedBy,
			[LastUpdatedAt] = GETDATE()
			WHERE [EnforcementReportValidationId] = @EnforcementReportValidationId;

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
/****** Object:  StoredProcedure [dbo].[usp_enforcementreportvalidation_getByID]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementreportvalidation_getByID]
	@EnforcementReportValidationId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @CrimeIncidentReportId		NVARCHAR(100);
		DECLARE @EnforcementUnitId			NVARCHAR(100);
		DECLARE @ReportValidationStatusId	NVARCHAR(100);
		DECLARE @CreatedBy					NVARCHAR(100);
		DECLARE @CreatedAt					DATETIME;
		DECLARE @LastUpdatedBy				NVARCHAR(100);
		DECLARE @LastUpdatedAt				DATETIME;
		DECLARE @EntityStatusId				BIGINT;
		
		SELECT 
		@CrimeIncidentReportId = erv.[CrimeIncidentReportId],
		@EnforcementUnitId = erv.[EnforcementUnitId],
		@ReportValidationStatusId = erv.[ReportValidationStatusId],
		@CreatedBy = erv.[CreatedBy],
		@CreatedAt = erv.[CreatedAt],
		@LastUpdatedBy = erv.[LastUpdatedBy],
		@LastUpdatedAt = erv.[LastUpdatedAt],
		@EntityStatusId = erv.[EntityStatusId]
		FROM [dbo].[EnforcementReportValidation] erv
		WHERE erv.[EnforcementReportValidationId] = @EnforcementReportValidationId
		AND erv.[EntityStatusId] = 1;
		
		SELECT *
		FROM [dbo].[EnforcementReportValidation] erv
		WHERE erv.[EnforcementReportValidationId] = @EnforcementReportValidationId
		AND erv.[EntityStatusId] = 1;

		exec [dbo].[usp_enforcementunit_getByID] @EnforcementUnitId

		exec [dbo].[usp_crimeincidentreport_getByID] @CrimeIncidentReportId

		SELECT * FROM [dbo].[ReportValidationStatus] WHERE [ReportValidationStatusId] = @ReportValidationStatusId;

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
/****** Object:  StoredProcedure [dbo].[usp_enforcementreportvalidation_getPagedByCrimeIncidentReportId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	-- =============================================
	-- Author:		erwin
	-- Create date: 2020-09-16
	-- =============================================
CREATE PROCEDURE [dbo].[usp_enforcementreportvalidation_getPagedByCrimeIncidentReportId]
	@Search						NVARCHAR(100) = '',
	@IsAdvanceSearchMode		BIT = 0,
	@CrimeIncidentReportId		NVARCHAR(100) = '',
	@EnforcementUnitName		NVARCHAR(250) = '',
	@DateSubmittedFrom			DATETIME = GETDATE,
	@DateSubmittedTo			DATETIME = GETDATE,
	@ReportValidationStatusId	BIGINT = 3,
	@PageNo						BIGINT = 1,
	@PageSize					BIGINT = 10,
	@OrderColumn				NVARCHAR(100) = 'EnforcementReportValidationId',
	@OrderDir					NVARCHAR(5) = 'ASC'
AS
BEGIN
	SET NOCOUNT ON;

		WITH DATA_CTE
		AS
		(
			Select tableSource.*, 
			(CASE IIF(ISNULL(@OrderDir, 'asc') = '', 'asc', @OrderDir)
				WHEN 'asc' THEN
				CASE IIF(ISNULL(@OrderColumn, 'EnforcementReportValidationId') = '', 'EnforcementReportValidationId', @OrderColumn) 
					WHEN 'DateSubmitted' THEN ROW_NUMBER() OVER(ORDER BY [PossibleDate] ASC)
					WHEN 'CrimeIncidentReport.CrimeIncidentCategory.CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] ASC)
					WHEN 'EnforcementUnit.LegalEntity.FullName' THEN ROW_NUMBER() OVER(ORDER BY [FullName] ASC)
					WHEN 'ReportValidationStatus.ReportValidationStatusName' THEN ROW_NUMBER() OVER(ORDER BY [ReportValidationStatusName] ASC)
					ELSE ROW_NUMBER() OVER(ORDER BY [EnforcementReportValidationId] DESC)
				END
			ELSE
				CASE IIF(ISNULL(@OrderColumn, 'EnforcementReportValidationId') = '', 'EnforcementReportValidationId', @OrderColumn) 
					WHEN 'DateSubmitted' THEN ROW_NUMBER() OVER(ORDER BY [PossibleDate] ASC)
					WHEN 'CrimeIncidentReport.CrimeIncidentCategory.CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] ASC)
					WHEN 'EnforcementUnit.LegalEntity.FullName' THEN ROW_NUMBER() OVER(ORDER BY [FullName] ASC)
					WHEN 'ReportValidationStatus.ReportValidationStatusName' THEN ROW_NUMBER() OVER(ORDER BY [ReportValidationStatusName] ASC)
					ELSE ROW_NUMBER() OVER(ORDER BY [EnforcementReportValidationId] DESC)
				END
				END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
				SELECT 
				erv.[EnforcementReportValidationId],
				MAX(erv.[DateSubmitted])[DateSubmitted],
				MAX(cir.[CrimeIncidentReportId])[CrimeIncidentReportId],
				MAX(cir.[DateReported])[DateReported],
				MAX(cir.[PossibleDate])[PossibleDate],
				--MAX(CAST(cir.[PossibleTime] as NVARCHAR(10)))[PossibleTime],
				MAX(RIGHT(Convert(VARCHAR(20), cir.[PossibleTime],100),7))[PossibleTime],
				MAX(cir.[Description])[Description],
				MAX(CONCAT(ISNULL(cir.[GeoStreet], ''), ', ',ISNULL(cir.[GeoDistrict], ''), ', ',ISNULL(cir.[GeoCityMun], ''), ', ',ISNULL(cir.[GeoProvince], ''), ', ',ISNULL(cir.[GeoCountry], '')))[GeoAddress],
				MAX(cir.[GeoStreet])[GeoStreet],
				MAX(cir.[GeoDistrict])[GeoDistrict],
				MAX(cir.[GeoCityMun])[GeoCityMun],
				MAX(cir.[GeoProvince])[GeoProvince],
				MAX(cir.[GeoCountry])[GeoCountry],
				MAX(cic.[CrimeIncidentCategoryId])[CrimeIncidentCategoryId],
				MAX(cic.[CrimeIncidentCategoryName])[CrimeIncidentCategoryName],
				MAX(eas.[ApprovalStatusId])[ApprovalStatusId],
				MAX(eas.[ApprovalStatusName])[ApprovalStatusName],
				MAX(eu.[EnforcementUnitId])[EnforcementUnitId],
				MAX(eule.[LegalEntityId])[LegalEntityId],
				MAX(eule.[FullName])[FullName],
				MAX(rvs.[ReportValidationStatusId])[ReportValidationStatusId],
				MAX(rvs.[ReportValidationStatusName])[ReportValidationStatusName]
			FROM [dbo].[EnforcementReportValidation] AS erv 
			LEFT JOIN [dbo].[CrimeIncidentReport] AS cir ON erv.CrimeIncidentReportId = cir.CrimeIncidentReportId
			LEFT JOIN [dbo].[EnforcementUnit] AS eu ON erv.EnforcementUnitId = eu.EnforcementUnitId
			LEFT JOIN (SELECT [LegalEntityId],CONCAT(ISNULL([FirstName], ''), ' ', ISNULL([MiddleName], ''), ' ', ISNULL([LastName], ''), ' ') AS [FullName] FROM [dbo].[LegalEntity] WHERE [EntityStatusId] = 1) eule ON eu.[LegalEntityId] = eule.[LegalEntityId]
			LEFT JOIN [dbo].[CrimeIncidentCategory] cic ON cir.[CrimeIncidentCategoryId] = cic.[CrimeIncidentCategoryId]
			LEFT JOIN [dbo].[ReportValidationStatus] rvs ON erv.[ReportValidationStatusId] = rvs.[ReportValidationStatusId]
			LEFT JOIN [dbo].[EntityApprovalStatus] eas ON cir.[ApprovalStatusId] = eas.[ApprovalStatusId]
			WHERE 
			(
				@IsAdvanceSearchMode = 1
					AND cir.[CrimeIncidentReportId] = @CrimeIncidentReportId  
					AND eule.[FullName] LIKE '%' + ISNULL(@EnforcementUnitName, '') + '%' 
					AND CAST(erv.[DateSubmitted] as date) BETWEEN @DateSubmittedFrom AND @DateSubmittedTo
					AND cir.EntityStatusId = 1
					AND erv.ReportValidationStatusId = @ReportValidationStatusId
				)
			OR ( 
				@IsAdvanceSearchMode = 0 
				AND (
						eule.[FullName] LIKE '%' + ISNULL(@Search, '') + '%' 
						OR cir.[CrimeIncidentReportId] LIKE '%' + ISNULL(@Search, '') + '%' 
						OR cir.[CrimeIncidentCategoryId] LIKE '%' + ISNULL(@Search, '') + '%' 
						OR cic.[CrimeIncidentCategoryName] LIKE '%' + ISNULL(@Search, '') + '%' 
						OR erv.[DateSubmitted] LIKE '%' + ISNULL(@Search, '') + '%' 
					)
					AND cir.EntityStatusId = 1
					AND cir.[CrimeIncidentReportId] = @CrimeIncidentReportId  
					AND erv.[ReportValidationStatusId] = @ReportValidationStatusId
				)

			GROUP BY erv.[EnforcementReportValidationId]
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementreportvalidation_getPagedByEnforcementStationId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	-- =============================================
	-- Author:		erwin
	-- Create date: 2020-09-16
	-- =============================================
CREATE PROCEDURE [dbo].[usp_enforcementreportvalidation_getPagedByEnforcementStationId]
	@Search						NVARCHAR(100) = '',
	@IsAdvanceSearchMode		BIT = 0,
	@EnforcementStationId		NVARCHAR(100) = '',
	@CrimeIncidentCategoryName	NVARCHAR(MAX) = '',
	@DateSubmittedFrom			DATETIME = GETDATE,
	@DateSubmittedTo			DATETIME = GETDATE,
	@ReportValidationStatusId	BIGINT = 3,
	@PageNo						BIGINT = 1,
	@PageSize					BIGINT = 10,
	@OrderColumn				NVARCHAR(100) = 'EnforcementReportValidationId',
	@OrderDir					NVARCHAR(5) = 'ASC'
AS
BEGIN
	SET NOCOUNT ON;

		WITH DATA_CTE
		AS
		(
			Select tableSource.*, 
			(CASE IIF(ISNULL(@OrderDir, 'asc') = '', 'asc', @OrderDir)
				WHEN 'asc' THEN
				CASE IIF(ISNULL(@OrderColumn, 'EnforcementReportValidationId') = '', 'EnforcementReportValidationId', @OrderColumn) 
					WHEN 'DateSubmitted' THEN ROW_NUMBER() OVER(ORDER BY [PossibleDate] ASC)
					WHEN 'CrimeIncidentReport.CrimeIncidentCategory.CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] ASC)
					WHEN 'ReportValidationStatus.ReportValidationStatusName' THEN ROW_NUMBER() OVER(ORDER BY [ReportValidationStatusName] ASC)
					ELSE ROW_NUMBER() OVER(ORDER BY [EnforcementReportValidationId] DESC)
				END
			ELSE
				CASE IIF(ISNULL(@OrderColumn, 'EnforcementReportValidationId') = '', 'EnforcementReportValidationId', @OrderColumn) 
					WHEN 'DateSubmitted' THEN ROW_NUMBER() OVER(ORDER BY [PossibleDate] ASC)
					WHEN 'CrimeIncidentReport.CrimeIncidentCategory.CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] ASC)
					WHEN 'ReportValidationStatus.ReportValidationStatusName' THEN ROW_NUMBER() OVER(ORDER BY [ReportValidationStatusName] ASC)
					ELSE ROW_NUMBER() OVER(ORDER BY [EnforcementReportValidationId] DESC)
				END
				END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
				SELECT 
				erv.[EnforcementReportValidationId],
				MAX(erv.[DateSubmitted])[DateSubmitted],
				MAX(cir.[CrimeIncidentReportId])[CrimeIncidentReportId],
				MAX(cir.[DateReported])[DateReported],
				MAX(cir.[PossibleDate])[PossibleDate],
				--MAX(CAST(cir.[PossibleTime] as NVARCHAR(10)))[PossibleTime],
				MAX(RIGHT(Convert(VARCHAR(20), cir.[PossibleTime],100),7))[PossibleTime],
				MAX(cir.[Description])[Description],
				MAX(CONCAT(ISNULL(cir.[GeoStreet], ''), ', ',ISNULL(cir.[GeoDistrict], ''), ', ',ISNULL(cir.[GeoCityMun], ''), ', ',ISNULL(cir.[GeoProvince], ''), ', ',ISNULL(cir.[GeoCountry], '')))[GeoAddress],
				MAX(cir.[GeoStreet])[GeoStreet],
				MAX(cir.[GeoDistrict])[GeoDistrict],
				MAX(cir.[GeoCityMun])[GeoCityMun],
				MAX(cir.[GeoProvince])[GeoProvince],
				MAX(cir.[GeoCountry])[GeoCountry],
				MAX(cic.[CrimeIncidentCategoryId])[CrimeIncidentCategoryId],
				MAX(cic.[CrimeIncidentCategoryName])[CrimeIncidentCategoryName],
				MAX(rvs.[ReportValidationStatusId])[ReportValidationStatusId],
				MAX(rvs.[ReportValidationStatusName])[ReportValidationStatusName]
			FROM [dbo].[EnforcementReportValidation] AS erv 
			LEFT JOIN [dbo].[CrimeIncidentReport] AS cir ON erv.CrimeIncidentReportId = cir.CrimeIncidentReportId
			LEFT JOIN [dbo].[CrimeIncidentCategory] cic ON cir.[CrimeIncidentCategoryId] = cic.[CrimeIncidentCategoryId]
			LEFT JOIN [dbo].[ReportValidationStatus] rvs ON erv.[ReportValidationStatusId] = rvs.[ReportValidationStatusId]
			LEFT JOIN [dbo].[EntityApprovalStatus] eas ON cir.[ApprovalStatusId] = eas.[ApprovalStatusId]
			WHERE 
			(
				@IsAdvanceSearchMode = 1
					AND cic.[CrimeIncidentCategoryName] LIKE '%' + @CrimeIncidentCategoryName + '%'  
					AND CAST(erv.[DateSubmitted] as date) BETWEEN @DateSubmittedFrom AND @DateSubmittedTo
					AND cir.EntityStatusId = 1
					AND erv.ReportValidationStatusId = @ReportValidationStatusId
					AND erv.EntityStatusId = 1
					--AND erv.[EnforcementStationId] = @EnforcementStationId
				)
			OR ( 
				@IsAdvanceSearchMode = 0 
				AND (
						cir.[CrimeIncidentCategoryId] LIKE '%' + ISNULL(@Search, '') + '%' 
						OR cic.[CrimeIncidentCategoryName] LIKE '%' + ISNULL(@Search, '') + '%' 
						OR erv.[DateSubmitted] LIKE '%' + ISNULL(@Search, '') + '%' 
					)
					AND cir.EntityStatusId = 1 
					--AND erv.[EnforcementStationId] = @EnforcementStationId
					AND erv.[ReportValidationStatusId] = @ReportValidationStatusId
				)

			GROUP BY erv.[EnforcementReportValidationId]
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementreportvalidation_getPagedBySystemUserId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	-- =============================================
	-- Author:		erwin
	-- Create date: 2020-09-16
	-- =============================================
CREATE PROCEDURE [dbo].[usp_enforcementreportvalidation_getPagedBySystemUserId]
	@Search						NVARCHAR(100) = '',
	@IsAdvanceSearchMode		BIT = 0,
	@SystemUserId				NVARCHAR(100) = '',
	@CrimeIncidentCategoryName	NVARCHAR(MAX) = '',
	@DateSubmittedFrom			DATETIME = GETDATE,
	@DateSubmittedTo			DATETIME = GETDATE,
	@ReportValidationStatusId	BIGINT = 3,
	@PageNo						BIGINT = 1,
	@PageSize					BIGINT = 10,
	@OrderColumn				NVARCHAR(100) = 'EnforcementReportValidationId',
	@OrderDir					NVARCHAR(5) = 'ASC'
AS
BEGIN
	SET NOCOUNT ON;
	
		DECLARE @CanManageEnforcementValidationReportForOtherEnforcementUnit BIT;
		SELECT @CanManageEnforcementValidationReportForOtherEnforcementUnit = IIF(COUNT(*) <> 0, 1, 0) FROM [SystemWebAdminUserRole] WHERE [SystemWebAdminRoleId] IN
		(
			SELECT [SystemWebAdminRoleId] FROM [SystemWebAdminRolePrivileges] WHERE [SystemWebAdminPrivilegeId] IN
			(
				SELECT [SystemWebAdminPrivilegeId] FROM [SystemWebAdminPrivileges] WHERE [SystemWebAdminPrivilegeId] = 1
			) AND [SystemWebAdminRolePrivileges].IsAllowed = 1 AND [EntityStatusId] = 1
		) AND [SystemUserId] = @SystemUserId AND [EntityStatusId] = 1;
		;WITH DATA_CTE
		AS
		(
			Select tableSource.*, 
			(CASE IIF(ISNULL(@OrderDir, 'asc') = '', 'asc', @OrderDir)
				WHEN 'asc' THEN
				CASE IIF(ISNULL(@OrderColumn, 'EnforcementReportValidationId') = '', 'EnforcementReportValidationId', @OrderColumn) 
					WHEN 'DateSubmitted' THEN ROW_NUMBER() OVER(ORDER BY [PossibleDate] ASC)
					WHEN 'CrimeIncidentReport.CrimeIncidentCategory.CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] ASC)
					WHEN 'ReportValidationStatus.ReportValidationStatusName' THEN ROW_NUMBER() OVER(ORDER BY [ReportValidationStatusName] ASC)
					ELSE ROW_NUMBER() OVER(ORDER BY [EnforcementReportValidationId] DESC)
				END
			ELSE
				CASE IIF(ISNULL(@OrderColumn, 'EnforcementReportValidationId') = '', 'EnforcementReportValidationId', @OrderColumn) 
					WHEN 'DateSubmitted' THEN ROW_NUMBER() OVER(ORDER BY [PossibleDate] ASC)
					WHEN 'CrimeIncidentReport.CrimeIncidentCategory.CrimeIncidentCategoryName' THEN ROW_NUMBER() OVER(ORDER BY [CrimeIncidentCategoryName] ASC)
					WHEN 'ReportValidationStatus.ReportValidationStatusName' THEN ROW_NUMBER() OVER(ORDER BY [ReportValidationStatusName] ASC)
					ELSE ROW_NUMBER() OVER(ORDER BY [EnforcementReportValidationId] DESC)
				END
				END) AS row_num ,
			count(*) over() as TotalRows
			FROM (

			SELECT EnforcementReportValidation.[EnforcementReportValidationId],
				MAX(EnforcementReportValidation.[DateSubmitted])[DateSubmitted],
				MAX(EnforcementReportValidation.[CrimeIncidentReportId])[CrimeIncidentReportId],
				MAX(EnforcementReportValidation.[DateReported])[DateReported],
				MAX(EnforcementReportValidation.[PossibleDate])[PossibleDate],
				--MAX(CAST(EnforcementReportValidation.[PossibleTime] as NVARCHAR(10)))[PossibleTime],
				MAX(RIGHT(Convert(VARCHAR(20), EnforcementReportValidation.[PossibleTime],100),7))[PossibleTime],
				MAX(EnforcementReportValidation.[Description])[Description],
				MAX(CONCAT(ISNULL(EnforcementReportValidation.[GeoStreet], ''), ', ',ISNULL(EnforcementReportValidation.[GeoDistrict], ''), ', ',ISNULL(EnforcementReportValidation.[GeoCityMun], ''), ', ',ISNULL(EnforcementReportValidation.[GeoProvince], ''), ', ',ISNULL(EnforcementReportValidation.[GeoCountry], '')))[GeoAddress],
				MAX(EnforcementReportValidation.[GeoStreet])[GeoStreet],
				MAX(EnforcementReportValidation.[GeoDistrict])[GeoDistrict],
				MAX(EnforcementReportValidation.[GeoCityMun])[GeoCityMun],
				MAX(EnforcementReportValidation.[GeoProvince])[GeoProvince],
				MAX(EnforcementReportValidation.[GeoCountry])[GeoCountry],
				MAX(EnforcementReportValidation.[CrimeIncidentCategoryId])[CrimeIncidentCategoryId],
				MAX(EnforcementReportValidation.[CrimeIncidentCategoryName])[CrimeIncidentCategoryName],
				MAX(EnforcementReportValidation.[ReportValidationStatusId])[ReportValidationStatusId],
				MAX(EnforcementReportValidation.[ReportValidationStatusName])[ReportValidationStatusName] 
				FROM (
					SELECT 
					erv.[EnforcementReportValidationId],
					MAX(erv.[DateSubmitted])[DateSubmitted],
					MAX(cir.[CrimeIncidentReportId])[CrimeIncidentReportId],
					MAX(cir.[DateReported])[DateReported],
					MAX(cir.[PossibleDate])[PossibleDate],
					MAX(CAST(cir.[PossibleTime] as NVARCHAR(10)))[PossibleTime],
					MAX(cir.[Description])[Description],
					MAX(CONCAT(ISNULL(cir.[GeoStreet], ''), ', ',ISNULL(cir.[GeoDistrict], ''), ', ',ISNULL(cir.[GeoCityMun], ''), ', ',ISNULL(cir.[GeoProvince], ''), ', ',ISNULL(cir.[GeoCountry], '')))[GeoAddress],
					MAX(cir.[GeoStreet])[GeoStreet],
					MAX(cir.[GeoDistrict])[GeoDistrict],
					MAX(cir.[GeoCityMun])[GeoCityMun],
					MAX(cir.[GeoProvince])[GeoProvince],
					MAX(cir.[GeoCountry])[GeoCountry],
					MAX(cic.[CrimeIncidentCategoryId])[CrimeIncidentCategoryId],
					MAX(cic.[CrimeIncidentCategoryName])[CrimeIncidentCategoryName],
					MAX(rvs.[ReportValidationStatusId])[ReportValidationStatusId],
					MAX(rvs.[ReportValidationStatusName])[ReportValidationStatusName]
				FROM [dbo].[EnforcementReportValidation] AS erv 
				LEFT JOIN [dbo].[SystemUser] su ON erv.[EnforcementUnitId] = su.[EnforcementUnitId]
				LEFT JOIN [dbo].[CrimeIncidentReport] AS cir ON erv.CrimeIncidentReportId = cir.CrimeIncidentReportId
				LEFT JOIN [dbo].[CrimeIncidentCategory] cic ON cir.[CrimeIncidentCategoryId] = cic.[CrimeIncidentCategoryId]
				LEFT JOIN [dbo].[ReportValidationStatus] rvs ON erv.[ReportValidationStatusId] = rvs.[ReportValidationStatusId]
				LEFT JOIN [dbo].[EntityApprovalStatus] eas ON cir.[ApprovalStatusId] = eas.[ApprovalStatusId]
				WHERE 
				(
					@IsAdvanceSearchMode = 1
						AND cic.[CrimeIncidentCategoryName] LIKE '%' + @CrimeIncidentCategoryName + '%'  
						AND CAST(erv.[DateSubmitted] as date) BETWEEN @DateSubmittedFrom AND @DateSubmittedTo
						AND cir.EntityStatusId = 1
						AND erv.ReportValidationStatusId = @ReportValidationStatusId
						AND erv.EntityStatusId = 1
						AND su.SystemUserId = @SystemUserId
					)
				OR ( 
					@IsAdvanceSearchMode = 0 
					AND (
							cir.[CrimeIncidentCategoryId] LIKE '%' + ISNULL(@Search, '') + '%' 
							OR cic.[CrimeIncidentCategoryName] LIKE '%' + ISNULL(@Search, '') + '%' 
							OR erv.[DateSubmitted] LIKE '%' + ISNULL(@Search, '') + '%' 
						)
						AND cir.EntityStatusId = 1 
						AND su.SystemUserId = @SystemUserId
						AND erv.[ReportValidationStatusId] = @ReportValidationStatusId
					)

				GROUP BY erv.[EnforcementReportValidationId]

				UNION 

				SELECT 
					erv.[EnforcementReportValidationId],
					MAX(erv.[DateSubmitted])[DateSubmitted],
					MAX(cir.[CrimeIncidentReportId])[CrimeIncidentReportId],
					MAX(cir.[DateReported])[DateReported],
					MAX(cir.[PossibleDate])[PossibleDate],
					MAX(CAST(cir.[PossibleTime] as NVARCHAR(10)))[PossibleTime],
					MAX(cir.[Description])[Description],
					MAX(CONCAT(ISNULL(cir.[GeoStreet], ''), ', ',ISNULL(cir.[GeoDistrict], ''), ', ',ISNULL(cir.[GeoCityMun], ''), ', ',ISNULL(cir.[GeoProvince], ''), ', ',ISNULL(cir.[GeoCountry], '')))[GeoAddress],
					MAX(cir.[GeoStreet])[GeoStreet],
					MAX(cir.[GeoDistrict])[GeoDistrict],
					MAX(cir.[GeoCityMun])[GeoCityMun],
					MAX(cir.[GeoProvince])[GeoProvince],
					MAX(cir.[GeoCountry])[GeoCountry],
					MAX(cic.[CrimeIncidentCategoryId])[CrimeIncidentCategoryId],
					MAX(cic.[CrimeIncidentCategoryName])[CrimeIncidentCategoryName],
					MAX(rvs.[ReportValidationStatusId])[ReportValidationStatusId],
					MAX(rvs.[ReportValidationStatusName])[ReportValidationStatusName]
				FROM [dbo].[EnforcementReportValidation] AS erv 
				LEFT JOIN [dbo].[EnforcementUnit] eu ON erv.[EnforcementUnitId] = eu.[EnforcementUnitId]
				LEFT JOIN [dbo].[SystemUser] su ON  eu.[EnforcementStationId] = su.[EnforcementStationId]
				LEFT JOIN [dbo].[CrimeIncidentReport] AS cir ON erv.CrimeIncidentReportId = cir.CrimeIncidentReportId
				LEFT JOIN [dbo].[CrimeIncidentCategory] cic ON cir.[CrimeIncidentCategoryId] = cic.[CrimeIncidentCategoryId]
				LEFT JOIN [dbo].[ReportValidationStatus] rvs ON erv.[ReportValidationStatusId] = rvs.[ReportValidationStatusId]
				LEFT JOIN [dbo].[EntityApprovalStatus] eas ON cir.[ApprovalStatusId] = eas.[ApprovalStatusId]
				WHERE 
				(
					@IsAdvanceSearchMode = 1
						AND cic.[CrimeIncidentCategoryName] LIKE '%' + @CrimeIncidentCategoryName + '%'  
						AND CAST(erv.[DateSubmitted] as date) BETWEEN @DateSubmittedFrom AND @DateSubmittedTo
						AND cir.EntityStatusId = 1
						AND erv.ReportValidationStatusId = @ReportValidationStatusId
						AND erv.EntityStatusId = 1
						AND su.SystemUserId = IIF(@CanManageEnforcementValidationReportForOtherEnforcementUnit = 1, @SystemUserId, null)
					)
				OR ( 
					@IsAdvanceSearchMode = 0 
					AND (
							cir.[CrimeIncidentCategoryId] LIKE '%' + ISNULL(@Search, '') + '%' 
							OR cic.[CrimeIncidentCategoryName] LIKE '%' + ISNULL(@Search, '') + '%' 
							OR erv.[DateSubmitted] LIKE '%' + ISNULL(@Search, '') + '%' 
						)
						AND cir.EntityStatusId = 1 
						AND su.SystemUserId = IIF(@CanManageEnforcementValidationReportForOtherEnforcementUnit = 1, @SystemUserId, null) 
						AND erv.[ReportValidationStatusId] = @ReportValidationStatusId
					)

				GROUP BY [EnforcementReportValidationId]
			) EnforcementReportValidation
			

			GROUP BY EnforcementReportValidation.[EnforcementReportValidationId]

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
/****** Object:  StoredProcedure [dbo].[usp_enforcementreportvalidation_update]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementreportvalidation_update]
	@EnforcementReportValidationId	NVARCHAR(100),
	@EnforcementUnitId				NVARCHAR(100),
	@ReportNotes					NVARCHAR(MAX),
	@ValidationNotes				NVARCHAR(MAX),
	@ReportValidationStatusId		BIGINT,
	@LastUpdatedBy					NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[EnforcementReportValidation]
		SET 
		[EnforcementUnitId] = @EnforcementUnitId,
		[ReportNotes] = @ReportNotes,
		[ValidationNotes] = @ValidationNotes,
		[ReportValidationStatusId] = @ReportValidationStatusId,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [EnforcementReportValidationId] = @EnforcementReportValidationId;
		
		DECLARE @CrimeIncidentReportId	NVARCHAR(100);
		IF @ReportValidationStatusId = 1 -- validated enforcement validation report
		BEGIN
			-- UPDATE [dbo].[CrimeIncidentReport] set to Validated
			SET @CrimeIncidentReportId = (SELECT [CrimeIncidentReportId] FROM [dbo].[EnforcementReportValidation] WHERE [EnforcementReportValidationId] = @EnforcementReportValidationId)
			EXEC [dbo].[usp_crimeincidentreport_updateStatus] @CrimeIncidentReportId, 5, @LastUpdatedBy
		END
		IF @ReportValidationStatusId = 5 -- process/ongoing enforcement validation report
		BEGIN
			-- UPDATE [dbo].[CrimeIncidentReport] set to Ongoing
			SET @CrimeIncidentReportId = (SELECT [CrimeIncidentReportId] FROM [dbo].[EnforcementReportValidation] WHERE [EnforcementReportValidationId] = @EnforcementReportValidationId)
			EXEC [dbo].[usp_crimeincidentreport_updateStatus] @CrimeIncidentReportId, 4, @LastUpdatedBy
		END
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementstation_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementstation_add]
	@EnforcementStationName			NVARCHAR(100),
	@EnforcementStationGuestCode	NVARCHAR(100),
	@IconFileId						NVARCHAR(100),
	@CreatedBy						NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @EnforcementStationId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'EnforcementStation', @Id = @EnforcementStationId OUTPUT

		INSERT INTO [dbo].[EnforcementStation](
			[EnforcementStationId],
			[EnforcementStationName],
			[EnforcementStationGuestCode],
			[IconFileId],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@EnforcementStationId,
			@EnforcementStationName,
			@EnforcementStationGuestCode,
			@IconFileId,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @EnforcementStationId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementstation_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementstation_delete]
	@EnforcementStationId		NVARCHAR(100),
	@LastUpdatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[SystemUser] WHERE [IsEnforcementUnit] = 1 AND [EnforcementStationId] = @EnforcementStationId AND [EntityStatusId] = 1)
		BEGIN
			RAISERROR('Cannot delete EnforcementStation, data is used in SystemUser', 16, 1)
		END
		IF EXISTS(SELECT * FROM [dbo].[EnforcementUnit] WHERE [EnforcementStationId] = @EnforcementStationId AND [EntityStatusId] = 1)
		BEGIN
			RAISERROR('Cannot delete EnforcementStation, data is used in EnforcementUnit', 16, 1)
		END

		IF EXISTS(SELECT * FROM [dbo].[EnforcementStation] WHERE [EnforcementStationId] = @EnforcementStationId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[EnforcementStation]
			SET 
			[EnforcementStationName] = @EnforcementStationId + ' - ' + [EnforcementStationName] + '(DELETED - ' + CONVERT(VARCHAR(50),GETDATE())+ ')',
			EntityStatusId = 2,
			[LastUpdatedBy] = @LastUpdatedBy,
			[LastUpdatedAt] = GETDATE()
			WHERE [EnforcementStationId] = @EnforcementStationId;

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
/****** Object:  StoredProcedure [dbo].[usp_enforcementstation_getByGuestCode]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementstation_getByGuestCode]
	@EnforcementStationGuestCode	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @EnforcementStationId	NVARCHAR(100);
		DECLARE @IconFileId				NVARCHAR(100);
		DECLARE @CreatedBy				NVARCHAR(100);
		DECLARE @CreatedAt				DATETIME;
		DECLARE @LastUpdatedBy			NVARCHAR(100);
		DECLARE @LastUpdatedAt			DATETIME;
		DECLARE @EntityStatusId			BIGINT;
		
		SELECT 
		@EnforcementStationId = et.[EnforcementStationId],
		@IconFileId = et.[IconFileId],
		@CreatedBy = et.[CreatedBy],
		@CreatedAt = et.[CreatedAt],
		@LastUpdatedBy = et.[LastUpdatedBy],
		@LastUpdatedAt = et.[LastUpdatedAt],
		@EntityStatusId = et.[EntityStatusId]
		FROM [dbo].[EnforcementStation] et
		WHERE et.[EnforcementStationGuestCode] = @EnforcementStationGuestCode
		AND et.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[EnforcementStation] AS et
		WHERE et.[EnforcementStationId] = @EnforcementStationId
		AND et.[EntityStatusId] = 1;
		
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementstation_getByID]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementstation_getByID]
	@EnforcementStationId	NVARCHAR(100)
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
		@IconFileId = et.[IconFileId],
		@CreatedBy = et.[CreatedBy],
		@CreatedAt = et.[CreatedAt],
		@LastUpdatedBy = et.[LastUpdatedBy],
		@LastUpdatedAt = et.[LastUpdatedAt],
		@EntityStatusId = et.[EntityStatusId]
		FROM [dbo].[EnforcementStation] et
		WHERE et.[EnforcementStationId] = @EnforcementStationId
		AND et.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[EnforcementStation] AS et
		WHERE et.[EnforcementStationId] = @EnforcementStationId
		AND et.[EntityStatusId] = 1;
		
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementstation_getPaged]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- =============================================
CREATE PROCEDURE [dbo].[usp_enforcementstation_getPaged]
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
					WHEN 'EnforcementStationId' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementStationId] ASC)
					WHEN 'EnforcementStationName' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementStationName] ASC)
					WHEN 'EnforcementStationGuestCode' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementStationGuestCode] ASC)
				END
			WHEN 'desc' THEN
				CASE @OrderColumn 
					WHEN 'CrimeIncidentTypeId' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementStationId] DESC)
					WHEN 'EnforcementStationName' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementStationName] DESC)
					WHEN 'EnforcementStationGuestCode' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementStationGuestCode] DESC)
				END
			 END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 es.[EnforcementStationId],
			 MAX(es.[EnforcementStationName])[EnforcementStationName],
			 MAX(es.[EnforcementStationGuestCode])[EnforcementStationGuestCode],
			 MAX(f.[FileId])[FileId],
			 MAX(f.[FileName])[FileName],
			 MAX(f.[MimeType])[MimeType],
			 MAX(f.[FileSize])[FileSize],
			 MAX(f.[FileContent])[FileContent]
			FROM [dbo].[EnforcementStation] AS es
			LEFT JOIN [dbo].[File] f ON es.IconFileId = f.FileId
			WHERE es.EntityStatusId = 1
			AND (es.[EnforcementStationId] like '%' + @Search + '%' 
			OR es.[EnforcementStationName] like '%' + @Search + '%'
			)
			GROUP BY es.[EnforcementStationId]
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementstation_update]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementstation_update]
	@EnforcementStationId			NVARCHAR(100),
	@EnforcementStationName			NVARCHAR(100),
	@EnforcementStationGuestCode	NVARCHAR(100),
	@LastUpdatedBy					NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[EnforcementStation]
		SET 
		[EnforcementStationName] = @EnforcementStationName,
		[EnforcementStationGuestCode] = @EnforcementStationGuestCode,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [EnforcementStationId] = @EnforcementStationId;

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
/****** Object:  StoredProcedure [dbo].[usp_enforcementtype_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementtype_add]
	@EnforcementTypeName			NVARCHAR(100),
	@IconFileId						NVARCHAR(100),
	@CreatedBy						NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @EnforcementTypeId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'EnforcementType', @Id = @EnforcementTypeId OUTPUT

		INSERT INTO [dbo].[EnforcementType](
			[EnforcementTypeId],
			[EnforcementTypeName],
			[IconFileId],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@EnforcementTypeId,
			@EnforcementTypeName,
			@IconFileId,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @EnforcementTypeId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementtype_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementtype_delete]
	@EnforcementTypeId		NVARCHAR(100),
	@LastUpdatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		IF EXISTS(SELECT * FROM [dbo].[EnforcementUnit] WHERE [EnforcementTypeId] = @EnforcementTypeId AND [EntityStatusId] = 1)
		BEGIN
			RAISERROR('Cannot delete EnforcementType, data is used in EnforcementUnit', 16, 1)
		END

		IF EXISTS(SELECT * FROM [dbo].[EnforcementType] WHERE [EnforcementTypeId] = @EnforcementTypeId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[EnforcementType]
			SET 
			[EnforcementTypeName] = @EnforcementTypeId + ' - ' + [EnforcementTypeName] + '(DELETED - ' + CONVERT(VARCHAR(50),GETDATE())+ ')',
			EntityStatusId = 2,
			[LastUpdatedBy] = @LastUpdatedBy,
			[LastUpdatedAt] = GETDATE()
			WHERE [EnforcementTypeId] = @EnforcementTypeId;

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
/****** Object:  StoredProcedure [dbo].[usp_enforcementtype_getByID]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementtype_getByID]
	@EnforcementTypeId	NVARCHAR(100)
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
		@IconFileId = et.[IconFileId],
		@CreatedBy = et.[CreatedBy],
		@CreatedAt = et.[CreatedAt],
		@LastUpdatedBy = et.[LastUpdatedBy],
		@LastUpdatedAt = et.[LastUpdatedAt],
		@EntityStatusId = et.[EntityStatusId]
		FROM [dbo].[EnforcementType] et
		WHERE et.[EnforcementTypeId] = @EnforcementTypeId
		AND et.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[EnforcementType] AS et
		WHERE et.[EnforcementTypeId] = @EnforcementTypeId
		AND et.[EntityStatusId] = 1;
		
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementtype_getPaged]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- =============================================
CREATE PROCEDURE [dbo].[usp_enforcementtype_getPaged]
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
					WHEN 'EnforcementTypeId' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementTypeId] ASC)
					WHEN 'EnforcementTypeName' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementTypeName] ASC)
				END
			WHEN 'desc' THEN
				CASE @OrderColumn 
					WHEN 'CrimeIncidentTypeId' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementTypeId] DESC)
					WHEN 'EnforcementTypeName' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementTypeName] DESC)
				END
			 END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 et.[EnforcementTypeId],
			 MAX(et.[EnforcementTypeName])[EnforcementTypeName],
			 MAX(f.[FileId])[FileId],
			 MAX(f.[FileName])[FileName],
			 MAX(f.[MimeType])[MimeType],
			 MAX(f.[FileSize])[FileSize],
			 MAX(f.[FileContent])[FileContent]
			FROM [dbo].[EnforcementType] AS et
			LEFT JOIN [dbo].[File] f ON et.IconFileId = f.FileId
			WHERE et.EntityStatusId = 1
			AND (et.[EnforcementTypeId] like '%' + @Search + '%' 
			OR et.[EnforcementTypeName] like '%' + @Search + '%'
			)
			GROUP BY et.[EnforcementTypeId]
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementtype_update]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementtype_update]
	@EnforcementTypeId				NVARCHAR(100),
	@EnforcementTypeName			NVARCHAR(100),
	@LastUpdatedBy					NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[EnforcementType]
		SET 
		[EnforcementTypeName] = @EnforcementTypeName,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [EnforcementTypeId] = @EnforcementTypeId;

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
/****** Object:  StoredProcedure [dbo].[usp_enforcementunit_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementunit_add]
	@EnforcementTypeId		NVARCHAR(100),
	@EnforcementStationId	NVARCHAR(100),
	@LegalEntityId			NVARCHAR(100),
	@ProfilePictureFile		NVARCHAR(100),
	@CreatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @EnforcementUnitId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'EnforcementUnit', @Id = @EnforcementUnitId OUTPUT

		INSERT INTO [dbo].[EnforcementUnit](
			[EnforcementUnitId], 
			[EnforcementTypeId],
			[EnforcementStationId],
			[LegalEntityId],
			[ProfilePictureFile],
			[CreatedBy],
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@EnforcementUnitId,
			@EnforcementTypeId,
			@EnforcementStationId,
			@LegalEntityId,
			@ProfilePictureFile,
			@CreatedBy,
			GETDATE(),
			1
		);

		SELECT @EnforcementUnitId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementunit_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementunit_delete]
	@EnforcementUnitId	NVARCHAR(100),
	@LastUpdatedBy		NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[SystemUser] WHERE [IsEnforcementUnit] = 1 AND [EnforcementUnitId] = @EnforcementUnitId AND [EntityStatusId] = 1)
		BEGIN
			RAISERROR('Cannot delete EnforcementUnit, data is used in SystemUser', 16, 1)
		END
		IF EXISTS(SELECT * FROM [dbo].[EnforcementReportValidation] WHERE [EnforcementUnitId] = @EnforcementUnitId AND [EntityStatusId] = 1)
		BEGIN
			RAISERROR('Cannot delete EnforcementUnit, data is used in EnforcementReportValidation', 16, 1)
		END

		IF EXISTS(SELECT * FROM [dbo].[EnforcementUnit] WHERE [EnforcementUnitId] = @EnforcementUnitId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[EnforcementUnit]
			SET 
			EntityStatusId = 2,
			[LastUpdatedBy] = @LastUpdatedBy,
			[LastUpdatedAt] = GETDATE()
			WHERE [EnforcementUnitId] = @EnforcementUnitId;

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
/****** Object:  StoredProcedure [dbo].[usp_enforcementunit_getByID]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementunit_getByID]
	@EnforcementUnitId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @EnforcementTypeId		NVARCHAR(100);
		DECLARE @EnforcementStationId	NVARCHAR(100);
		DECLARE @LegalEntityId			NVARCHAR(100);
		DECLARE @ProfilePictureFile		NVARCHAR(100);
		DECLARE @GenderId				BIGINT;
		DECLARE @CreatedBy				NVARCHAR(100);
		DECLARE @CreatedAt				DATETIME;
		DECLARE @LastUpdatedBy			NVARCHAR(100);
		DECLARE @LastUpdatedAt			DATETIME;
		DECLARE @EntityStatusId			BIGINT;
		
		SELECT 
		@EnforcementTypeId = eu.[EnforcementTypeId],
		@EnforcementStationId = eu.[EnforcementStationId],
		@LegalEntityId = eu.[LegalEntityId],
		@ProfilePictureFile = eu.[ProfilePictureFile],
		@CreatedBy = eu.[CreatedBy],
		@CreatedAt = eu.[CreatedAt],
		@LastUpdatedBy = eu.[LastUpdatedBy],
		@LastUpdatedAt = eu.[LastUpdatedAt],
		@EntityStatusId = eu.[EntityStatusId]
		FROM [dbo].[EnforcementUnit] eu
		WHERE eu.[EnforcementUnitId] = @EnforcementUnitId
		AND eu.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[EnforcementUnit] AS eu
		WHERE eu.[EnforcementUnitId] = @EnforcementUnitId
		AND eu.[EntityStatusId] = 1;

		SELECT  
		et.[EnforcementTypeId],
		et.[EnforcementTypeName]
		FROM [dbo].[EnforcementType] AS et
		WHERE et.[EnforcementTypeId] = @EnforcementTypeId;

		SELECT  
		es.[EnforcementStationId],
		es.[EnforcementStationName]
		FROM [dbo].[EnforcementStation] AS es
		WHERE es.[EnforcementStationId] = @EnforcementStationId;
		
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementunit_getByLegalEntityId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementunit_getByLegalEntityId]
	@LegalEntityId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @EnforcementUnitId		NVARCHAR(100);
		DECLARE @EnforcementTypeId		NVARCHAR(100);
		DECLARE @EnforcementStationId	NVARCHAR(100);
		DECLARE @ProfilePictureFile		NVARCHAR(100);
		DECLARE @GenderId				BIGINT;
		DECLARE @CreatedBy				NVARCHAR(100);
		DECLARE @CreatedAt				DATETIME;
		DECLARE @LastUpdatedBy			NVARCHAR(100);
		DECLARE @LastUpdatedAt			DATETIME;
		DECLARE @EntityStatusId			BIGINT;
		
		SELECT 
		@EnforcementUnitId = eu.[EnforcementUnitId],
		@EnforcementTypeId = eu.[EnforcementTypeId],
		@EnforcementStationId = eu.[EnforcementStationId],
		@ProfilePictureFile = eu.[ProfilePictureFile],
		@CreatedBy = eu.[CreatedBy],
		@CreatedAt = eu.[CreatedAt],
		@LastUpdatedBy = eu.[LastUpdatedBy],
		@LastUpdatedAt = eu.[LastUpdatedAt],
		@EntityStatusId = eu.[EntityStatusId]
		FROM [dbo].[EnforcementUnit] eu
		WHERE eu.[LegalEntityId] = @LegalEntityId
		AND eu.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[EnforcementUnit] AS eu
		WHERE eu.[EnforcementUnitId] = @EnforcementUnitId
		AND eu.[EntityStatusId] = 1;

		SELECT  
		et.[EnforcementTypeId],
		et.[EnforcementTypeName]
		FROM [dbo].[EnforcementType] AS et
		WHERE et.[EnforcementTypeId] = @EnforcementTypeId;

		SELECT  
		es.[EnforcementStationId],
		es.[EnforcementStationName]
		FROM [dbo].[EnforcementStation] AS es
		WHERE es.[EnforcementStationId] = @EnforcementStationId;
		
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementunit_getLookupByEnforcementStationId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementunit_getLookupByEnforcementStationId]
	@EnforcementStationId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		SELECT 'EnforcementUnit' AS LookupName,CAST(eu.[EnforcementUnitId] AS nvarchar(100)) AS Id,CONCAT(ISNULL(le.[FirstName], ''), ' ', ISNULL(le.[MiddleName], ''), ' ', ISNULL(le.[LastName], '')) AS Name FROM [dbo].[EnforcementUnit] eu
		LEFT JOIN [dbo].[LegalEntity] le ON eu.LegalEntityId = le.LegalEntityId
		WHERE eu.EnforcementStationId = @EnforcementStationId
		AND eu.EntityStatusId = 1;

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
/****** Object:  StoredProcedure [dbo].[usp_enforcementunit_getPaged]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erwin
-- Create date: 2020-09-16
-- Description:	filter contract list by location
-- =============================================
CREATE PROCEDURE [dbo].[usp_enforcementunit_getPaged]
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
					WHEN 'EnforcementUnitId' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementUnitId] ASC)
					WHEN 'EnforcementType.EnforcementTypeName' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementTypeName] ASC)
					WHEN 'EnforcementStation.EnforcementStationName' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementStationName] ASC)
					WHEN 'LegalEntity.FullName' THEN ROW_NUMBER() OVER(ORDER BY [FullName] ASC)
					WHEN 'LegalEntity.Gender.GenderName' THEN ROW_NUMBER() OVER(ORDER BY [GenderName] ASC)
					WHEN 'LegalEntity.Age' THEN ROW_NUMBER() OVER(ORDER BY [Age] ASC)
					WHEN 'LegalEntity.EmailAddress' THEN ROW_NUMBER() OVER(ORDER BY [EmailAddress] ASC)
					WHEN 'LegalEntity.MobileNumber' THEN ROW_NUMBER() OVER(ORDER BY [MobileNumber] ASC)
				END
			WHEN 'desc' THEN
				CASE @OrderColumn 
					WHEN 'EnforcementUnitId' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementUnitId] DESC)
					WHEN 'EnforcementType.EnforcementTypeName' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementTypeName] ASC)
					WHEN 'EnforcementStation.EnforcementStationName' THEN ROW_NUMBER() OVER(ORDER BY [EnforcementStationName] ASC)
					WHEN 'LegalEntity.FullName' THEN ROW_NUMBER() OVER(ORDER BY [FullName] ASC)
					WHEN 'LegalEntity.Gender.GenderName' THEN ROW_NUMBER() OVER(ORDER BY [GenderName] ASC)
					WHEN 'LegalEntity.Age' THEN ROW_NUMBER() OVER(ORDER BY [Age] ASC)
					WHEN 'LegalEntity.EmailAddress' THEN ROW_NUMBER() OVER(ORDER BY [EmailAddress] ASC)
					WHEN 'LegalEntity.MobileNumber' THEN ROW_NUMBER() OVER(ORDER BY [MobileNumber] ASC)
				END
			 END) AS row_num ,
			count(*) over() as TotalRows
			FROM (
			 SELECT 
			 eu.[EnforcementUnitId],
			 MAX(et.[EnforcementTypeId])[EnforcementTypeId],
			 MAX(et.[EnforcementTypeName])[EnforcementTypeName],
			 MAX(es.[EnforcementStationId])[EnforcementStationId],
			 MAX(es.[EnforcementStationName])[EnforcementStationName],
			 MAX(le.[LegalEntityId])[LegalEntityId],
			 MAX(le.[FullName])[FullName],
			 MAX(le.[BirthDate])[BirthDate],
			 MAX(le.[Age])[Age],
			 MAX(le.[EmailAddress])[EmailAddress],
			 MAX(le.[MobileNumber])[MobileNumber],
			 MAX(eg.[GenderId])[GenderId],
			 MAX(eg.[GenderName])[GenderName],
			 MAX(f.[FileId])[FileId],
			 MAX(f.[FileName])[FileName],
			 MAX(f.[MimeType])[MimeType],
			 MAX(f.[FileSize])[FileSize],
			 MAX(f.[FileContent])[FileContent]
			FROM [dbo].[EnforcementUnit] AS eu
			LEFT JOIN (SELECT *,ISNULL([FirstName],'') + ' ' + ISNULL([MiddleName],'') + ' ' + ISNULL([LastName],'') AS [FullName] FROM [dbo].[LegalEntity] WHERE EntityStatusId = 1) AS le ON eu.LegalEntityId = le.LegalEntityId
			LEFT JOIN [dbo].[EnforcementType] AS et ON eu.[EnforcementTypeId] = et.[EnforcementTypeId]
			LEFT JOIN [dbo].[EnforcementStation] AS es ON eu.[EnforcementStationId] = es.[EnforcementStationId]
			LEFT JOIN [dbo].[EntityGender] AS eg ON le.GenderId = eg.GenderId
			LEFT JOIN [dbo].[File] AS f ON eu.[ProfilePictureFile] = f.[FileId]
			WHERE eu.EntityStatusId = 1
			AND (eu.[EnforcementUnitId] like '%' + @Search + '%' 
			OR et.[EnforcementTypeName] like '%' + @Search + '%' 
			OR es.[EnforcementStationName] like '%' + @Search + '%' 
			OR le.[FullName] like '%' + @Search + '%' 
			OR eg.[GenderName] like '%' + @Search + '%'
			OR le.[Age] like '%' + @Search + '%' 
			OR le.[EmailAddress] like '%' + @Search + '%'
			OR le.[MobileNumber] like '%' + @Search + '%' )
			GROUP BY eu.[EnforcementUnitId]
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
/****** Object:  StoredProcedure [dbo].[usp_enforcementunit_update]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_enforcementunit_update]
	@EnforcementUnitId		NVARCHAR(100),
	@EnforcementTypeId		NVARCHAR(100),
	@EnforcementStationId	NVARCHAR(100),
	@LastUpdatedBy			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[EnforcementUnit]
		SET 
		[EnforcementTypeId] = @EnforcementTypeId,
		[EnforcementStationId] = @EnforcementStationId,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [EnforcementUnitId] = @EnforcementUnitId;

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
/****** Object:  StoredProcedure [dbo].[usp_file_add]    Script Date: 12/7/2021 1:28:34 PM ******/
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
	@FileContent		VARBINARY(MAX),
	@IsFromStorage		BIT,
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
			[FileContent],
			[IsFromStorage],
			[CreatedBy], 
			[CreatedAt],
			[EntityStatusId]
		)
		VALUES(
			@FileId,
			@FileName, 
			@MimeType,
			@FileContent,
			@IsFromStorage,
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
/****** Object:  StoredProcedure [dbo].[usp_file_getById]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_file_update]    Script Date: 12/7/2021 1:28:34 PM ******/
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
	@FileContent		VARBINARY(MAX),
	@IsFromStorage		BIT,
	@LastUpdatedBy		NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
	
		UPDATE [dbo].[File] SET 
		[FileName] = @FileName, 
		[MimeType] = @MimeType, 
		[FileContent] = @FileContent, 
		[IsFromStorage] = @IsFromStorage, 
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
/****** Object:  StoredProcedure [dbo].[usp_legalentity_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalentity_add]
	@FirstName			NVARCHAR(100) = '',
	@LastName			NVARCHAR(100) = '',
	@MiddleName			NVARCHAR(100) = '',
	@GenderId			NVARCHAR(100) = 1,
	@BirthDate			DATE = GETDATE,
	@EmailAddress		NVARCHAR(100),
	@MobileNumber		BIGINT = 0
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
			ISNULL(@MiddleName, ''),
			@GenderId,
			@BirthDate,
			@EmailAddress,
			ISNULL(@MobileNumber, 0),
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
/****** Object:  StoredProcedure [dbo].[usp_legalentity_update]    Script Date: 12/7/2021 1:28:34 PM ******/
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
	@EmailAddress		NVARCHAR(100),
	@MobileNumber		NVARCHAR(100),
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
			[BirthDate] = @BirthDate,
			[EmailAddress] = @EmailAddress,
			[MobileNumber] = @MobileNumber
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
/****** Object:  StoredProcedure [dbo].[usp_legalentityaddress_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalentityaddress_add]
	@LegalEntityId		NVARCHAR(100),
	@Address			NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @LegalEntityAddressId nvarchar(100);
		
		exec [dbo].[usp_sequence_getNextCode] 'LegalEntityAddress', @Id = @LegalEntityAddressId OUTPUT

		INSERT INTO [dbo].[LegalEntityAddress](
			[LegalEntityAddressId],
			[LegalEntityId],
			[Address],
			[EntityStatusId]
		)
		VALUES(
			@LegalEntityAddressId,
			@LegalEntityId,
			@Address,
			1
		);

		SELECT @LegalEntityAddressId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_legalentityaddress_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalentityaddress_delete]
	@LegalEntityAddressId		NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[LegalEntityAddress] WHERE [LegalEntityAddressId] = @LegalEntityAddressId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[LegalEntityAddress]
			SET 
			[Address] = @LegalEntityAddressId + ' - ' + [Address] + '(DELETED - ' + CONVERT(VARCHAR(50),GETDATE())+ ')',
			EntityStatusId = 2
			WHERE [LegalEntityAddressId] = @LegalEntityAddressId;

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
/****** Object:  StoredProcedure [dbo].[usp_legalentityaddress_getById]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalentityaddress_getById]
	@LegalEntityAddressId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @LegalEntityId	NVARCHAR(100);
		DECLARE @EntityStatusId	BIGINT;
		
		SELECT 
		@LegalEntityId = lea.[LegalEntityId],
		@EntityStatusId = lea.[EntityStatusId]
		FROM [dbo].[LegalEntityAddress] lea
		WHERE lea.[LegalEntityAddressId] = @LegalEntityAddressId
		AND lea.[EntityStatusId] = 1;
		
		SELECT  *
		FROM [dbo].[LegalEntityAddress] AS lea
		WHERE lea.[LegalEntityAddressId] = @LegalEntityAddressId
		AND lea.[EntityStatusId] = 1;
		
		SELECT le.*
		FROM [dbo].[LegalEntity] le
		WHERE le.[LegalEntityId] = @LegalEntityId
		AND le.[EntityStatusId] = 1;

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
/****** Object:  StoredProcedure [dbo].[usp_legalentityaddress_getByLegalEntityId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalentityaddress_getByLegalEntityId]
	@LegalEntityId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		SELECT 
		lea.[LegalEntityAddressId],
		lea.[Address],
		lea.[EntityStatusId],
		le.*
		FROM [dbo].[LegalEntity] le
		LEFT JOIN [dbo].[LegalEntityAddress] lea ON le.[LegalEntityId] = lea.[LegalEntityId]
		WHERE le.LegalEntityId = @LegalEntityId
		AND lea.[EntityStatusId] = 1;
		
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
/****** Object:  StoredProcedure [dbo].[usp_legalentityaddress_getBySystemUserId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalentityaddress_getBySystemUserId]
	@SystemUserId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		SELECT 
		lea.[LegalEntityAddressId],
		lea.[Address],
		lea.[EntityStatusId],
		le.*
		FROM [dbo].[SystemUser] su
		LEFT JOIN [dbo].[LegalEntity] le ON su.[LegalEntityId] = le.[LegalEntityId]
		LEFT JOIN [dbo].[LegalEntityAddress] lea ON le.[LegalEntityId] = lea.[LegalEntityId]
		WHERE su.[SystemUserId] = @SystemUserId
		AND lea.[EntityStatusId] = 1;
		
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
/****** Object:  StoredProcedure [dbo].[usp_legalentityaddress_update]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_legalentityaddress_update]
	@LegalEntityAddressId	NVARCHAR(100),
	@Address				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[LegalEntityAddress]
		SET 
		[Address] = @Address
		WHERE [LegalEntityAddressId] = @LegalEntityAddressId;

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
/****** Object:  StoredProcedure [dbo].[usp_lookuptable_getByTableNames]    Script Date: 12/7/2021 1:28:34 PM ******/
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

		SELECT 'EntityGender' AS LookupName,CAST([GenderId] AS nvarchar(100)) AS Id,[GenderName] AS Name FROM [dbo].[EntityGender]
		WHERE 'EntityGender' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )

		UNION ALL

		SELECT 'EntityStatus' AS LookupName,CAST([EntityStatusId] AS nvarchar(100)) AS Id,[EntityStatusName] AS Name FROM [dbo].[EntityStatus]
		WHERE 'EntityStatus' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )

		UNION ALL

		SELECT 'SystemConfigType' AS LookupName,CAST([SystemConfigTypeId] AS nvarchar(100)) AS Id,[ValueType] AS Name FROM [dbo].[SystemConfigType]
		WHERE 'SystemConfigType' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )

		UNION ALL

		SELECT 'SystemConfig' AS LookupName,CAST([ConfigKey] AS nvarchar(100)) AS Id,[ConfigValue] FROM [dbo].[SystemConfig]
		WHERE 'SystemConfig' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )

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
		
		UNION ALL

		SELECT 'CrimeIncidentCategory' AS LookupName,[CrimeIncidentCategoryId] AS Id,[CrimeIncidentCategoryName] AS Name FROM [dbo].[CrimeIncidentCategory]
		WHERE [EntityStatusId] = 1 AND 'CrimeIncidentCategory' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )
		
		UNION ALL

		SELECT 'EnforcementType' AS LookupName,[EnforcementTypeId] AS Id,[EnforcementTypeName] AS Name FROM [dbo].[EnforcementType]
		WHERE [EntityStatusId] = 1 AND 'EnforcementType' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )
		
		UNION ALL

		SELECT 'EnforcementStation' AS LookupName,[EnforcementStationId] AS Id,[EnforcementStationName] AS Name FROM [dbo].[EnforcementStation]
		WHERE [EntityStatusId] = 1 AND 'EnforcementStation' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )
		
		UNION ALL

		SELECT 'EntityApprovalStatus' AS LookupName,CAST([ApprovalStatusId] AS nvarchar(100)) AS Id,[ApprovalStatusName] AS Name FROM [dbo].[EntityApprovalStatus]
		WHERE 'EntityApprovalStatus' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )
		
		UNION ALL

		SELECT 'DocReportMediaType' AS LookupName,CAST([DocReportMediaTypeId] AS nvarchar(100)) AS Id,CAST([DocReportMediaTypeName] AS nvarchar(100)) AS Name FROM [dbo].[DocReportMediaType]
		WHERE 'DocReportMediaType' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )
		
		UNION ALL

		SELECT 'ReportValidationStatus' AS LookupName,CAST([ReportValidationStatusId] AS nvarchar(100)) AS Id,CAST([ReportValidationStatusName] AS nvarchar(100)) AS Name FROM [dbo].[ReportValidationStatus]
		WHERE 'ReportValidationStatus' IN (select Item from [dbo].[tvf_SplitString](@TableNames,',') )
		
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
/****** Object:  StoredProcedure [dbo].[usp_Reset]    Script Date: 12/7/2021 1:28:34 PM ******/
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
		
		DELETE FROM [dbo].[CrimeIncidentReportMedia];
		DELETE FROM [dbo].[EnforcementReportValidation];
		DELETE FROM [dbo].[CrimeIncidentReport];
		DELETE FROM [dbo].[EnforcementType];
		DELETE FROM [dbo].[EnforcementStation];
		DELETE FROM [dbo].[EnforcementUnit];
		DELETE FROM [dbo].[SystemWebAdminUserRole];
		DELETE FROM [dbo].[SystemWebAdminRolePrivileges];
		DELETE FROM [dbo].[SystemWebAdminMenuRole];
		DELETE FROM [dbo].[SystemUserContact];
		DELETE FROM [dbo].[SystemUserConfig];
		DELETE FROM [dbo].[SystemWebAdminRole];
		DELETE FROM [dbo].[SystemUser];
		DELETE FROM [dbo].[LegalEntityAddress];
		DELETE FROM [dbo].[LegalEntity];
		DELETE FROM [dbo].[CrimeIncidentCategory];
		DELETE FROM [dbo].[CrimeIncidentType];
		DELETE FROM [dbo].[File];
		DELETE FROM [dbo].[SystemToken];
		DBCC CHECKIDENT ('SystemToken', RESEED, 0)
		DELETE FROM [dbo].[SystemUserVerification];
		DBCC CHECKIDENT ('SystemUserVerification', RESEED, 0)
		
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemWebAdminRolePrivileges';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemWebAdminUserRole';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemWebAdminMenuRole';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemUserContact';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemUserConfig';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemWebAdminRole';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'SystemUser';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'LegalEntityAddress';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'LegalEntity';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'CrimeIncidentCategory';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'CrimeIncidentType';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'File';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'EnforcementType';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'EnforcementStation';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'EnforcementUnit';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'CrimeIncidentReportMedia';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'CrimeIncidentReport';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'EnforcementReportValidation';
        
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
/****** Object:  StoredProcedure [dbo].[usp_Reset_Activity]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_Reset_Activity]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
		SET NOCOUNT ON;
		
		DECLARE @MediaFileTable TABLE
		(
			FileId NVARCHAR(100)
		)
		
		INSERT INTO @MediaFileTable (FileId)
		SELECT FileId FROM [dbo].[CrimeIncidentReportMedia];

		DELETE FROM [dbo].[CrimeIncidentReportMedia];
		DELETE FROM [dbo].[EnforcementReportValidation];
		DELETE FROM [dbo].[CrimeIncidentReport];
		DELETE FROM [dbo].[File] WHERE FileId IN (SELECT FileId FROM @MediaFileTable);
		
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'CrimeIncidentReportMedia';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'CrimeIncidentReport';
		Update [dbo].[Sequence] set [LastNumber] = 0 WHERE [TableName] = 'EnforcementReportValidation';
        
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
/****** Object:  StoredProcedure [dbo].[usp_sequence_getNextCode]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemtoken_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemtoken_add]
	@TokenId			NVARCHAR(MAX),
	@SystemUserId		NVARCHAR(100),
	@ClientId			NVARCHAR(100),
	@Subject			NVARCHAR(100),
	@IssuedUtc			DATETIME,
	@ExpiresUtc			DATETIME,
	@ProtectedTicket	NVARCHAR(MAX),
	@TokenType			NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		IF(EXISTS(SELECT * FROM [dbo].[SystemToken] WHERE [SystemUserId] = @SystemUserId))
		BEGIN
			UPDATE [dbo].[SystemToken] SET 
			[TokenId]=@TokenId,
			[ClientId]=@ClientId,
			[Subject]=@Subject,
			[IssuedUtc]=@IssuedUtc,
			[ExpiresUtc]=@ExpiresUtc,
			[ProtectedTicket]=@ProtectedTicket,
			[TokenType]=@TokenType
			WHERE [SystemUserId] = @SystemUserId;
			SELECT 1;
		END
		ELSE
		BEGIN	
			INSERT INTO [dbo].[SystemToken](
				[TokenId], 
				[SystemUserId], 
				[ClientId],
				[Subject],
				[IssuedUtc],
				[ExpiresUtc],
				[ProtectedTicket],
				[TokenType]
			)
			VALUES(
				@TokenId,
				@SystemUserId,
				@ClientId,
				@Subject,
				@IssuedUtc,
				@ExpiresUtc,
				@ProtectedTicket,
				@TokenType
			);
			SELECT SCOPE_IDENTITY();
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
/****** Object:  StoredProcedure [dbo].[usp_systemtoken_getById]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemtoken_getById]
	@HashedTokenId	NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		DECLARE @SystemUserId NVARCHAR(100);
		
		SELECT 
		@SystemUserId = st.[SystemUserId]
		FROM [dbo].[SystemToken] st
		WHERE st.[TokenId] = @HashedTokenId
		
		SELECT  *
		FROM [dbo].[SystemToken] AS st
		WHERE st.[TokenId] = @HashedTokenId;

		exec usp_systemuser_getByID @SystemUserId

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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_add]    Script Date: 12/7/2021 1:28:34 PM ******/
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
	@LegalEntityId			NVARCHAR(100),
	@ProfilePictureFile		NVARCHAR(100),
	@UserName				NVARCHAR(100),
	@Password				NVARCHAR(100),
	@EnforcementStationId	NVARCHAR(100),
	@IsEnforcementUnit		BIT,
	@EnforcementUnitId		NVARCHAR(100),
	@CreatedBy				NVARCHAR(100)
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
			[EnforcementStationId],
			[IsEnforcementUnit],
			[EnforcementUnitId],
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
			@EnforcementStationId,
			@IsEnforcementUnit,
			@EnforcementUnitId,
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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_changePassword]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_changePassword]
	@SystemUserId	NVARCHAR(100),
	@Password		NVARCHAR(100),
	@LastUpdatedBy	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		-- UPDATE HERE
		UPDATE [dbo].[SystemUser]
		SET 
		[Password] = HashBytes('MD5', @Password),
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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_changeUsername]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_changeUsername]
	@SystemUserId	NVARCHAR(100),
	@UserName		NVARCHAR(100),
	@LastUpdatedBy	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		-- UPDATE HERE
		UPDATE [dbo].[SystemUser]
		SET 
		[UserName] = @UserName,
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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_createAccount]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_createAccount]
	@SystemUserTypeId		NVARCHAR(100),
	@LegalEntityId			NVARCHAR(100),
	@ProfilePictureFile		NVARCHAR(100),
	@UserName				NVARCHAR(100),
	@Password				NVARCHAR(100),
	@IsWebAdminGuestUser	BIT,
	@EnforcementStationId	NVARCHAR(100)
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
			[IsWebAdminGuestUser],
			[EnforcementStationId],
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
			@IsWebAdminGuestUser,
			@EnforcementStationId,
			@SystemUserId,
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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getByCredentials]    Script Date: 12/7/2021 1:28:34 PM ******/
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
		DECLARE @EnforcementUnitId		NVARCHAR(100);
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
		@EnforcementUnitId = su.[EnforcementUnitId],
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

		exec [dbo].[usp_systemuserconfig_getBySystemUserId] @SystemUserId

		exec [dbo].[usp_systemwebadminuserroles_getBySystemUserId] @SystemUserId

		exec [dbo].[usp_systemwebadminmenuroles_getBySystemUserId] @SystemUserId

		exec [dbo].[usp_systemwebadminroleprivileges_getBySystemUserId] @SystemUserId

		exec [dbo].[usp_enforcementunit_getByID] @EnforcementUnitId

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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getByID]    Script Date: 12/7/2021 1:28:34 PM ******/
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
		DECLARE @EnforcementUnitId		NVARCHAR(100);
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
		@EnforcementUnitId = su.[EnforcementUnitId],
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

		exec [dbo].[usp_systemuserconfig_getBySystemUserId] @SystemUserId

		exec [dbo].[usp_systemwebadminuserroles_getBySystemUserId] @SystemUserId

		exec [dbo].[usp_systemwebadminmenuroles_getBySystemUserId] @SystemUserId

		exec [dbo].[usp_systemwebadminroleprivileges_getBySystemUserId] @SystemUserId
		
		exec [dbo].[usp_enforcementunit_getByID] @EnforcementUnitId

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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getByUsername]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_getByUsername]
	@Username	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @SystemUserId			NVARCHAR(100);
		DECLARE @SystemUserTypeId		BIGINT;
		DECLARE @LegalEntityId			NVARCHAR(100);
		DECLARE @EnforcementUnitId		NVARCHAR(100);
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
		@LegalEntityId = su.[LegalEntityId],
		@EnforcementUnitId = su.[EnforcementUnitId],
		@ProfilePictureFile = su.[ProfilePictureFile],
		@CreatedBy = su.[CreatedBy],
		@CreatedAt = su.[CreatedAt],
		@LastUpdatedBy = su.[LastUpdatedBy],
		@LastUpdatedAt = su.[LastUpdatedAt],
		@EntityStatusId = su.[EntityStatusId]
		FROM [dbo].[SystemUser] su
		WHERE su.[UserName] = @Username
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

		exec [dbo].[usp_systemuserconfig_getBySystemUserId] @SystemUserId

		exec [dbo].[usp_systemwebadminuserroles_getBySystemUserId] @SystemUserId

		exec [dbo].[usp_systemwebadminmenuroles_getBySystemUserId] @SystemUserId

		exec [dbo].[usp_systemwebadminroleprivileges_getBySystemUserId] @SystemUserId
		
		exec [dbo].[usp_enforcementunit_getByID] @EnforcementUnitId

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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getPaged]    Script Date: 12/7/2021 1:28:34 PM ******/
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
	@Search					NVARCHAR(50) = '',
	@SystemUserType			BIGINT = 1,
	@ApprovalStatus			NVARCHAR(50),
	@PageNo					BIGINT = 1,
	@PageSize				BIGINT = 10,
	@OrderColumn			NVARCHAR(100),
	@OrderDir				NVARCHAR(5)
AS
BEGIN
	SET NOCOUNT ON;
		DECLARE @IsWebAdminGuestUser NVARCHAR(10);
		SET @IsWebAdminGuestUser = IIF(CAST(@ApprovalStatus AS NVARCHAR(10)) <> 2, IIF(CAST(@ApprovalStatus AS NVARCHAR(10)) <> 1, 1, 0), NULL);
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
			 MAX(IIF(su.[IsWebAdminGuestUser] <> 1, 0, 1))[IsWebAdminGuestUser],
			 MAX(pf.[FileId])[FileId],
			 MAX(pf.[FileName])[FileName],
			 MAX(pf.[MimeType])[MimeType],
			 MAX(pf.[FileSize])[FileSize],
			 MAX(pf.[FileContent])[FileContent],
			 MAX(IIF(pf.[IsFromStorage] <> 1, 0, 1))[IsFromStorage],
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
			LEFT JOIN [dbo].[File] AS pf ON su.ProfilePictureFile = pf.FileId
			LEFT JOIN [dbo].[SystemUserType] AS sut ON su.SystemUserTypeId = sut.SystemUserTypeId
			LEFT JOIN [dbo].[EntityGender] AS eg ON le.GenderId = eg.GenderId
			WHERE su.EntityStatusId = 1
			AND su.SystemUserTypeId = @SystemUserType
			--AND su.IsWebAdminGuestUser LIKE (
			--								CASE 
			--									WHEN @ApprovalStatus = 0 THEN 1 
			--									WHEN @ApprovalStatus = 1 THEN 0 
			--									WHEN @ApprovalStatus = 2 THEN '%%'
			--								END
			--								)
			AND su.IsWebAdminGuestUser like '%'+ ISNULL(@IsWebAdminGuestUser, '') +'%'
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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_getTrackerStatus]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_getTrackerStatus]
	@SystemUserId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		SELECT *
		FROM [dbo].[SystemUser] AS su
		WHERE su.[SystemUserId] = @SystemUserId
		AND su.[EntityStatusId] = 1;

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
/****** Object:  StoredProcedure [dbo].[usp_systemuser_update]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuser_update]
	@SystemUserId			NVARCHAR(100),
	@IsWebAdminGuestUser	BIT,
	@EnforcementStationId	NVARCHAR(100),
	@IsEnforcementUnit		BIT,
	@EnforcementUnitId		NVARCHAR(100),
	@ProfilePictureFile		NVARCHAR(100),
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
		[IsWebAdminGuestUser] = @IsWebAdminGuestUser,
		[IsEnforcementUnit] = @IsEnforcementUnit,
		[EnforcementStationId] = @EnforcementStationId,
		[EnforcementUnitId] = @EnforcementUnitId,
		[ProfilePictureFile] = (CASE WHEN ISNULL(@ProfilePictureFile, '') <> '' THEN @ProfilePictureFile END) ,
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
/****** Object:  StoredProcedure [dbo].[usp_systemuserconfig_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuserconfig_add]
	@SystemUserId					NVARCHAR(100),
	@IsUserEnable					BIT,
	@IsUserAllowToPostNextReport	BIT,
	@IsNextReportPublic				BIT,
	@IsAnonymousNextReport			BIT,
	@AllowReviewActionNextPost		BIT,
	@AllowReviewCommentNextPost		BIT,
	@IsAllReportPublic				BIT,
	@IsAnonymousAllReport			BIT,
	@AllowReviewActionAllReport		BIT,
	@AllowReviewCommentAllReport	BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @SystemUserConfigId NVARCHAR(100);

		exec [dbo].[usp_sequence_getNextCode] 'SystemUserConfig', @Id = @SystemUserConfigId OUTPUT
		
		INSERT INTO [dbo].[SystemUserConfig](
			[SystemUserConfigId], 
			[SystemUserId], 
			[IsUserEnable],
			[IsUserAllowToPostNextReport],
			[IsNextReportPublic],
			[IsAnonymousNextReport],
			[AllowReviewActionNextPost],
			[AllowReviewCommentNextPost],
			[IsAllReportPublic],
			[IsAnonymousAllReport],
			[AllowReviewActionAllReport],
			[AllowReviewCommentAllReport]
		)
		VALUES(
			@SystemUserConfigId,
			@SystemUserId,
			@IsUserEnable,
			@IsUserAllowToPostNextReport,
			@IsNextReportPublic,
			@IsAnonymousNextReport,
			@AllowReviewActionNextPost,
			@AllowReviewCommentNextPost,
			@IsAllReportPublic,
			@IsAnonymousAllReport,
			@AllowReviewActionAllReport,
			@AllowReviewCommentAllReport
		);

		SELECT @SystemUserConfigId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemuserconfig_getBySystemUserId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuserconfig_getBySystemUserId]
	@SystemUserId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		SELECT  *
		FROM [dbo].[SystemUserConfig] AS suc
		WHERE suc.[SystemUserId] = @SystemUserId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemuserconfig_update]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuserconfig_update]
	@SystemUserConfigId				NVARCHAR(100),
	@IsUserEnable					BIT,
	@IsUserAllowToPostNextReport	BIT,
	@IsNextReportPublic				BIT,
	@IsAnonymousNextReport			BIT,
	@AllowReviewActionNextPost		BIT,
	@AllowReviewCommentNextPost		BIT,
	@IsAllReportPublic				BIT,
	@IsAnonymousAllReport			BIT,
	@AllowReviewActionAllReport		BIT,
	@AllowReviewCommentAllReport	BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		UPDATE [dbo].[SystemUserConfig]
		SET 
		[IsUserEnable] = @IsUserEnable,
		[IsUserAllowToPostNextReport] = @IsUserAllowToPostNextReport,
		[IsNextReportPublic] = @IsNextReportPublic,
		[IsAnonymousNextReport] = @IsAnonymousNextReport,
		[AllowReviewActionNextPost] = @AllowReviewActionNextPost,
		[AllowReviewCommentNextPost] = @AllowReviewActionNextPost,
		[IsAllReportPublic] = @AllowReviewActionNextPost,
		[IsAnonymousAllReport] = @AllowReviewActionNextPost,
		[AllowReviewActionAllReport] = @AllowReviewActionNextPost,
		[AllowReviewCommentAllReport] = @AllowReviewActionNextPost
		WHERE [SystemUserConfigId] = @SystemUserConfigId;

		SELECT @@ROWCOUNT;

		SELECT @SystemUserConfigId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemuserverification_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuserverification_add]
	@VerificationSender		NVARCHAR(100),
	@VerificationTypeId		BIGINT,
	@VerificationCode		NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		IF EXISTS(SELECT 1 From [SystemUserVerification] WHERE [VerificationSender] = @VerificationSender)
		BEGIN
			DECLARE @id BIGINT;
			SELECT @id = (SELECT [Id] From [SystemUserVerification] WHERE [VerificationSender] = @VerificationSender);
			UPDATE [dbo].[SystemUserVerification] SET 
			[VerificationCode] = @VerificationCode
			WHERE [Id] = @id

			SELECT @id;
			
		END
		ELSE
		BEGIN
			INSERT INTO [dbo].[SystemUserVerification](
				[VerificationSender], 
				[VerificationTypeId], 
				[VerificationCode],
				[EntityStatusId]
			)
			VALUES(
				@VerificationSender,
				@VerificationTypeId,
				@VerificationCode,
				1
			);
		
			SELECT SCOPE_IDENTITY();
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
/****** Object:  StoredProcedure [dbo].[usp_systemuserverification_getByID]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuserverification_getByID]
	@Id	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		SELECT  *
		FROM [dbo].[SystemUserVerification] AS suv
		WHERE suv.[Id] = @Id
		AND suv.[EntityStatusId] = 1;

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
/****** Object:  StoredProcedure [dbo].[usp_systemuserverification_getBySender]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuserverification_getBySender]
	@VerificationSender	NVARCHAR(100),
	@VerificationCode	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		SELECT  *
		FROM [dbo].[SystemUserVerification] AS suv
		WHERE suv.[VerificationSender] = @VerificationSender
		AND suv.[VerificationCode] = @VerificationCode
		AND suv.[EntityStatusId] = 1;

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
/****** Object:  StoredProcedure [dbo].[usp_systemuserverification_verifyUser]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemuserverification_verifyUser]
	@Id		BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[SystemUserVerification] WHERE [Id] = @Id AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[SystemUserVerification]
			SET 
			[IsVerified] = 1
			WHERE [Id] = @Id;

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenumodule_getBySystemWebAdminMenuId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminmenumodule_getBySystemWebAdminMenuId]
	@SystemWebAdminMenuId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		DECLARE @SystemWebAdminModuleId BIGINT;

		SELECT @SystemWebAdminModuleId = [SystemWebAdminModuleId]
		FROM [dbo].[SystemWebAdminMenu]
		WHERE [SystemWebAdminMenuId] = @SystemWebAdminMenuId;
		
		SELECT [SystemWebAdminModuleId],[SystemWebAdminModuleName]
		FROM [dbo].[SystemWebAdminModule]
		WHERE [SystemWebAdminModuleId] = @SystemWebAdminModuleId;
		
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_add]    Script Date: 12/7/2021 1:28:34 PM ******/
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
		
		IF EXISTS(SELECT * FROM [dbo].[SystemWebAdminMenuRole] WHERE [SystemWebAdminMenuId] = @SystemWebAdminMenuId AND [SystemWebAdminRoleId] = @SystemWebAdminRoleId)
		BEGIN
			SELECT @SystemWebAdminMenuRoleId = [SystemWebAdminMenuRoleId] FROM [dbo].[SystemWebAdminMenuRole] WHERE [SystemWebAdminMenuId] = @SystemWebAdminMenuId AND [SystemWebAdminRoleId] = @SystemWebAdminRoleId;
			UPDATE [dbo].[SystemWebAdminMenuRole] SET IsAllowed = 1, EntityStatusId = 1 WHERE [SystemWebAdminMenuRoleId] = @SystemWebAdminMenuRoleId;
		END
		ELSE
		BEGIN
		
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

		END

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_getBySystemUserId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminmenuroles_getBySystemUserId]
	@SystemUserId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		SELECT 
		swam.[SystemWebAdminMenuId],
		MAX(swam.[SystemWebAdminMenuName])[SystemWebAdminMenuName],
		MAX(swamod.[SystemWebAdminModuleId])[SystemWebAdminModuleId],
		MAX(swamod.[SystemWebAdminModuleName])[SystemWebAdminModuleName]
		FROM [dbo].[SystemWebAdminUserRole] swaur
		LEFT JOIN [dbo].[SystemWebAdminMenuRole] swamr ON swaur.SystemWebAdminRoleId = swamr.SystemWebAdminRoleId
		LEFT JOIN [dbo].[SystemWebAdminRole] swar ON swamr.[SystemWebAdminRoleId] = swar.[SystemWebAdminRoleId]
		LEFT JOIN [dbo].[SystemWebAdminMenu] swam ON swamr.[SystemWebAdminMenuId] = swam.[SystemWebAdminMenuId]
		LEFT JOIN [dbo].[SystemWebAdminModule] swamod ON swam.[SystemWebAdminModuleId] = swamod.[SystemWebAdminModuleId]
		WHERE swaur.SystemUserId = @SystemUserId
		AND swaur.EntityStatusId = 1
		AND swamr.[EntityStatusId] = 1
		AND swamr.IsAllowed = 1
		GROUP BY swam.SystemWebAdminMenuId

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_getBySystemWebAdminMenuIdAndSystemWebAdminRoleId]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_getBySystemWebAdminRoleId]    Script Date: 12/7/2021 1:28:34 PM ******/
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
		AND swam.EntityStatusId = 1
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_getBySystemWebAdminRoleIdandSystemWebAdminModuleId]    Script Date: 12/7/2021 1:28:34 PM ******/
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
			AND swam.EntityStatusId = 1
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
			AND swam.EntityStatusId = 1
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminmenuroles_update]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminrole_add]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminrole_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminrole_getByID]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminrole_getPaged]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminrole_update]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminroleprivileges_add]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminroleprivileges_add]
	@SystemWebAdminPrivilegeId	NVARCHAR(100),
	@IsAllowed				BIT,
	@SystemWebAdminRoleId	NVARCHAR(100),
	@CreatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		DECLARE @SystemWebAdminRolePrivilegeId nvarchar(100);
		
		IF EXISTS(SELECT * FROM [dbo].[SystemWebAdminRolePrivileges] WHERE [SystemWebAdminPrivilegeId] = @SystemWebAdminPrivilegeId AND [SystemWebAdminRoleId] = @SystemWebAdminRoleId)
		BEGIN
			SELECT @SystemWebAdminRolePrivilegeId = [SystemWebAdminRolePrivilegeId] FROM [dbo].[SystemWebAdminRolePrivileges] WHERE [SystemWebAdminPrivilegeId] = @SystemWebAdminPrivilegeId AND [SystemWebAdminRoleId] = @SystemWebAdminRoleId;
			UPDATE [dbo].[SystemWebAdminRolePrivileges] SET IsAllowed = 1, EntityStatusId = 1 WHERE [SystemWebAdminRolePrivilegeId] = @SystemWebAdminRolePrivilegeId;
		END
		ELSE
		BEGIN
		
			exec [dbo].[usp_sequence_getNextCode] 'SystemWebAdminRolePrivileges', @Id = @SystemWebAdminRolePrivilegeId OUTPUT

			INSERT INTO [dbo].[SystemWebAdminRolePrivileges](
				[SystemWebAdminRolePrivilegeId],
				[SystemWebAdminPrivilegeId],
				[SystemWebAdminRoleId],
				[IsAllowed],
				[CreatedBy],
				[CreatedAt],
				[EntityStatusId]
			)
			VALUES(
				@SystemWebAdminRolePrivilegeId,
				@SystemWebAdminPrivilegeId,
				@SystemWebAdminRoleId,
				@IsAllowed,
				@CreatedBy,
				GETDATE(),
				1
			);

		END

		SELECT @SystemWebAdminRolePrivilegeId;
        
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminroleprivileges_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminroleprivileges_delete]
	@SystemWebAdminRolePrivilegeId	NVARCHAR(100),
	@LastUpdatedBy				NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;

		IF EXISTS(SELECT * FROM [dbo].[SystemWebAdminRolePrivileges] WHERE [SystemWebAdminRolePrivilegeId] = @SystemWebAdminRolePrivilegeId AND EntityStatusId = 1)
		BEGIN
			-- UPDATE HERE
			UPDATE [dbo].[SystemWebAdminRolePrivileges]
			SET 
			IsAllowed = 0,
			EntityStatusId = 2,
			LastUpdatedBy = @LastUpdatedBy,
			LastUpdatedAt = GETDATE()
			WHERE [SystemWebAdminRolePrivilegeId] = @SystemWebAdminRolePrivilegeId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminroleprivileges_getBySystemUserId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminroleprivileges_getBySystemUserId]
	@SystemUserId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		SELECT 
		swap.[SystemWebAdminPrivilegeId],
		MAX(swap.[SystemWebAdminPrivilegeName])[SystemWebAdminPrivilegeName]
		FROM [dbo].[SystemWebAdminRolePrivileges] swarp
		LEFT JOIN [dbo].[SystemWebAdminUserRole] swaur ON swarp.[SystemWebAdminRoleId] = swaur.[SystemWebAdminRoleId]
		LEFT JOIN [dbo].[SystemWebAdminPrivileges] swap ON swarp.[SystemWebAdminPrivilegeId] = swap.[SystemWebAdminPrivilegeId]
		WHERE swaur.[SystemUserId] = @SystemUserId
		AND swarp.[IsAllowed] = 1
		AND swarp.[EntityStatusId] = 1
		GROUP BY swap.[SystemWebAdminPrivilegeId]

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminroleprivileges_getBySystemWebAdminPrivilegeIdAndSystemWebAdminRoleId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminroleprivileges_getBySystemWebAdminPrivilegeIdAndSystemWebAdminRoleId]
	@SystemWebAdminPrivilegeId	BIGINT,
	@SystemWebAdminRoleId		NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		
		DECLARE @SystemWebAdminRolePrivilegeId	NVARCHAR(100);
		DECLARE @CreatedBy			NVARCHAR(100);
		DECLARE @CreatedAt			DATETIME;
		DECLARE @LastUpdatedBy		NVARCHAR(100);
		DECLARE @LastUpdatedAt		DATETIME;
		DECLARE @EntityStatusId		BIGINT;
		
		SELECT 
		@SystemWebAdminRolePrivilegeId = swarp.[SystemWebAdminRolePrivilegeId],
		@CreatedBy = swarp.[CreatedBy],
		@CreatedAt = swarp.[CreatedAt],
		@LastUpdatedBy = swarp.[LastUpdatedBy],
		@LastUpdatedAt = swarp.[LastUpdatedAt],
		@EntityStatusId = swarp.[EntityStatusId]
		FROM [dbo].[SystemWebAdminRolePrivileges] swarp
		WHERE swarp.[SystemWebAdminRoleId] = @SystemWebAdminRoleId
		AND swarp.[SystemWebAdminPrivilegeId] = @SystemWebAdminPrivilegeId
		AND swarp.[EntityStatusId] = 1;
		
		SELECT *
		FROM [dbo].[SystemWebAdminRolePrivileges] swarp
		WHERE swarp.[SystemWebAdminRolePrivilegeId] = @SystemWebAdminRolePrivilegeId
		AND swarp.[EntityStatusId] = 1;

		SELECT *
		FROM [dbo].[SystemWebAdminRole] AS swar
		WHERE swar.[SystemWebAdminRoleId] = @SystemWebAdminRoleId;
		
		SELECT *
		FROM [dbo].[SystemWebAdminPrivileges] AS swap
		WHERE swap.[SystemWebAdminPrivilegeId] = @SystemWebAdminPrivilegeId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminroleprivileges_getBySystemWebAdminRoleId]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminroleprivileges_getBySystemWebAdminRoleId]
	@SystemWebAdminRoleId	NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		SELECT 
		MAX(systemWebAdminRolePrivileges.[SystemWebAdminRolePrivilegeId])[SystemWebAdminRolePrivilegeId], 
		MAX(systemWebAdminRolePrivileges.[IsAllowed])[IsAllowed], 
		systemWebAdminRolePrivileges.[SystemWebAdminPrivilegeId], 
		MAX(systemWebAdminRolePrivileges.[SystemWebAdminPrivilegeName])[SystemWebAdminPrivilegeName],  
		MAX(systemWebAdminRolePrivileges.[SystemWebAdminRoleId])[SystemWebAdminRoleId], 
		MAX(systemWebAdminRolePrivileges.[RoleName])[RoleName]
		FROM (
			SELECT 
			swarp.[SystemWebAdminRolePrivilegeId],
			swarp.[IsAllowed],
			swap.[SystemWebAdminPrivilegeId],
			swap.[SystemWebAdminPrivilegeName],
			swar.[SystemWebAdminRoleId],
			swar.[RoleName]
			FROM [dbo].[SystemWebAdminRolePrivileges] swarp
			LEFT JOIN [dbo].[SystemWebAdminRole] swar ON swarp.[SystemWebAdminRoleId] = swar.[SystemWebAdminRoleId]
			LEFT JOIN [dbo].[SystemWebAdminPrivileges] swap ON swarp.[SystemWebAdminPrivilegeId] = swap.[SystemWebAdminPrivilegeId]
			WHERE swarp.[SystemWebAdminRoleId] = @SystemWebAdminRoleId
			AND swarp.[EntityStatusId] = 1

			UNION ALL

			SELECT 
			CAST(swap.[SystemWebAdminPrivilegeId] AS NVARCHAR(100)) AS [SystemWebAdminRolePrivilegeId],
			0 [IsAllowed],
			swap.[SystemWebAdminPrivilegeId],
			swap.[SystemWebAdminPrivilegeName],
			'' [SystemWebAdminRoleId],
			'' [RoleName]
			FROM [dbo].[SystemWebAdminPrivileges] swap
		) systemWebAdminRolePrivileges
		GROUP BY systemWebAdminRolePrivileges.SystemWebAdminPrivilegeId
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminroleprivileges_update]    Script Date: 12/7/2021 1:28:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Created date: Sept 25, 2020
-- Author: 
-- ====================================================================
CREATE PROCEDURE [dbo].[usp_systemwebadminroleprivileges_update]
	@SystemWebAdminRolePrivilegeId		NVARCHAR(100),
	@IsAllowed						BIT,
	@LastUpdatedBy					NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

		SET NOCOUNT ON;
		-- UPDATE HERE
		UPDATE [dbo].[SystemWebAdminRolePrivileges]
		SET 
		[IsAllowed] = @IsAllowed,
		[LastUpdatedBy] = @LastUpdatedBy,
		[LastUpdatedAt] = GETDATE()
		WHERE [SystemWebAdminRolePrivilegeId] = @SystemWebAdminRolePrivilegeId;

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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_add]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_delete]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_getBySystemUserId]    Script Date: 12/7/2021 1:28:34 PM ******/
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
		LEFT JOIN [dbo].[SystemUser] su ON swaur.[SystemUserId] = su.[SystemUserId]
		LEFT JOIN [dbo].[SystemWebAdminRole] swar ON swaur.[SystemWebAdminRoleId] = swar.[SystemWebAdminRoleId]
		WHERE su.[SystemUserId] = @SystemUserId
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_getBySystemWebAdminRoleIdAndSystemUserId]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_getBySystemWebAdminUserRoleId]    Script Date: 12/7/2021 1:28:34 PM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_systemwebadminuserroles_getPaged]    Script Date: 12/7/2021 1:28:34 PM ******/
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
