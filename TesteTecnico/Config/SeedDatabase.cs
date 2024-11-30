using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using TesteTecnico.Data;
using TesteTecnico.DataModel;

namespace TesteTecnico.Config
{
    public class SeedDatabase
    {
        public static void Seed(AppDbContext dbContext, string csvFileName)
        {
            if (!dbContext.Movies.Any())
            {
                var solutionDirectory = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
                var csvFilePath = Path.Combine(solutionDirectory, csvFileName);

                if (!File.Exists(csvFilePath))
                {
                    Console.WriteLine($"CSV file not found at: {csvFilePath}");
                    return; // Não faz nada se o arquivo não for encontrado
                }

                using var reader = new StreamReader(csvFilePath);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";", // Define o delimitador como ponto e vírgula
                    HeaderValidated = null, // Desabilita a validação de cabeçalhos
                    MissingFieldFound = null, // Ignora campos ausentes
                });

                csv.Context.RegisterClassMap<MovieMap>();

                var movies = csv.GetRecords<Movie>().ToList();
                dbContext.Movies.AddRange(movies);
                dbContext.SaveChanges();
            }
        }
    }
}
