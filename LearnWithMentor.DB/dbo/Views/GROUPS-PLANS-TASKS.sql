CREATE VIEW [dbo].[GROUPS-PLANS-TASKS]
AS
SELECT        dbo.Plans.Name, dbo.Plans.Description, dbo.Plans.Create_Date, dbo.Plans.Mod_Date, dbo.Plans.Published, dbo.PlanTasks.Priority, dbo.Sections.Name AS Section_Name, dbo.Tasks.Name AS Task_Name, dbo.Tasks.Description AS Task_Description, 
                         dbo.Tasks.Create_Date AS Tasks_Create_Date, dbo.Tasks.Mod_Date AS Task_Mod_Date, dbo.Tasks.Private
FROM            dbo.Plans INNER JOIN
                         dbo.PlanTasks ON dbo.Plans.Id = dbo.PlanTasks.Plan_Id INNER JOIN
                         dbo.Sections ON dbo.PlanTasks.Section_Id = dbo.Sections.Id INNER JOIN
                         dbo.Tasks ON dbo.PlanTasks.Task_Id = dbo.Tasks.Id
