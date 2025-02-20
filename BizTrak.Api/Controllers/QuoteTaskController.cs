using BizTrak.Data;
using BizTrak.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizTrak.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteTaskController(IQuoteTaskRepository quoteTaskRepository) : ControllerBase
    {
        private readonly IQuoteTaskRepository _quoteTaskRepository = quoteTaskRepository;

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<QuoteTask>>> ListTasks()
        // {
        //     var tasks = await _quoteTaskRepository.ListTasksAsync();
        //      return Ok(tasks);
        // }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<QuoteTask>> GetTask(int id)
        // {
        //     var task = await _quoteTaskRepository.GetTaskAsync(id);

        //     if (task == null)
        //     {
        //         return NotFound();
        //     }

        //     return task;
        // }
    }
}
