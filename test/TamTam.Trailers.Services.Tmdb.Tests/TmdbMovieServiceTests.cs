namespace TamTam.Trailers.Services.Tmdb
{
    using System.Linq;
    using System.Threading.Tasks;
    using TamTam.Trailers.Infrastructure.Extensions;
    using TamTam.Trailers.Infrastructure.Services;
    using TamTam.Trailers.Services.Tmdb.Options;
    using Xunit;

    public class TmdbMovieServiceTests : MovieServiceTests
    {
        public TmdbMovieServiceTests()
        {
            var options = Microsoft.Extensions.Options.Options.Create(
                new TmdbOptions
                    { Address = "http://dummy/" });
            service = new TmdbMovieService(Factory.Object, options);

            SetupResponse(
                @"{
                ""results"": [
                    {
                        ""id"": 263115,
                        ""vote_average"": 7.6,
                        ""title"": ""Logan"",
                        ""poster_path"": ""/gGBu0hKw9BGddG8RkRAMX7B6NDB.jpg"",
                        ""release_date"": ""2017-02-28""
                    },
                    {
                        ""id"": 413749,
                        ""vote_average"": 10,
                        ""title"": ""Logan's Run"",
                        ""poster_path"": null,
                        ""release_date"": """"
                    },
                ]
            }");
        }

        private readonly IMovieService service;

        [Fact]
        public async Task Search_WithValidResponse_ParsesId()
        {
            // Act
            var result = await service.Search("Logan");
            // Assert
            Assert.Equal("263115", result.First().Id);
        }

        [Fact]
        public async Task Search_WithValidResponse_ParsesPoster()
        {
            // Act
            var result = await service.Search("Logan").ToArray();
            // Assert
            Assert.Equal("http://image.tmdb.org/t/p/w185/gGBu0hKw9BGddG8RkRAMX7B6NDB.jpg", result[0].Poster);
            Assert.Null(result[1].Poster);
        }

        [Fact]
        public async Task Search_WithValidResponse_ParsesRating()
        {
            // Act
            var result = await service.Search("Logan");
            // Assert
            Assert.Equal(7.6, result.First().Rating);
        }

        [Fact]
        public async Task Search_WithValidResponse_ParsesResults()
        {
            // Act
            var result = await service.Search("Logan");
            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Search_WithValidResponse_ParsesTitle()
        {
            // Act
            var result = await service.Search("Logan");
            // Assert
            Assert.Equal("Logan", result.First().Title);
        }

        [Fact]
        public async Task Search_WithValidResponse_ParsesYear()
        {
            // Act
            var result = await service.Search("Logan").ToArray();
            // Assert
            Assert.Equal(2017, result[0].Year);
            Assert.Null(result[1].Year);
        }
    }
}