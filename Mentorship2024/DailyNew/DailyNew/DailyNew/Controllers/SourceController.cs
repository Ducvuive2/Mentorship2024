using DailyNew.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DailyNew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private readonly DailyNewContext _context;

        public SourceController(DailyNewContext context)
        {
            _context = context;
        }
        // GET: api/<SourceController>
        [HttpGet]
        public async Task<IActionResult> GetRecentlyAddedAndTopSources()
        {
            try
            {
                var recentlyAddedSources = await _context.Sources
                    .OrderByDescending(s => s.CreatedAt)
                    .Take(10)
                    .ToListAsync();

                var topSourcesByArticleCount = await _context.Sources
                    .Select(s => new
                    {
                        Source = s,
                        ArticleCount = s.RssFeeds.SelectMany(r => r.Articles).Count()
                    })
                    .OrderByDescending(s => s.ArticleCount)
                    .Take(10)
                    .ToListAsync();


                var response = new
                {
                    RecentlyAddedSources = recentlyAddedSources,
                    TopSourcesByArticleCount = topSourcesByArticleCount
                };
                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }

        }

        // GET api/<SourceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SourceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SourceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SourceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
