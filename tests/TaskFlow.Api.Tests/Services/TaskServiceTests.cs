using System;
using NUnit.Framework;
using TaskFlow.Api.Repositories;
using TaskFlow.Api.Services;
using TaskFlow.Api.Tests.Fakes;
using System.Linq;

namespace TaskFlow.Api.Tests.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        private ITaskRepository _repository;
        private TaskService _service;

        [SetUp]
        public void Setup()
        {
            _repository = new InMemoryTaskRepository();
            _service = new TaskService(_repository);
        }

        [Test]
        public void CreateTask_ShouldCreateTask_WhenTitleIsValid()
        {
            var result = _service.CreateTask("Study TDD", "Write the first tests");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Title, Is.EqualTo("Study TDD"));
            Assert.That(result.Description, Is.EqualTo("Write the first tests"));
            Assert.That(result.IsCompleted, Is.False);
        }

        [Test]
        public void CreateTask_ShouldThrowException_WhenTitleIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                _service.CreateTask("", "Invalid task"));

            Assert.That(ex.Message, Is.EqualTo("Title is required."));
        }

        [Test]
        public void GetAllTasks_ShouldReturnEmptyList_WhenThereAreNoTasks()
        {
            var result = _service.GetAllTasks();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }   

        [Test]
        public void GetAllTasks_ShouldReturnAllCreatedTasks()
        {
            _service.CreateTask("Task 1", "Description 1");
            _service.CreateTask("Task 2", "Description 2");

            var result = _service.GetAllTasks();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(t => t.Title == "Task 1"), Is.True);
            Assert.That(result.Any(t => t.Title == "Task 2"), Is.True);
        }

        [Test]
        public void GetTaskById_ShouldReturnTask_WhenIdExists()
        {
            var createdTask = _service.CreateTask("Study NUnit", "Read about assertions");

            var result = _service.GetTaskById(createdTask.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(createdTask.Id));
            Assert.That(result.Title, Is.EqualTo("Study NUnit"));
            Assert.That(result.Description, Is.EqualTo("Read about assertions"));
        }

        [Test]
        public void GetTaskById_ShouldReturnNull_WhenIdDoesNotExist()
        {
            var result = _service.GetTaskById(Guid.NewGuid());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateTask_ShouldUpdateTask_WhenTaskExists()
        {
            var createdTask = _service.CreateTask("Old Title", "Old Description");

            var updatedTask = _service.UpdateTask(createdTask.Id, "New Title", "New Description");

            Assert.That(updatedTask, Is.Not.Null);
            Assert.That(updatedTask.Id, Is.EqualTo(createdTask.Id));
            Assert.That(updatedTask.Title, Is.EqualTo("New Title"));
            Assert.That(updatedTask.Description, Is.EqualTo("New Description"));
            Assert.That(updatedTask.IsCompleted, Is.False);
        }

        [Test]
        public void UpdateTask_ShouldThrowException_WhenTaskDoesNotExist()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                _service.UpdateTask(Guid.NewGuid(), "New Title", "New Description"));

            Assert.That(ex.Message, Is.EqualTo("Task not found."));
        }

        [Test]
        public void UpdateTask_ShouldThrowException_WhenTitleIsEmpty()
        {
            var createdTask = _service.CreateTask("Valid Title", "Valid Description");

            var ex = Assert.Throws<ArgumentException>(() =>
                _service.UpdateTask(createdTask.Id, "", "Updated Description"));

            Assert.That(ex.Message, Is.EqualTo("Title is required."));
        }

        [Test]
        public void CompleteTask_ShouldMarkTaskAsCompleted_WhenTaskExists()
        {
            var createdTask = _service.CreateTask("Finish project", "Complete the TDD implementation");

            var completedTask = _service.CompleteTask(createdTask.Id);

            Assert.That(completedTask, Is.Not.Null);
            Assert.That(completedTask.Id, Is.EqualTo(createdTask.Id));
            Assert.That(completedTask.IsCompleted, Is.True);
        }

        [Test]
        public void CompleteTask_ShouldThrowException_WhenTaskDoesNotExist()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                _service.CompleteTask(Guid.NewGuid()));

            Assert.That(ex.Message, Is.EqualTo("Task not found."));
        }
    }
}