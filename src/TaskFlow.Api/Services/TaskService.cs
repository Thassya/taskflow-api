using System;
using TaskFlow.Api.Domain;
using TaskFlow.Api.Repositories;

namespace TaskFlow.Api.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public TaskItem CreateTask(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required.");
            }

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                IsCompleted = false
            };

            _repository.Add(task);

            return task;
        }

        public IList<TaskItem> GetAllTasks()
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Empty title",
                Description = "test failure",
                IsCompleted = false
            };

            var list = new List<TaskItem>();
            list.Add(task);

            return list;
            //return _repository.GetAll();
        }
    }
}