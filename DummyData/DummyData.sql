--
-- All user's password: 123
-- 
--///////////////////////
USE [CompanyManagement]
GO
SET IDENTITY_INSERT [dbo].[Companies] ON 
GO
INSERT [dbo].[Companies] ([Id], [Name], [Address], [CreatedDate], [Code]) VALUES (1, N'VTC Game', N'Tòa nhà Lotte Center Hanoi, 54 Liễu Giai, Phường Cống Vị, Quận Ba Đình, Thành phố Hà Nội', CAST(N'2024-05-22T17:01:38.1460287' AS DateTime2), N'1JtyeHC3CzkqqXkHd3jmMFQ')
GO
INSERT [dbo].[Companies] ([Id], [Name], [Address], [CreatedDate], [Code]) VALUES (2, N'VNC Group', N'Ba Đình, Hà Nội', CAST(N'2024-05-22T20:31:25.4067331' AS DateTime2), N'2Jj2gPfzGukyqAQXzEFcsBQ')
GO
SET IDENTITY_INSERT [dbo].[Companies] OFF
GO
SET IDENTITY_INSERT [dbo].[Departments] ON 
GO
INSERT [dbo].[Departments] ([Id], [Name], [CompanyId]) VALUES (1, N'Phòng Nhân sự', 1)
GO
INSERT [dbo].[Departments] ([Id], [Name], [CompanyId]) VALUES (2, N'Phòng Kế toán', 1)
GO
INSERT [dbo].[Departments] ([Id], [Name], [CompanyId]) VALUES (4, N'Phòng Kinh doanh và Tiếp thị', 1)
GO
INSERT [dbo].[Departments] ([Id], [Name], [CompanyId]) VALUES (5, N'Phòng Công nghệ thông tin', 1)
GO
INSERT [dbo].[Departments] ([Id], [Name], [CompanyId]) VALUES (6, N'Phòng Chăm sóc khách hàng', 1)
GO
INSERT [dbo].[Departments] ([Id], [Name], [CompanyId]) VALUES (8, N'Ban điều hành', 1)
GO
SET IDENTITY_INSERT [dbo].[Departments] OFF
GO
SET IDENTITY_INSERT [dbo].[PostCategories] ON 
GO
INSERT [dbo].[PostCategories] ([Id], [Name], [CompanyId]) VALUES (1, N'Thông báo chung', 1)
GO
INSERT [dbo].[PostCategories] ([Id], [Name], [CompanyId]) VALUES (2, N'Tin tức công ty', 1)
GO
INSERT [dbo].[PostCategories] ([Id], [Name], [CompanyId]) VALUES (4, N'Nhân sự', 1)
GO
INSERT [dbo].[PostCategories] ([Id], [Name], [CompanyId]) VALUES (5, N'Chính sách và quy định', 1)
GO
INSERT [dbo].[PostCategories] ([Id], [Name], [CompanyId]) VALUES (7, N'Thông báo quan trọng', 1)
GO
INSERT [dbo].[PostCategories] ([Id], [Name], [CompanyId]) VALUES (8, N'Xử phạt', 1)
GO
INSERT [dbo].[PostCategories] ([Id], [Name], [CompanyId]) VALUES (9, N'Thông báo quan trọng', 2)
GO
SET IDENTITY_INSERT [dbo].[PostCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([Id], [Name], [IsAdmin], [CompanyId]) VALUES (1, N'Admin', 1, 1)
GO
INSERT [dbo].[Roles] ([Id], [Name], [IsAdmin], [CompanyId]) VALUES (2, N'Trưởng phòng', 0, 1)
GO
INSERT [dbo].[Roles] ([Id], [Name], [IsAdmin], [CompanyId]) VALUES (3, N'Nhân viên', 0, 1)
GO
INSERT [dbo].[Roles] ([Id], [Name], [IsAdmin], [CompanyId]) VALUES (4, N'Admin', 1, 2)
GO
INSERT [dbo].[Roles] ([Id], [Name], [IsAdmin], [CompanyId]) VALUES (5, N'Nhân viên', 0, 2)
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([Id], [Name], [Email], [PhoneNumber], [PasswordHash], [RefreshToken]) VALUES (1, N'Lâm Anh', N'lamanh@gmail.com', N'0381234567', N'AQAAAAIAAYagAAAAELoD4bcLbwq7OVJml2wPlBiAglD1VowHHTGqivSbX9tI94SRh9qhPiU5S21AIsih0Q==', NULL)
GO
INSERT [dbo].[Users] ([Id], [Name], [Email], [PhoneNumber], [PasswordHash], [RefreshToken]) VALUES (2, N'Văn An', N'vanan@gmail.com', N'0384513881', N'AQAAAAIAAYagAAAAEBwtEE2SxvfPCzzuk6D0F9GsN1Ywq1vwIuO/TLnEFDzQuMRh7Hf/OFdR0BxRyr7NUA==', N'ouwczyzR+VaNjkJKOLNGy1dPZtkl6g/4b0O7ahkLM2k=')
GO
INSERT [dbo].[Users] ([Id], [Name], [Email], [PhoneNumber], [PasswordHash], [RefreshToken]) VALUES (3, N'Trân Anh', N'trananh@gmail.com', N'0981234567', N'AQAAAAIAAYagAAAAEEaQasq8nlLjL5tGk46Uxc5P3zodqBMSm0IOfkNzbHVpnjhzDPlp4f/X1oW26zfLMg==', N'0rRugUQsO23MyAjaOANMuB+O73KQSUEyNuKAHKhuNb0=')
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Schedules] ON 
GO
INSERT [dbo].[Schedules] ([Id], [Title], [Description], [Content], [StartDate], [EndDate], [DepartmentId], [CompanyId], [EmployeeId]) VALUES (2, N'Họp bàn kế hoạch dự án Zaho', N'Họp bàn về giai đoạn đầu phát triển Zaho', N'<p><b>Các nội dung chính:</b></p><ol><li>Xác định mục tiêu</li><li>Xác định yêu cầu</li><li>Lập kế hoạch ban đầu</li></ol>', CAST(N'2024-05-23T13:30:00.0000000' AS DateTime2), CAST(N'2024-05-23T15:00:00.0000000' AS DateTime2), 5, 1, 1)
GO
INSERT [dbo].[Schedules] ([Id], [Title], [Description], [Content], [StartDate], [EndDate], [DepartmentId], [CompanyId], [EmployeeId]) VALUES (3, N'Liên hoan kỷ niệm ứng dụng Taem 5 tuổi', N'Buổi liên hoan dành cho tất cả nhân viên', NULL, CAST(N'2024-05-22T17:30:00.0000000' AS DateTime2), CAST(N'2024-05-22T19:00:00.0000000' AS DateTime2), NULL, 1, 1)
GO
INSERT [dbo].[Schedules] ([Id], [Title], [Description], [Content], [StartDate], [EndDate], [DepartmentId], [CompanyId], [EmployeeId]) VALUES (5, N'Họp kế hoạch cho dự án web mới', N'Buổi họp ngắn để trao đổi', NULL, CAST(N'2024-05-22T21:30:00.0000000' AS DateTime2), CAST(N'2024-05-22T23:00:00.0000000' AS DateTime2), 5, 1, 1)
GO
INSERT [dbo].[Schedules] ([Id], [Title], [Description], [Content], [StartDate], [EndDate], [DepartmentId], [CompanyId], [EmployeeId]) VALUES (7, N'Phỏng vấn thí sinh vòng 2', N'Buổi phỏng vấn lần 2', NULL, CAST(N'2024-05-22T07:30:00.0000000' AS DateTime2), CAST(N'2024-05-22T11:30:00.0000000' AS DateTime2), 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Schedules] OFF
GO
SET IDENTITY_INSERT [dbo].[Posts] ON 
GO
INSERT [dbo].[Posts] ([Id], [Title], [Description], [Content], [CreatedDate], [PostCategoryId], [DepartmentId], [CompanyId], [EmployeeId]) VALUES (3, N'Tuyên dương nhân viên xuất sắc tháng 5', N'Danh sách nhân viên xuất sắc được khen thưởng tháng 5', N'<p>Danh sách nhân viên</p><ul><li>Anh Văn Nha</li><li>Ngô Huỳnh Đức</li><li>Ngô Hữu Thành</li></ul>', CAST(N'2024-05-22T17:29:10.6560619' AS DateTime2), NULL, 1, 1, 1)
GO
INSERT [dbo].[Posts] ([Id], [Title], [Description], [Content], [CreatedDate], [PostCategoryId], [DepartmentId], [CompanyId], [EmployeeId]) VALUES (5, N'Xử phạt nhân viên vi phạm', N'Danh sách xử phạt những nhân viên không tuân thủ', N'<p><b>Các nhân viên:</b></p><ol><li>Văn lâm</li><li>Ngọc Khánh</li><li>Ngọc Đức</li><li>Huy Toàn</li></ol><p>Có hiêuj lực</p>', CAST(N'2024-05-22T20:15:24.6990586' AS DateTime2), 1, 2, 1, 1)
GO
INSERT [dbo].[Posts] ([Id], [Title], [Description], [Content], [CreatedDate], [PostCategoryId], [DepartmentId], [CompanyId], [EmployeeId]) VALUES (6, N'KHen thưởng nhân viên xuất sắc tháng 6', N'Danh sách khen thưởng nhân viên có nỗ lực tốt', N'<p style="text-align: center; "><b>Các nhân viên:</b></p><ol><li>Anh Dũng</li><li>Ngọc Đỗ</li><li>Hùng Danh</li></ol><p style="text-align: center; "><img src="https://synnexfpt.com/wp-content/uploads/2022/04/10-meo-giup-nhung-cuoc-hop-hang-tuan-hieu-qua-hon-05.jpg" style="width: 50%;"><br></p>', CAST(N'2024-05-22T20:21:51.5702241' AS DateTime2), 2, NULL, 1, 1)
GO
INSERT [dbo].[Posts] ([Id], [Title], [Description], [Content], [CreatedDate], [PostCategoryId], [DepartmentId], [CompanyId], [EmployeeId]) VALUES (7, N'Chào mừng thành viên mới', N'Thông báo chào thành viên', N'<h1 class=""><i><b>Nội dung bài đăng</b></i></h1>', CAST(N'2024-05-22T20:34:01.0681089' AS DateTime2), 9, NULL, 2, 3)
GO
SET IDENTITY_INSERT [dbo].[Posts] OFF
GO
SET IDENTITY_INSERT [dbo].[UserCompanies] ON 
GO
INSERT [dbo].[UserCompanies] ([Id], [UserId], [CompanyId], [RoleId], [DepartmentId]) VALUES (1, 1, 1, 1, 2)
GO
INSERT [dbo].[UserCompanies] ([Id], [UserId], [CompanyId], [RoleId], [DepartmentId]) VALUES (5, 3, 2, 4, NULL)
GO
INSERT [dbo].[UserCompanies] ([Id], [UserId], [CompanyId], [RoleId], [DepartmentId]) VALUES (6, 2, 2, 5, NULL)
GO
SET IDENTITY_INSERT [dbo].[UserCompanies] OFF
GO
SET IDENTITY_INSERT [dbo].[Permissions] ON 
GO
INSERT [dbo].[Permissions] ([Id], [Name]) VALUES (1, N'None')
GO
INSERT [dbo].[Permissions] ([Id], [Name]) VALUES (2, N'Edit')
GO
INSERT [dbo].[Permissions] ([Id], [Name]) VALUES (3, N'View')
GO
SET IDENTITY_INSERT [dbo].[Permissions] OFF
GO
SET IDENTITY_INSERT [dbo].[Resources] ON 
GO
INSERT [dbo].[Resources] ([Id], [Name]) VALUES (1, N'Post')
GO
INSERT [dbo].[Resources] ([Id], [Name]) VALUES (2, N'Schedule')
GO
INSERT [dbo].[Resources] ([Id], [Name]) VALUES (3, N'PostCategory')
GO
INSERT [dbo].[Resources] ([Id], [Name]) VALUES (4, N'Department')
GO
INSERT [dbo].[Resources] ([Id], [Name]) VALUES (5, N'Role')
GO
INSERT [dbo].[Resources] ([Id], [Name]) VALUES (6, N'Member')
GO
SET IDENTITY_INSERT [dbo].[Resources] OFF
GO
SET IDENTITY_INSERT [dbo].[RolePermissions] ON 
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (1, 2, 2, 1)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (2, 2, 2, 2)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (3, 2, 2, 3)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (4, 2, 2, 4)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (5, 2, 3, 5)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (6, 2, 3, 6)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (7, 3, 3, 1)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (8, 3, 3, 2)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (9, 3, 1, 3)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (10, 3, 1, 4)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (11, 3, 1, 5)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (12, 3, 1, 6)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (13, 5, 3, 1)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (14, 5, 3, 2)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (15, 5, 3, 3)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (16, 5, 3, 4)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (17, 5, 3, 5)
GO
INSERT [dbo].[RolePermissions] ([Id], [RoleId], [PermissionId], [ResourceId]) VALUES (18, 5, 3, 6)
GO
SET IDENTITY_INSERT [dbo].[RolePermissions] OFF
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240522085541_init', N'8.0.4')
GO
