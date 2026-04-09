using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace TaskFlow.Api.Tests.Integration
{
    [TestFixture]
    public class TaskEndpointsTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private JsonSerializerOptions _jsonOptions;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async System.Threading.Tasks.Task PostTasks_ShouldCreateTask_WhenRequestIsValid()
        {
            var request = new
            {
                Title = "Study API Tests",
                Description = "Create integration tests"
            };

            var response = await _client.PostAsJsonAsync("/tasks", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var createdTask = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);

            Assert.That(createdTask, Is.Not.Null);
            Assert.That(createdTask.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(createdTask.Title, Is.EqualTo("Study API Tests"));
            Assert.That(createdTask.Description, Is.EqualTo("Create integration tests"));
            Assert.That(createdTask.IsCompleted, Is.False);
        }

        [Test]
        public async System.Threading.Tasks.Task PostTasks_ShouldReturnBadRequest_WhenTitleIsEmpty()
        {
            var request = new
            {
                Title = "",
                Description = "Invalid task"
            };

            var response = await _client.PostAsJsonAsync("/tasks", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async System.Threading.Tasks.Task GetTasks_ShouldReturnCreatedTasks()
        {
            await _client.PostAsJsonAsync("/tasks", new
            {
                Title = "Task 1",
                Description = "Description 1"
            });

            await _client.PostAsJsonAsync("/tasks", new
            {
                Title = "Task 2",
                Description = "Description 2"
            });

            var response = await _client.GetAsync("/tasks");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var tasks = await response.Content.ReadFromJsonAsync<TaskResponse[]>(_jsonOptions);

            Assert.That(tasks, Is.Not.Null);
            Assert.That(tasks.Length, Is.GreaterThanOrEqualTo(2));
        }

        [Test]
        public async System.Threading.Tasks.Task GetTaskById_ShouldReturnTask_WhenIdExists()
        {
            var createResponse = await _client.PostAsJsonAsync("/tasks", new
            {
                Title = "Task by id",
                Description = "Find me"
            });

            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);

            var response = await _client.GetAsync("/tasks/" + createdTask.Id);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var task = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);

            Assert.That(task, Is.Not.Null);
            Assert.That(task.Id, Is.EqualTo(createdTask.Id));
        }

        [Test]
        public async System.Threading.Tasks.Task GetTaskById_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            var response = await _client.GetAsync("/tasks/" + Guid.NewGuid());

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async System.Threading.Tasks.Task PutTask_ShouldUpdateTask_WhenTaskExists()
        {
            var createResponse = await _client.PostAsJsonAsync("/tasks", new
            {
                Title = "Old title",
                Description = "Old description"
            });

            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);

            var response = await _client.PutAsJsonAsync("/tasks/" + createdTask.Id, new
            {
                Title = "New title",
                Description = "New description"
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var updatedTask = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);

            Assert.That(updatedTask, Is.Not.Null);
            Assert.That(updatedTask.Title, Is.EqualTo("New title"));
            Assert.That(updatedTask.Description, Is.EqualTo("New description"));
        }

        [Test]
        public async System.Threading.Tasks.Task PatchComplete_ShouldMarkTaskAsCompleted_WhenTaskExists()
        {
            var createResponse = await _client.PostAsJsonAsync("/tasks", new
            {
                Title = "Complete me",
                Description = "Finish task"
            });

            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);

            var response = await _client.PatchAsync("/tasks/" + createdTask.Id + "/complete", null);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var completedTask = await response.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);

            Assert.That(completedTask, Is.Not.Null);
            Assert.That(completedTask.IsCompleted, Is.True);
        }

        [Test]
        public async System.Threading.Tasks.Task DeleteTask_ShouldRemoveTask_WhenTaskExists()
        {
            var createResponse = await _client.PostAsJsonAsync("/tasks", new
            {
                Title = "Delete me",
                Description = "Remove task"
            });

            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>(_jsonOptions);

            var deleteResponse = await _client.DeleteAsync("/tasks/" + createdTask.Id);

            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            var getResponse = await _client.GetAsync("/tasks/" + createdTask.Id);

            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        private class TaskResponse
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public bool IsCompleted { get; set; }
        }
    }
}