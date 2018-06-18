﻿using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface IPlanTaskRepository:IRepository<PlanTask>
    {
        PlanTask Get(int id);
        int? GetIdByTaskAndPlan(int taskId, int planId);
        bool ContainsTaskInPlan(int taskId, int planId);
        int? GetTaskPriorityInPlan(int taskId, int planId);
        int? GetTaskSectionIdInPlan(int taskId, int planId);
        int[] GetTasksIdForPlan(int planId);
        int[] GetPlansIdForTask(int taskId);
    }
}