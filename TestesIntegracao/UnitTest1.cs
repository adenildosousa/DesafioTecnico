using System.Text.Json;
using TesteTecnico.DataModel;
using FluentAssertions;
using TesteTecnico.Response;

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
            var response = await _client.GetAsync("/api/Movies/GetVencedorMaiorMenorIntervalo");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var vencedores = JsonSerializer.Deserialize<VencedoresMoviesResponse>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            vencedores.Should().NotBeNull();
            vencedores.Min.Should().NotBeNull();
            vencedores.Max.Should().NotBeNull();
            vencedores.Max.Should().HaveCountGreaterThan(0);
            vencedores.Min.Should().HaveCountGreaterThan(0);
        }

    }
}