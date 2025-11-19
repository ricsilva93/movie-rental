using Microsoft.AspNetCore.Mvc;
using MovieRental.Movie;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {

        private readonly IMovieFeatures _features;

        public MovieController(IMovieFeatures features)
        {
            _features = features;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _features.GetAll();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Movie.Movie movie)
        {
	        return Ok(_features.Save(movie));
        }
    }
}
