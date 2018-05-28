IF OBJECT_ID('[dbo].[UERS_ROLES]') IS NOT NULL
  DROP VIEW [dbo].[UERS_ROLES]
GO
CREATE VIEW [dbo].[UERS_ROLES]
AS
SELECT        dbo.Users.Name, dbo.Roles.Name AS Expr1, dbo.Users.Email
FROM            dbo.Roles INNER JOIN
                         dbo.Users ON dbo.Roles.Id = dbo.Users.Role_Id
GO