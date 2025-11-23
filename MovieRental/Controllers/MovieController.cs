using Microsoft.AspNetCore.Mvc;
using MovieRental.Configuration.Validation;
using MovieRental.Controllers.DTOs;
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
        [ProducesResponseType(typeof(PagedResult<RentalResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default
            )
        {
            var result = await _features.GetAllAsync(
                page,
                pageSize,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Movie.Movie), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] Movie.Movie movie)
        {
            var result = await _features.SaveAsync(movie);

            return Created("", result);
        }
    }
}
