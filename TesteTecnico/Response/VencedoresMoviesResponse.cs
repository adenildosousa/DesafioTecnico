using TesteTecnico.Models;

namespace TesteTecnico.Response
{
    public class VencedoresMoviesResponse
    {
        public IEnumerable<Intervalo> Max { get; set; }
        public IEnumerable<Intervalo> Min { get; set; }
    }
}
