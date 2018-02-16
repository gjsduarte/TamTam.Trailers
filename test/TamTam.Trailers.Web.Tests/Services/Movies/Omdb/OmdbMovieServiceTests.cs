namespace TamTam.Trailers.Web.Services.Movies.Omdb
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Moq;
    using TamTam.Trailers.Web.Extensions;
    using Xunit;

    public class OmdbMovieServiceTests : MovieServiceTests
    {
        public OmdbMovieServiceTests()
        {
            var options = Options.Create(new OmdbOptions
            {
                Address = "http://dummy/"
            });
            service = new OmdbMovieService(Factory.Object, options);

            SetupResponse(
                @"{
                ""Search"": [
                    {
                        ""Title"": ""Logan"",
                        ""Year"": ""2017"",
                        ""imdbID"": ""tt3315342"",
                        ""Type"": ""movie"",
                        ""Poster"": ""https://images-na.ssl-images-amazon.com/images/M/MV5BMjQwODQwNTg4OV5BMl5BanBnXkFtZTgwMTk4MTAzMjI@._V1_SX300.jpg""
                    },
                    {
                        ""Title"": ""Logan Paul Vs"",
                        ""Year"": ""2016–"",
                        ""imdbID"": ""tt6193484"",
                        ""Type"": ""series"",
                        ""Poster"": ""N/A""
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
            Assert.Equal("tt3315342", result.First().Id);
        }

        [Fact]
        public async Task Search_WithValidResponse_ParsesPoster()
        {
            // Act
            var result = await service.Search("Logan").ToArray();
            // Assert
            Assert.Equal(
                "https://images-na.ssl-images-amazon.com/images/M/MV5BMjQwODQwNTg4OV5BMl5BanBnXkFtZTgwMTk4MTAzMjI@._V1_SX300.jpg",
                result[0].Poster);
            Assert.Null(result[1].Poster);
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
            Assert.Equal(2016, result[1].Year);
        }
    }
}