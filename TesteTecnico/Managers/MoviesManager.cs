using TesteTecnico.Data;
using TesteTecnico.Repositories;
using TesteTecnico.Response;

namespace TesteTecnico.Managers
{
    public class MoviesManager
    {
        private readonly AppDbContext _context;
        public MoviesManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task<VencedoresMoviesResponse>  GetVencedoresMovies()
        {
            MovieRepository repo = new MovieRepository(_context);

            var listaIntervalos = await repo.GetMaiorMenorIntervalo();

            if(listaIntervalos == null)
            {
                return null;
            }

            var minimo = listaIntervalos.Min(w => w.Interval);
            var maximo = listaIntervalos.Max(w => w.Interval);

            var response = new VencedoresMoviesResponse();

            response.Max = listaIntervalos.Where(_c => _c.Interval == maximo).ToList();
            response.Min = listaIntervalos.Where(_c => _c.Interval == minimo).ToList();

            return response;
        }
    }
}
