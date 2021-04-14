/*
 Navicat Premium Data Transfer

 Source Server         : LocalSqlServer
 Source Server Type    : SQL Server
 Source Server Version : 11002100
 Source Host           : localhost:1433
 Source Catalog        : ApiTestDb
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 11002100
 File Encoding         : 65001

 Date: 14/04/2021 14:49:49
*/


-- ----------------------------
-- Table structure for sys_menu
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_menu]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_menu]
GO

CREATE TABLE [dbo].[sys_menu] (
  [ID] int  NOT NULL,
  [PID] int  NULL,
  [Code] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Name] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Type] int  NULL,
  [Icon] varchar(128) COLLATE Chinese_PRC_CI_AS  NULL,
  [Permission] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Link] varchar(128) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] smallint  NULL,
  [IsDelete] smallint  NULL
)
GO

ALTER TABLE [dbo].[sys_menu] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_menu
-- ----------------------------
INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'1', N'0', N'homeQuery', N'用户管理', N'1', NULL, N'homeQuery', N'', NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'2', N'1', N'userAdd', N'用户添加', N'2', NULL, N'userAdd', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'3', N'1', N'userUpdate', N'用户修改', N'2', NULL, N'userUpdate', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'4', N'1', N'userDelete', N'用户删除', N'2', NULL, N'userDelete', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'5', N'0', N'menuQuery', N'菜单管理', N'1', NULL, N'menuQuery', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'6', N'6', N'menuAdd', N'菜单添加', N'2', NULL, N'menuAdd', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'7', N'6', N'menuUpdate', N'菜单修改', N'2', NULL, N'menuUpdate', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'8', N'6', N'menuDelete', N'菜单删除', N'2', NULL, N'menuDelete', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'9', N'0', N'roleQuery', N'角色管理', N'1', NULL, N'roleQuery', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'10', N'10', N'roleAdd', N'角色添加', N'2', NULL, N'roleAdd', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'11', N'10', N'roleUpdate', N'角色修改', N'2', NULL, N'roleUpdate', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[sys_menu] ([ID], [PID], [Code], [Name], [Type], [Icon], [Permission], [Link], [Status], [IsDelete]) VALUES (N'12', N'10', N'roleDelete', N'角色删除', N'2', NULL, N'roleDelete', NULL, NULL, NULL)
GO


-- ----------------------------
-- Table structure for sys_role
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_role]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_role]
GO

CREATE TABLE [dbo].[sys_role] (
  [ID] int  NOT NULL,
  [Name] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Code] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Sort] int  NULL,
  [Remark] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] smallint  NULL,
  [IsDeleted] smallint  NULL
)
GO

ALTER TABLE [dbo].[sys_role] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N' ',
'SCHEMA', N'dbo',
'TABLE', N'sys_role',
'COLUMN', N'ID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'sys_role',
'COLUMN', N'Remark'
GO


-- ----------------------------
-- Records of sys_role
-- ----------------------------
INSERT INTO [dbo].[sys_role] ([ID], [Name], [Code], [Sort], [Remark], [Status], [IsDeleted]) VALUES (N'1', N'系统管理员', N'1', N'0', N'无', N'0', N'0')
GO


-- ----------------------------
-- Table structure for sys_role_menu
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_role_menu]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_role_menu]
GO

CREATE TABLE [dbo].[sys_role_menu] (
  [SysRoleId] int  NULL,
  [SysMenuId] int  NULL
)
GO

ALTER TABLE [dbo].[sys_role_menu] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'角色ID',
'SCHEMA', N'dbo',
'TABLE', N'sys_role_menu',
'COLUMN', N'SysRoleId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'菜单ID',
'SCHEMA', N'dbo',
'TABLE', N'sys_role_menu',
'COLUMN', N'SysMenuId'
GO


-- ----------------------------
-- Records of sys_role_menu
-- ----------------------------
INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'1')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'2')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'3')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'4')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'5')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'6')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'7')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'8')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'9')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'10')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'11')
GO

INSERT INTO [dbo].[sys_role_menu] ([SysRoleId], [SysMenuId]) VALUES (N'1', N'12')
GO


-- ----------------------------
-- Table structure for sys_user
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_user]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_user]
GO

CREATE TABLE [dbo].[sys_user] (
  [ID] int  NOT NULL,
  [Account] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Password] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [NickName] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Name] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Avatar] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Birthday] date  NULL,
  [Sex] smallint  NULL,
  [Email] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Phone] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] smallint  NULL,
  [IsDeleted] smallint  NULL
)
GO

ALTER TABLE [dbo].[sys_user] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_user
-- ----------------------------
INSERT INTO [dbo].[sys_user] ([ID], [Account], [Password], [NickName], [Name], [Avatar], [Birthday], [Sex], [Email], [Phone], [Status], [IsDeleted]) VALUES (N'1', N'string', N'string', N'啊啊啊啊', N'刘俊', N'', N'2021-04-08', N'1', NULL, N'17713531754', N'0', N'0')
GO


-- ----------------------------
-- Table structure for sys_user_role
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_user_role]') AND type IN ('U'))
	DROP TABLE [dbo].[sys_user_role]
GO

CREATE TABLE [dbo].[sys_user_role] (
  [SysUserId] int  NOT NULL,
  [SysRoleId] int  NOT NULL
)
GO

ALTER TABLE [dbo].[sys_user_role] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sys_user_role
-- ----------------------------
INSERT INTO [dbo].[sys_user_role] ([SysUserId], [SysRoleId]) VALUES (N'1', N'1')
GO


-- ----------------------------
-- Primary Key structure for table sys_menu
-- ----------------------------
ALTER TABLE [dbo].[sys_menu] ADD CONSTRAINT [PK__sys_menu__3214EC27CB5C6EF5] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table sys_role
-- ----------------------------
ALTER TABLE [dbo].[sys_role] ADD CONSTRAINT [PK__sys_role__3214EC274EF33E23] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_role_menu
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [index_role_menu]
ON [dbo].[sys_role_menu] (
  [SysRoleId] ASC,
  [SysMenuId] ASC
)
GO


-- ----------------------------
-- Indexes structure for table sys_user
-- ----------------------------
CREATE NONCLUSTERED INDEX [index_u_a_p]
ON [dbo].[sys_user] (
  [Account] ASC,
  [Password] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'密码账号加速查询',
'SCHEMA', N'dbo',
'TABLE', N'sys_user',
'INDEX', N'index_u_a_p'
GO

CREATE UNIQUE NONCLUSTERED INDEX [index_u_a]
ON [dbo].[sys_user] (
  [Account] ASC
)
GO

EXEC sp_addextendedproperty
'MS_Description', N'账号唯一索引',
'SCHEMA', N'dbo',
'TABLE', N'sys_user',
'INDEX', N'index_u_a'
GO


-- ----------------------------
-- Primary Key structure for table sys_user
-- ----------------------------
ALTER TABLE [dbo].[sys_user] ADD CONSTRAINT [PK__sys_user__3214EC2704CF37FD] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table sys_user_role
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [index_user_role]
ON [dbo].[sys_user_role] (
  [SysUserId] ASC,
  [SysRoleId] ASC
)
GO


-- ----------------------------
-- Foreign Keys structure for table sys_role_menu
-- ----------------------------
ALTER TABLE [dbo].[sys_role_menu] ADD CONSTRAINT [fk_rm_role] FOREIGN KEY ([SysRoleId]) REFERENCES [dbo].[sys_role] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[sys_role_menu] ADD CONSTRAINT [fk_rm_menu] FOREIGN KEY ([SysMenuId]) REFERENCES [dbo].[sys_menu] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table sys_user_role
-- ----------------------------
ALTER TABLE [dbo].[sys_user_role] ADD CONSTRAINT [fk_ur_user] FOREIGN KEY ([SysUserId]) REFERENCES [dbo].[sys_user] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[sys_user_role] ADD CONSTRAINT [fk_ur_role] FOREIGN KEY ([SysRoleId]) REFERENCES [dbo].[sys_role] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

