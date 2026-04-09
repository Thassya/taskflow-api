using System;
using System.Collections.Generic;
using TaskFlow.Api.Domain;

namespace TaskFlow.Api.Repositories
{
    public interface ITaskRepository
    {
        void Add(TaskItem task);
        TaskItem GetById(Guid id);
        IList<TaskItem> GetAll();
        void Update(TaskItem task);
        void Delete(Guid id);
    }
}