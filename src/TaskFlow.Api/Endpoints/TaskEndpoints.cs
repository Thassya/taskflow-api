using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using TaskFlow.Api.Contracts;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Endpoints
{
    public static class TaskEndpoints
    {
        public static void MapTaskEndpoints(this WebApplication app)
        {
            app.MapPost("/tasks", (CreateTaskRequest request, TaskService service) =>
            {
                try
                {
                    var createdTask = service.CreateTask(request.Title, request.Description);
                    return Results.Created($"/tasks/{createdTask.Id}", createdTask);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
            });

            app.MapGet("/tasks", (TaskService service) =>
            {
                var tasks = service.GetAllTasks();
                return Results.Ok(tasks);
            });

            app.MapGet("/tasks/{id:guid}", (Guid id, TaskService service) =>
            {
                var task = service.GetTaskById(id);

                if (task == null)
                {
                    return Results.NotFound(new { message = "Task not found." });
                }

                return Results.Ok(task);
            });

            app.MapPut("/tasks/{id:guid}", (Guid id, UpdateTaskRequest request, TaskService service) =>
            {
                try
                {
                    var updatedTask = service.UpdateTask(id, request.Title, request.Description);
                    return Results.Ok(updatedTask);
                }
                catch (ArgumentException ex)
                {
                    if (ex.Message == "Task not found.")
                    {
                        return Results.NotFound(new { message = ex.Message });
                    }

                    return Results.BadRequest(new { message = ex.Message });
                }
            });

            app.MapPatch("/tasks/{id:guid}/complete", (Guid id, TaskService service) =>
            {
                try
                {
                    var completedTask = service.CompleteTask(id);
                    return Results.Ok(completedTask);
                }
                catch (ArgumentException ex)
                {
                    return Results.NotFound(new { message = ex.Message });
                }
            });

            app.MapDelete("/tasks/{id:guid}", (Guid id, TaskService service) =>
            {
                try
                {
                    service.DeleteTask(id);
                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.NotFound(new { message = ex.Message });
                }
            });
        }
    }
}