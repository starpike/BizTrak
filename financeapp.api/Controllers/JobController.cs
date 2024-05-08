using FinanceApp.Data;
using FinanceApp.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController(IJobRepository jobRepository) : ControllerBase
    {
        private readonly IJobRepository _jobRepository = jobRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            var jobs = await _jobRepository.AllJobsAsync();
             return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            var job = await _jobRepository.GetJobAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return job;
        }
    }
}
