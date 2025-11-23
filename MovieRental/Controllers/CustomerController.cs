using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRental.Data;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class CustomerController : ControllerBase
    {
        private readonly MovieRentalDbContext _dbcontext;

        public CustomerController(MovieRentalDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var customers = await _dbcontext.Customers.ToListAsync(cancellationToken);
            return Ok(customers);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Customer.Customer customer,
                                                              CancellationToken cancellationToken)
        {
            _dbcontext.Customers.Add(customer);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(GetAllAsync), new { id = customer.Id }, customer);
        }
    }
}
