/*
 Navicat Premium Data Transfer

 Source Server         : LocalSqlServer
 Source Server Type    : SQL Server
 Source Server Version : 11002100
 Source Host           : localhost:1433
 Source Catalog        : BSMG
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 11002100
 File Encoding         : 65001

 Date: 08/06/2021 08:39:42
*/


-- ----------------------------
-- Table structure for admin_menu
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[admin_menu]') AND type IN ('U'))
	DROP TABLE [dbo].[admin_menu]
GO

CREATE TABLE [dbo].[admin_menu] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [Name] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [PID] int  NULL,
  [Code] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Type] int  NULL,
  [Icon] varchar(128) COLLATE Chinese_PRC_CI_AS  NULL,
  [Permission] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Link] varchar(128) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[admin_menu] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'1级菜单2二级菜单3三级菜单4页面按钮',
'SCHEMA', N'dbo',
'TABLE', N'admin_menu',
'COLUMN', N'Type'
GO


-- ----------------------------
-- Records of admin_menu
-- ----------------------------
SET IDENTITY_INSERT [dbo].[admin_menu] ON
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'1', N'系统管理', N'0', N'AdminPermission', N'1', NULL, N'AdminPermission', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'2', N'用户管理', N'1', N'AdminUser', N'2', NULL, N'AdminUser', N'/pages/sys_admin/user_list.html')
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'3', N'用户添加', N'2', N'AdminAddUser', N'4', NULL, N'AdminAddUser', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'4', N'用户修改', N'2', N'AdminUpdateUser', N'4', NULL, N'AdminUpdateUser', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'5', N'用户删除', N'2', N'AdminDeleteUser', N'4', NULL, N'AdminDeleteUser', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'7', N'权限编辑', N'1', N'AdminPermissionEdit', N'2', NULL, N'AdminPermissionEdit', N'/pages/sys_admin/permission_edit.html')
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'1002', N'角色权限修改', N'7', N'AdminSaveRoleMenuPermissionEdit', N'4', NULL, N'AdminSaveRoleMenuPermissionEdit', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'1003', N'用户角色修改', N'7', N'AdminSaveUserRolePermissionEdit', N'4', NULL, N'AdminSaveUserRolePermissionEdit', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'1004', N'查看权限树', N'7', N'AdminLoadPermissionTreePermissionEdit', N'4', NULL, N'AdminLoadPermissionTreePermissionEdit', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'1005', N'查看角色选择框', N'7', N'AdminLoadRoleCheckBoxPermissionEdit', N'4', NULL, N'AdminLoadRoleCheckBoxPermissionEdit', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'1006', N'用户查看', N'2', N'AdminUserShow', N'4', NULL, N'AdminUserShow', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'2002', N'角色管理', N'1', N'AdminRole', N'2', NULL, N'AdminRole', N'/pages/sys_admin/role_list.html')
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'2003', N'角色查看', N'2002', N'AdminRoleShow', N'4', NULL, N'AdminRoleShow', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'2004', N'角色添加', N'2002', N'AdminAddRole', N'4', NULL, N'AdminAddRole', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'2005', N'角色修改', N'2002', N'AdminUpdateRole', N'4', NULL, N'AdminUpdateRole', NULL)
GO

INSERT INTO [dbo].[admin_menu] ([ID], [Name], [PID], [Code], [Type], [Icon], [Permission], [Link]) VALUES (N'2006', N'角色删除', N'2002', N'AdminDeleteRole', N'4', NULL, N'AdminDeleteRole', NULL)
GO

SET IDENTITY_INSERT [dbo].[admin_menu] OFF
GO


-- ----------------------------
-- Table structure for admin_role
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[admin_role]') AND type IN ('U'))
	DROP TABLE [dbo].[admin_role]
GO

CREATE TABLE [dbo].[admin_role] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [Name] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Code] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Remark] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[admin_role] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'admin_role',
'COLUMN', N'Remark'
GO


-- ----------------------------
-- Records of admin_role
-- ----------------------------
SET IDENTITY_INSERT [dbo].[admin_role] ON
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'1', N'系统管理员', N'Admin', N'系统管理员')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'2', N'测试员', N'Test', N'测试员')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'3', N'业务员', N'Ywy', N'业务员')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'4', N'运维人员', N'Yw', N'运维人员')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'5', N'测试1', N'Test1', N'测试1')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'6', N'Test2', N'Test2', N'Tes2')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'7', N'Tes3', N'Test3', N'Test3')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'8', N'Test4', N'Test4', N'Test4')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'9', N'Test5', N'Test5', N'Test5')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'10', N'Test6', N'Test6', N'Test6')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'11', N'Test7', N'Test7', N'Test7')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'12', N'Test8', N'Test8', N'Test8')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'13', N'Test9', N'Test9', N'Test9')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'14', N'Test10', N'Test10', N'Test10')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'15', N'Test11', N'Test11', N'Test11')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'16', N'Test121', N'Test121', N'Test121')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'17', N'Test122', N'Test122', N'Test122')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'18', N'Test123', N'Test123', N'Test123')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'19', N'Test124', N'Test124', N'Test124')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'20', N'Test112', N'Test112', N'Test112')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'21', N'Test1211', N'Test1211', N'Test1211')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'22', N'Test1212', N'Test1212', N'Test1212')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'23', N'Test1213', N'Test1213', N'Test1213')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'24', N'Test1214', N'Test1214', N'Test1214')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'25', N'Test1215', N'Test1215', N'Test1215')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'26', N'Test1216', N'Test1216', N'Test1216')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'27', N'Test1217', N'Test1217', N'Test1217')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'28', N'Test1218', N'Test1218', N'Test1218')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'29', N'Test1219', N'Test1219', N'Test1219')
GO

INSERT INTO [dbo].[admin_role] ([ID], [Name], [Code], [Remark]) VALUES (N'30', N'系统管理员测试长度啊啊啊啊', N'Test1220111111111111111111', N'Test1220111111111111111111')
GO

SET IDENTITY_INSERT [dbo].[admin_role] OFF
GO


-- ----------------------------
-- Table structure for admin_role_menu
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[admin_role_menu]') AND type IN ('U'))
	DROP TABLE [dbo].[admin_role_menu]
GO

CREATE TABLE [dbo].[admin_role_menu] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [RoleId] int  NULL,
  [MenuId] int  NULL
)
GO

ALTER TABLE [dbo].[admin_role_menu] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'角色ID',
'SCHEMA', N'dbo',
'TABLE', N'admin_role_menu',
'COLUMN', N'RoleId'
GO

EXEC sp_addextendedproperty
'MS_Description', N'菜单ID',
'SCHEMA', N'dbo',
'TABLE', N'admin_role_menu',
'COLUMN', N'MenuId'
GO


-- ----------------------------
-- Records of admin_role_menu
-- ----------------------------
SET IDENTITY_INSERT [dbo].[admin_role_menu] ON
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3007', N'2', N'1')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3008', N'2', N'2')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3009', N'2', N'3')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3010', N'2', N'4')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3011', N'2', N'5')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3012', N'2', N'1006')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3013', N'2', N'7')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3014', N'2', N'1002')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3015', N'2', N'1003')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3016', N'2', N'1004')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3017', N'2', N'1005')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3018', N'3', N'1')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3019', N'3', N'2')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3020', N'3', N'3')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3021', N'3', N'4')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3022', N'3', N'5')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3023', N'3', N'1006')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3024', N'1', N'1')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3025', N'1', N'2')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3026', N'1', N'3')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3027', N'1', N'4')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3028', N'1', N'5')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3029', N'1', N'1006')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3030', N'1', N'7')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3031', N'1', N'1002')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3032', N'1', N'1003')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3033', N'1', N'1004')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3034', N'1', N'1005')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3035', N'1', N'2002')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3036', N'1', N'2003')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3037', N'1', N'2004')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3038', N'1', N'2005')
GO

INSERT INTO [dbo].[admin_role_menu] ([ID], [RoleId], [MenuId]) VALUES (N'3039', N'1', N'2006')
GO

SET IDENTITY_INSERT [dbo].[admin_role_menu] OFF
GO


-- ----------------------------
-- Table structure for admin_user
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[admin_user]') AND type IN ('U'))
	DROP TABLE [dbo].[admin_user]
GO

CREATE TABLE [dbo].[admin_user] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [Name] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Account] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [Password] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL,
  [NickName] varchar(64) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[admin_user] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of admin_user
-- ----------------------------
SET IDENTITY_INSERT [dbo].[admin_user] ON
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'1', N'string', N'string', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'2', N'string1', N'string1', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'3', N'string2', N'string2', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'4', N'string3', N'string3', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'5', N'string4', N'string4', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'6', N'string5', N'string5', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'7', N'string6', N'string6', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'8', N'string7', N'string7', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'9', N'string8', N'string8', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'10', N'string9', N'string9', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'11', N'string10', N'string10', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'12', N'string11', N'string11', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'13', N'string12', N'string12', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'14', N'string13', N'string13', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'15', N'string14', N'string14', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'16', N'string15', N'string15', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'17', N'string16', N'string16', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'18', N'string17', N'string17', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'19', N'string18', N'string18', N'string', N'string')
GO

INSERT INTO [dbo].[admin_user] ([ID], [Name], [Account], [Password], [NickName]) VALUES (N'20', N'string19', N'string19', N'string', N'string')
GO

SET IDENTITY_INSERT [dbo].[admin_user] OFF
GO


-- ----------------------------
-- Table structure for admin_user_role
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[admin_user_role]') AND type IN ('U'))
	DROP TABLE [dbo].[admin_user_role]
GO

CREATE TABLE [dbo].[admin_user_role] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [RoleId] int  NOT NULL
)
GO

ALTER TABLE [dbo].[admin_user_role] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of admin_user_role
-- ----------------------------
SET IDENTITY_INSERT [dbo].[admin_user_role] ON
GO

INSERT INTO [dbo].[admin_user_role] ([ID], [UserId], [RoleId]) VALUES (N'2002', N'2', N'2')
GO

INSERT INTO [dbo].[admin_user_role] ([ID], [UserId], [RoleId]) VALUES (N'3005', N'1', N'1')
GO

INSERT INTO [dbo].[admin_user_role] ([ID], [UserId], [RoleId]) VALUES (N'3006', N'3', N'3')
GO

SET IDENTITY_INSERT [dbo].[admin_user_role] OFF
GO


-- ----------------------------
-- Auto increment value for admin_menu
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[admin_menu]', RESEED, 3001)
GO


-- ----------------------------
-- Primary Key structure for table admin_menu
-- ----------------------------
ALTER TABLE [dbo].[admin_menu] ADD CONSTRAINT [PK__admin_me__3214EC27D08FB400] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for admin_role
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[admin_role]', RESEED, 2012)
GO


-- ----------------------------
-- Primary Key structure for table admin_role
-- ----------------------------
ALTER TABLE [dbo].[admin_role] ADD CONSTRAINT [PK__admin_ro__3214EC2710D0E605] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for admin_role_menu
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[admin_role_menu]', RESEED, 4006)
GO


-- ----------------------------
-- Primary Key structure for table admin_role_menu
-- ----------------------------
ALTER TABLE [dbo].[admin_role_menu] ADD CONSTRAINT [PK__admin_ro__3214EC279A5C656C] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for admin_user
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[admin_user]', RESEED, 1001)
GO


-- ----------------------------
-- Primary Key structure for table admin_user
-- ----------------------------
ALTER TABLE [dbo].[admin_user] ADD CONSTRAINT [PK__admin_us__3214EC27A2B9BBF7] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for admin_user_role
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[admin_user_role]', RESEED, 4001)
GO


-- ----------------------------
-- Primary Key structure for table admin_user_role
-- ----------------------------
ALTER TABLE [dbo].[admin_user_role] ADD CONSTRAINT [PK__admin_us__3214EC27C2B44827] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

