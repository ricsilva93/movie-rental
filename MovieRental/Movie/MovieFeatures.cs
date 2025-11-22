using Microsoft.EntityFrameworkCore;
using MovieRental.Controllers.Dtos;
using MovieRental.Data;
using System.Threading;

namespace MovieRental.Movie
{
	public class MovieFeatures : IMovieFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		public MovieFeatures(MovieRentalDbContext movieRentalDb)
		{
			_movieRentalDb = movieRentalDb;
		}
		
		public Movie SaveAsync(
			Movie movie,
            CancellationToken cancellationToken = default)
		{
			_movieRentalDb.Movies.Add(movie);
			_movieRentalDb.SaveChangesAsync(cancellationToken);
			return movie;
		}

		// TODO: tell us what is wrong in this method? Forget about the async, what other concerns do you have?
		public async Task<PagedResult<Movie>> GetAllAsync(
            int page = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default) //page
		{
			var query = _movieRentalDb.Movies
				.AsNoTracking()
				.OrderBy(movie => movie.Id);

			var total = await query.CountAsync();
            var result = await query
				.Skip((page - 1) * pageSize)
				.Take(pageSize) //TODO me recheck
				.ToListAsync(cancellationToken);

			return new PagedResult<Movie>(result, page, pageSize, total);
		}


	}
}
