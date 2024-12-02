using TesteTecnico.Data;
using TesteTecnico.DataModel;
using TesteTecnico.Models;

namespace TesteTecnico.Repositories
{
    public class MovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Intervalo>> GetMaiorMenorIntervalo()
        {
            var venc = _context.Movies
                .Where(m => m.Winner)
                .AsEnumerable()
                .SelectMany(m => m.Producer.Replace(", and ", ",")
                                           .Replace(" and ", ",")
                                           .Split(separator: ',')
                .Select(p => new { m.Year, Producer = p.Trim() }))
                .GroupBy(m => m.Producer)
                .Where(m => m.Count() > 1)
                .SelectMany(m =>
                {
                    var vencedores = m.OrderBy(p => p.Year).ToList();
                    var intervalos = new List<Intervalo>();

                    for (int i = 0; i < vencedores.Count - 1; i++)
                    {
                        Intervalo inter = new Intervalo()
                        {
                            Producer = m.Key,
                            Interval = vencedores[i + 1].Year - vencedores[i].Year,
                            PreviousWin = vencedores[i].Year,
                            FollowingWin = vencedores[i + 1].Year
                        };
                        intervalos.Add(inter);
                    }
                    return intervalos;
                })
                .ToList();

            return venc;           
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
