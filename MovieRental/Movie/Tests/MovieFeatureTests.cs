using Moq;
using MovieRental.Configuration.Validation;
using MovieRental.Controllers.DTOs;
using MovieRental.Movie.Repositories;

namespace MovieRental.Movie.Tests
{
    [TestClass]
    public class MovieFeatureTests
    {

        private Mock<IMovieRepository> _movieRepositoryMock;
        private MovieFeatures _feature;

        [TestInitialize]
        public void Initialize()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _feature = new MovieFeatures(_movieRepositoryMock.Object);
        }


        [TestMethod]
        public async Task SaveAsync_ValidDataInserted_ShouldSaveAndReturn()
        {
            // Arrange
            var movie = new Movie { Title = "Taxi Driver" };
            var expectedMovie = new Movie { Title = "Taxi Driver", Id = 1 };

            _movieRepositoryMock
                .Setup(r => r.SaveAsync(movie, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedMovie);

            // Act
            var result = await _feature.SaveAsync(movie);

            // Assert
            Assert.AreEqual(expectedMovie, result);
            _movieRepositoryMock.Verify(
                r => r.SaveAsync(movie, It.IsAny<CancellationToken>()),
                Times.Once);
        }


        [TestMethod]
        public async Task SaveAsync_TitleNotValid_ShouldThrowException()
        {
            // Arrange
            var movie = new Movie { Title = "   " };

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _feature.SaveAsync(movie));
        }

        [TestMethod]
        public async Task GetAllAsync_ValidRequest_ShouldReturn()
        {
            // Arrange
            int page = 2;
            int pageSize = 5;

            var movies = new List<MovieResponseDto>
            {
                new MovieResponseDto (10, "Inception")
            };

            var expected = new PagedResult<MovieResponseDto>(movies, page, pageSize, 1);

            _movieRepositoryMock
                .Setup(r => r.GetAllAsync(page, pageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _feature.GetAllAsync(page, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Total);
            Assert.AreEqual("Inception", result.Result.First().Title);
            Assert.AreEqual(page, result.PageNumber);

            _movieRepositoryMock.Verify(r =>
                r.GetAllAsync(page, pageSize, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
