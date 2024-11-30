using System.Text.Json;
using TesteTecnico.DataModel;
using FluentAssertions;

namespace TestesIntegracao
{
    public class UnitTest1
    {
        private readonly HttpClient _client;

        public UnitTest1()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44342") 
            };
        }


        [Fact]
        public async Task GetMovies_ShouldReturnAllMovies()
        {
            var response = await _client.GetAsync("/api/movies");

            response.EnsureSuccessStatusCode(); 
            var responseString = await response.Content.ReadAsStringAsync();

            var movies = JsonSerializer.Deserialize<Movie[]>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            movies.Should().NotBeNull(); 
            movies.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetMovies_ShouldReturnAStringTheMostAndLessInterval()
        {
            // Act
            var response = await _client.GetAsync("/api/Movies/GetVencedorMaiorMenorIntervalo");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            responseString.Should().NotBeNull();
            responseString.Should().Contain("Matthew Vaughn");
            responseString.Should().Contain("Joel Silver");
        }

    }
}