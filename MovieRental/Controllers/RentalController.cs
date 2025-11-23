using Microsoft.AspNetCore.Mvc;
using MovieRental.Controllers.DTOs;
using MovieRental.Rental;
using System.ComponentModel.DataAnnotations;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {

        private readonly IRentalFeatures _features;

        public RentalController(IRentalFeatures features)
        {
            _features = features;
        }


        [HttpPost]
        [ProducesResponseType(typeof(Rental.Rental), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync(
            [FromBody] Rental.Rental rental,
            CancellationToken cancellationToken = default)
        {
            var result = await _features.SaveAsync(rental, cancellationToken);

            return Created("", result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<RentalResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(
            [Required][FromQuery] string customer,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _features.GetRentalsByCustomerNameAsync(customer, page, pageSize, cancellationToken);

            return Ok(result);
        }

    }
}
