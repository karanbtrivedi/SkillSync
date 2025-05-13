using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;

namespace SkillSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskApiController : ControllerBase
    {
        private readonly ITaskService _taskService;

        // Constructor injection of ITaskService to interact with business logic
        public TaskApiController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET api/taskapi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            if (tasks == null || !tasks.Any())
            {
                return NotFound();  // Return 404 if no tasks found
            }
            return Ok(tasks);  // Return 200 with the list of tasks
        }

        // GET api/taskapi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskViewModel>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();  // Return 404 if task not found
            }
            return Ok(task);  // Return 200 with the task details
        }

        // POST api/taskapi
        [HttpPost]
        public async Task<ActionResult<TaskViewModel>> CreateTask(TaskViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Task data is invalid.");
            }

            // Create a new task via the service
            var createdTask = await _taskService.CreateTaskAsync(model);

            // Return 201 (Created) with the location of the new task
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        // PUT api/taskapi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("Task ID mismatch.");  // Return 400 if ID doesn't match
            }

            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();  // Return 404 if task not found
            }

            await _taskService.UpdateTaskAsync(model);  // Update the task
            return NoContent();  // Return 204 No Content on success (No response body)
        }

        // DELETE api/taskapi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();  // Return 404 if task not found
            }

            await _taskService.DeleteTaskAsync(id);  // Delete the task
            return NoContent();  // Return 204 No Content on success (No response body)
        }
    }
}
