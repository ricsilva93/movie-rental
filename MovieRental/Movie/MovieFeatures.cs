using Microsoft.EntityFrameworkCore;
using MovieRental.Controllers.Dtos;
using MovieRental.Data;

namespace MovieRental.Movie
{
	public class MovieFeatures : IMovieFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		public MovieFeatures(MovieRentalDbContext movieRentalDb)
		{
			_movieRentalDb = movieRentalDb;
		}
		
		public Movie Save(Movie movie)
		{
			_movieRentalDb.Movies.Add(movie);
			_movieRentalDb.SaveChanges();
			return movie;
		}

		// TODO: tell us what is wrong in this method? Forget about the async, what other concerns do you have?
		// ienumrable, page, etc
		public async Task<PagedResult<Movie>> GetAll() //page
		{
			var result = await _movieRentalDb.Movies.ToListAsync(); //TODO /me fix
			var total = 1;
			int page = 1;
            int pagesize = 1;
			return new PagedResult<Movie>(result, page, pagesize, total);
		}


	}
}
