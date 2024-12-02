using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteTecnico.Data;
using TesteTecnico.DataModel;
using TesteTecnico.Managers;
using TesteTecnico.Repositories;

namespace TesteTecnico.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController: ControllerBase
    {
        private readonly AppDbContext _context;
        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetVencedorMaiorMenorIntervalo")]
        public async Task<IActionResult> GetVencedorMaiorMenorIntervalo()
        {
            try
            {
                MoviesManager manager = new MoviesManager(_context);

                var resultado = await manager.GetVencedoresMovies();

                if(resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch
            {
                return BadRequest("Erro interno no servidor!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await new MovieRepository(_context).FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] Movie movie)
        {
            if (movie == null)
            {
                return BadRequest("O filme não pode ser null");
            }
            await new MovieRepository(_context).AddAsync(movie);
           
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] Movie updatedMovie)
        {
            if (id != updatedMovie.Id)
            {
                return BadRequest("ID não encontrado");
            }

            var movie = await new MovieRepository(_context).FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            await new MovieRepository(_context).UpdateMovie(id, updatedMovie, movie);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await new MovieRepository(_context).FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            await new MovieRepository(_context).DeleteMovie(movie);
            return NoContent();
        }
    }
}
