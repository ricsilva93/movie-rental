using Microsoft.EntityFrameworkCore;
using MovieRental.Controllers.Dtos;
using MovieRental.Controllers.DTOs;
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
		
		public async Task<Movie> SaveAsync(
			Movie movie,
            CancellationToken cancellationToken = default)
		{
			_movieRentalDb.Movies.Add(movie);
			await _movieRentalDb.SaveChangesAsync(cancellationToken);
			return movie;
		}

		// TODO: tell us what is wrong in this method? Forget about the async, what other concerns do you have?
		public async Task<PagedResult<MovieResponseDto>> GetAllAsync(
            int page = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default)
		{
			var query = _movieRentalDb.Movies
				.AsNoTracking()
				.OrderBy(movie => movie.Id);

			var total = await query.CountAsync();
            var result = await query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(movie => new MovieResponseDto(movie.Id, movie.Title))
				.ToListAsync(cancellationToken);

			return new PagedResult<MovieResponseDto>(result, page, pageSize, total);
		}


	}
}
