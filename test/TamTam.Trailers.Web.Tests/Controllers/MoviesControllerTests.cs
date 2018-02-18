namespace TamTam.Trailers.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Moq;
    using TamTam.Trailers.Infrastructure.Model;
    using TamTam.Trailers.Infrastructure.Services;
    using Xunit;

    public class MoviesControllerTests
    {
        private readonly MoviesController controller;
        private readonly Mock<IMovieService> service;

        public MoviesControllerTests()
        {
            service = new Mock<IMovieService>();
            controller = new MoviesController(service.Object);
        }

        [Fact]
        public async Task Search_WithMoviesWithoutPoster_ReturnsOnlyMoviesWithPoster()
        {
            // Arrange
            var movies = new[]
            {
                new Movie { Poster = "Poster" },
                new Movie(),
            };
            service.Setup(x => x.Search("")).ReturnsAsync(movies);
            
            // Act
            var results = await controller.Search("");

            // Assert
            Assert.Equal(movies.Take(1), results);
        }
    }
}