using Microsoft.AspNetCore.Mvc;
using MovieRental.Configuration.Validation;
using MovieRental.Controllers.DTOs;
using MovieRental.Movie;
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
        public async Task<IActionResult> PostAsync(
            [FromBody] Rental.Rental rental,
            CancellationToken cancellationToken = default)
        {
	        return Ok(await _features.SaveAsync(rental, cancellationToken));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [Required][FromQuery] string customer,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            (page, pageSize) = PaginationValidator.Normalize(page, pageSize);

            var result = await _features.GetRentalsByCustomerNameAsync(customer, page, pageSize, cancellationToken);

            return Ok(result);
        }

	}
}
