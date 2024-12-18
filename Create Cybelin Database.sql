﻿USE [Cybelin]
GO
/****** Object:  Table [dbo].[AlertEndpoints]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertEndpoints](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Path] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Endpoints] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertInstances]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertInstances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AlertRuleId] [int] NULL,
	[AlertRuleName] [nvarchar](50) NOT NULL,
	[AlertTypeId] [int] NOT NULL,
	[AlertTypeName] [nvarchar](max) NOT NULL,
	[AlertValue] [int] NOT NULL,
	[Severity] [nvarchar](50) NULL,
	[ServerId] [int] NOT NULL,
	[ServerName] [nvarchar](100) NOT NULL,
	[ClientIP] [nvarchar](50) NULL,
	[EndpointId] [int] NULL,
	[EndpointPath] [nvarchar](max) NULL,
	[TriggeredAtUTC] [datetime2](7) NOT NULL,
	[ResolvedAtUTC] [datetime2](7) NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__AlertIns__3214EC07728CC0E0] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertNotifications]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertNotifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AlertInstanceId] [int] NULL,
	[NotificationType] [nvarchar](max) NOT NULL,
	[Recipient] [nvarchar](50) NOT NULL,
	[SentAtUTC] [datetime2](7) NOT NULL,
	[AlertRuleId] [int] NOT NULL,
	[AlertRuleName] [nvarchar](50) NOT NULL,
	[AlertTypeId] [int] NOT NULL,
	[AlertTypeName] [nvarchar](max) NOT NULL,
	[AlertValue] [int] NOT NULL,
	[Severity] [nvarchar](50) NULL,
	[ServerId] [int] NOT NULL,
	[ServerName] [nvarchar](100) NOT NULL,
	[ClientIP] [nvarchar](50) NULL,
	[EndpointId] [int] NULL,
	[EndpointPath] [nvarchar](max) NULL,
	[TriggeredAtUTC] [datetime2](7) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__AlertNot__3214EC072B870E70] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertRecipients]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertRecipients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Recipient] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Phone] [nvarchar](50) NULL,
 CONSTRAINT [PK__AlertRec__3214EC076062357D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertRuleRecipients]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertRuleRecipients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AlertRuleId] [int] NOT NULL,
	[RecipientId] [int] NOT NULL,
 CONSTRAINT [PK__AlertRul__3214EC07F2DCFB00] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertRules]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertRules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AlertTypeId] [int] NULL,
	[AlertTypeName] [nvarchar](100) NULL,
	[AlertValue] [int] NULL,
	[Expression] [nvarchar](max) NULL,
	[Severity] [nvarchar](50) NOT NULL,
	[CreatedAtUTC] [datetime2](7) NULL,
	[UpdatedAtUTC] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[ActiveInAllServers] [bit] NULL,
	[ActiveInAllEndpoints] [bit] NULL,
 CONSTRAINT [PK__AlertRul__3214EC07A9F92D69] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertRulesEndpoints]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertRulesEndpoints](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AlertRuleId] [int] NOT NULL,
	[AlertEndpointId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertRulesServers]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertRulesServers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AlertRuleId] [int] NOT NULL,
	[ServerId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertSilenced]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertSilenced](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AlertRuleId] [int] NOT NULL,
	[SilencedFromUTC] [datetime2](7) NOT NULL,
	[SilencedUntilUTC] [datetime2](7) NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK__Silenced__3214EC07AED15E94] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertTypes]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AlertTypeName] [nvarchar](max) NOT NULL,
	[Script] [bit] NULL,
	[Expression] [nvarchar](max) NULL,
 CONSTRAINT [PK_AlertTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MonitorBlacklistedIps]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonitorBlacklistedIps](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IpAddress] [nvarchar](max) NULL,
	[DateAdded] [datetime2](7) NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_MonitorBlacklistedIps] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MonitorConfigurations]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonitorConfigurations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_MonitorConfigurations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servers]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](100) NOT NULL,
	[ConnectionString] [nvarchar](max) NOT NULL,
	[ServerType] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WhitelistedIps]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WhitelistedIps](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IpAddress] [nvarchar](max) NULL,
	[DateAdded] [datetime2](7) NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_WhitelistedIps] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AlertRules] ADD  CONSTRAINT [DF__AlertRule__Creat__5EBF139D]  DEFAULT (getdate()) FOR [CreatedAtUTC]
GO
ALTER TABLE [dbo].[AlertRules] ADD  CONSTRAINT [DF__AlertRule__Updat__5FB337D6]  DEFAULT (getdate()) FOR [UpdatedAtUTC]
GO
ALTER TABLE [dbo].[AlertRules] ADD  CONSTRAINT [DF__AlertRule__IsAct__60A75C0F]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AlertInstances]  WITH CHECK ADD  CONSTRAINT [FK__AlertInst__Alert__6477ECF3] FOREIGN KEY([AlertRuleId])
REFERENCES [dbo].[AlertRules] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[AlertInstances] CHECK CONSTRAINT [FK__AlertInst__Alert__6477ECF3]
GO
ALTER TABLE [dbo].[AlertNotifications]  WITH CHECK ADD  CONSTRAINT [FK__AlertNoti__Alert__68487DD7] FOREIGN KEY([AlertInstanceId])
REFERENCES [dbo].[AlertInstances] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[AlertNotifications] CHECK CONSTRAINT [FK__AlertNoti__Alert__68487DD7]
GO
ALTER TABLE [dbo].[AlertRuleRecipients]  WITH CHECK ADD  CONSTRAINT [FK_AlertRuleRecipients_AlertRecipients] FOREIGN KEY([RecipientId])
REFERENCES [dbo].[AlertRecipients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertRuleRecipients] CHECK CONSTRAINT [FK_AlertRuleRecipients_AlertRecipients]
GO
ALTER TABLE [dbo].[AlertRuleRecipients]  WITH CHECK ADD  CONSTRAINT [FK_AlertRuleRecipients_AlertRules] FOREIGN KEY([AlertRuleId])
REFERENCES [dbo].[AlertRules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertRuleRecipients] CHECK CONSTRAINT [FK_AlertRuleRecipients_AlertRules]
GO
ALTER TABLE [dbo].[AlertRules]  WITH CHECK ADD  CONSTRAINT [FK_AlertRules_AlertTypes] FOREIGN KEY([AlertTypeId])
REFERENCES [dbo].[AlertTypes] ([Id])
GO
ALTER TABLE [dbo].[AlertRules] CHECK CONSTRAINT [FK_AlertRules_AlertTypes]
GO
ALTER TABLE [dbo].[AlertRulesEndpoints]  WITH CHECK ADD  CONSTRAINT [FK_AlertRulesEndpoints_AlertEndpoints] FOREIGN KEY([AlertEndpointId])
REFERENCES [dbo].[AlertEndpoints] ([Id])
GO
ALTER TABLE [dbo].[AlertRulesEndpoints] CHECK CONSTRAINT [FK_AlertRulesEndpoints_AlertEndpoints]
GO
ALTER TABLE [dbo].[AlertRulesEndpoints]  WITH CHECK ADD  CONSTRAINT [FK_AlertRulesEndpoints_AlertRules] FOREIGN KEY([AlertRuleId])
REFERENCES [dbo].[AlertRules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertRulesEndpoints] CHECK CONSTRAINT [FK_AlertRulesEndpoints_AlertRules]
GO
ALTER TABLE [dbo].[AlertRulesServers]  WITH CHECK ADD  CONSTRAINT [FK_AlertRulesServers_AlertRules] FOREIGN KEY([AlertRuleId])
REFERENCES [dbo].[AlertRules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertRulesServers] CHECK CONSTRAINT [FK_AlertRulesServers_AlertRules]
GO
ALTER TABLE [dbo].[AlertRulesServers]  WITH CHECK ADD  CONSTRAINT [FK_AlertRulesServers_Servers] FOREIGN KEY([ServerId])
REFERENCES [dbo].[Servers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertRulesServers] CHECK CONSTRAINT [FK_AlertRulesServers_Servers]
GO
ALTER TABLE [dbo].[AlertSilenced]  WITH CHECK ADD  CONSTRAINT [FK__SilencedA__Alert__6B24EA82] FOREIGN KEY([AlertRuleId])
REFERENCES [dbo].[AlertRules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlertSilenced] CHECK CONSTRAINT [FK__SilencedA__Alert__6B24EA82]
GO
/****** Object:  StoredProcedure [dbo].[GetAlertRuleEndpointsByServerAndRule]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAlertRuleEndpointsByServerAndRule]
    @ServerId INT,
    @AlertRuleId INT
AS
BEGIN
    SELECT AE.*
    FROM AlertEndpoints AE
    INNER JOIN AlertRulesEndpoints ARE ON AE.Id = ARE.AlertEndpointId
    INNER JOIN AlertRules AR ON ARE.AlertRuleId = AR.Id
    LEFT JOIN AlertRulesServers ARS ON AR.Id = ARS.AlertRuleId
    WHERE (AR.ActiveInAllServers = 1 OR ARS.ServerId = @ServerId)
    AND AR.IsActive = 1
    AND AR.Id = @AlertRuleId;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetAlertRuleRecipientsWithDetails]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAlertRuleRecipientsWithDetails]
AS
BEGIN
    SELECT 
        arr.Id AS AlertRuleRecipientId,
        ar.Name AS AlertRuleName,
		arr.AlertRuleId,
        arrc.Recipient AS RecipientName,
        arr.RecipientId
    FROM 
        AlertRuleRecipients arr
    JOIN 
        AlertRules ar ON arr.AlertRuleId = ar.Id
    JOIN 
        AlertRecipients arrc ON arr.RecipientId = arrc.Id;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetAlertRulesByServerId]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAlertRulesByServerId]
    @ServerId INT
AS
BEGIN
    SELECT AR.*
    FROM AlertRules AR
    LEFT JOIN AlertRulesServers ARS ON AR.Id = ARS.AlertRuleId
    WHERE (AR.ActiveInAllServers = 1
       OR (ARS.ServerId = @ServerId)) AND (AR.IsActive=1)
END
GO
/****** Object:  StoredProcedure [dbo].[GetAlertRulesServersWithDetails]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAlertRulesServersWithDetails]
AS
BEGIN
    SELECT 
        ars.Id,
        ars.AlertRuleId,
        ars.ServerId,
        ar.Name AS AlertRuleName,
        s.ServerName
    FROM 
        dbo.AlertRulesServers ars
    INNER JOIN 
        dbo.AlertRules ar ON ars.AlertRuleId = ar.Id
    INNER JOIN 
        dbo.Servers s ON ars.ServerId = s.Id;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllAlertRuleEndpointsWithDetails]    Script Date: 11/6/2024 8:06:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetAllAlertRuleEndpointsWithDetails]
AS
BEGIN
    SELECT 
        ARE.Id AS AlertRuleEndpointId,  -- Id from AlertRuleEndpoints
        ARE.AlertRuleId,                -- Foreign key reference to AlertRules
        ARE.AlertEndpointId,            -- Foreign key reference to AlertEndpoints
        AR.Name AS AlertRuleName,       -- Name from AlertRules
        AE.Path AS AlertEndpointPath    -- Path from AlertEndpoints
    FROM 
        AlertRulesEndpoints ARE
    LEFT JOIN 
        AlertRules AR ON ARE.AlertRuleId = AR.Id
    LEFT JOIN 
        AlertEndpoints AE ON ARE.AlertEndpointId = AE.Id
	ORDER BY ARE.Id;
END
GO

SET IDENTITY_INSERT [dbo].[AlertTypes] ON 

INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (1, N'Outbound traffic per client IP in bytes per minute', 0, N'false')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (2, N'Number of requests per client IP per minute', 0, N'false')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (3, N'Number of 4XX responses per client IP per minute', 0, N'False')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (4, N'Number of requests without response per client IP per minute', 0, N'False')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (5, N'Outbound traffic per endpoint in bytes per minute', 0, N'False')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (6, N'Number of requests per endpoint per minute', 0, N'false')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (7, N'Number of 4XX responses per endpoint per minute', 0, N'false')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (8, N'Number of requests without response per endpoint per minute', 0, N'false')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (9, N'Outbound traffic per server in bytes per minute', 0, N'false')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (10, N'Number of requests per server per minute', 0, N'false')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (11, N'Number of 4XX responses per server per minute', 0, N'false')
INSERT [dbo].[AlertTypes] ([Id], [AlertTypeName], [Script], [Expression]) VALUES (12, N'Number of requests without response per server per minute', 0, N'false')
SET IDENTITY_INSERT [dbo].[AlertTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[MonitorConfigurations] ON 

INSERT [dbo].[MonitorConfigurations] ([Id], [Key], [Value], [LastUpdated]) VALUES (3, N'Email SmtpClient', N'smtp.office365.com', CAST(N'2024-10-22T07:04:47.0000000' AS DateTime2))
INSERT [dbo].[MonitorConfigurations] ([Id], [Key], [Value], [LastUpdated]) VALUES (4, N'Email Port', N'587', CAST(N'2024-10-22T07:05:38.0000000' AS DateTime2))
INSERT [dbo].[MonitorConfigurations] ([Id], [Key], [Value], [LastUpdated]) VALUES (5, N'Email UseDefaultCredentials', N'false', CAST(N'2024-10-22T07:06:22.0000000' AS DateTime2))
INSERT [dbo].[MonitorConfigurations] ([Id], [Key], [Value], [LastUpdated]) VALUES (6, N'Email Credential Name', N'Your email', CAST(N'2024-11-03T06:57:45.0000000' AS DateTime2))
INSERT [dbo].[MonitorConfigurations] ([Id], [Key], [Value], [LastUpdated]) VALUES (7, N'Email Credential Password', N'Your password', CAST(N'2024-11-03T06:58:08.0000000' AS DateTime2))
INSERT [dbo].[MonitorConfigurations] ([Id], [Key], [Value], [LastUpdated]) VALUES (8, N'Email EnableSsl', N'true', CAST(N'2024-10-22T07:08:16.0000000' AS DateTime2))
INSERT [dbo].[MonitorConfigurations] ([Id], [Key], [Value], [LastUpdated]) VALUES (9, N'Email From', N'Your email', CAST(N'2024-11-03T06:58:26.0000000' AS DateTime2))
INSERT [dbo].[MonitorConfigurations] ([Id], [Key], [Value], [LastUpdated]) VALUES (10, N'Email IsBodyHtml', N'false', CAST(N'2024-10-22T07:09:06.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[MonitorConfigurations] OFF
GO
