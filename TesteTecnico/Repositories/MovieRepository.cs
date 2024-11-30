using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteTecnico.Data;
using TesteTecnico.DataModel;

namespace TesteTecnico.Repositories
{
    public class MovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetMaiorMenorIntervalo()
        {
            var winners = _context.Movies
                .Where(m => m.Winner)
                .AsEnumerable()
                .SelectMany(m => m.Producer.Replace(", and ", ",")
                                           .Replace(" and ", ",")
                                           .Split(separator: ',')
                .Select(p => new { m.Year, Producer = p.Trim() })) 
                .OrderBy(m => m.Producer)
                .ThenBy(m => m.Year)
                .ToList();

            var producerIntervals = winners
                .GroupBy(w => w.Producer)
                .Select(ganhadores =>
                {
                    var intervalos = ganhadores.Zip(ganhadores.Skip(1), (prev, next) => new
                    {
                        Producer = ganhadores.Key,
                        Interval = next.Year - prev.Year
                    }).ToList();

                    return new
                    {
                        Producer = ganhadores.Key,
                        MaxInterval = intervalos.Any() ? intervalos.Max(i => i.Interval) : 0,
                        MinInterval = intervalos.Any() ? intervalos.Min(i => i.Interval) : 0
                    };
                })
                .Where(p => p.MaxInterval > 0)
                .ToList();

            var maiorIntervalo = producerIntervals.OrderByDescending(p => p.MaxInterval).FirstOrDefault();
            var menorIntervalo = producerIntervals.OrderBy(p => p.MinInterval).FirstOrDefault();
            return $"Produtor com maior intervalo: {maiorIntervalo.Producer} com intervalo de {maiorIntervalo.MaxInterval} anos" +
                $"\nProdutor com maior intervalo: {menorIntervalo.Producer} com intervalo de {menorIntervalo.MaxInterval} anos";
        }

        public async Task AddAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<Movie> FindAsync(int id)
        {
           return await _context.Movies.FindAsync(id);
        }

        public async Task UpdateMovie(int id, Movie updatedMovie, Movie movie)
        {

            movie.Year = updatedMovie.Year;
            movie.Title = updatedMovie.Title;
            movie.Studios = updatedMovie.Studios;
            movie.Producer = updatedMovie.Producer;
            movie.Winner = updatedMovie.Winner;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovie(Movie movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}
