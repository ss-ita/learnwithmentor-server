CREATE VIEW [dbo].[USERS_ROLES]
AS
SELECT        dbo.Users.FirstName, dbo.Users.LastName, dbo.Roles.Name AS Roles_Name, dbo.Users.Email
FROM            dbo.Roles INNER JOIN
                         dbo.Users ON dbo.Roles.Id = dbo.Users.Role_Id
