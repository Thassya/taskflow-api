using System;
using System.Collections.Generic;
using System.Linq;
using TaskFlow.Api.Domain;
using TaskFlow.Api.Repositories;

namespace TaskFlow.Api.Tests.Fakes
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<TaskItem> _tasks = new List<TaskItem>();

        public void Add(TaskItem task)
        {
            _tasks.Add(task);
        }

        public TaskItem GetById(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public IList<TaskItem> GetAll()
        {
            return _tasks.ToList();
        }

        public void Update(TaskItem task)
        {
            var existingTask = GetById(task.Id);

            if (existingTask == null)
            {
                return;
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.IsCompleted = task.IsCompleted;
        }

        public void Delete(Guid id)
        {
            var existingTask = GetById(id);

            if (existingTask != null)
            {
                _tasks.Remove(existingTask);
            }
        }
    }
}