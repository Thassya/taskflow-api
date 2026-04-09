using System;
using NUnit.Framework;
using TaskFlow.Api.Repositories;
using TaskFlow.Api.Services;
using TaskFlow.Api.Tests.Fakes;

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
    }
}