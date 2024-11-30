using CsvHelper.Configuration;
using TesteTecnico.Data;
using TesteTecnico.DataModel;

namespace TesteTecnico.Config
{
    public class  MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Map(m => m.Year).Name("year");
            Map(m => m.Title).Name("title");
            Map(m => m.Studios).Name("studios");
            Map(m => m.Producer).Name("producers");
            Map(m => m.Winner)
                .Name("winner")
                .TypeConverterOption.BooleanValues(true, true, "yes")
                .TypeConverterOption.BooleanValues(false, true, ""); // Mapeia "yes" como true e "no" como false
        }
    }
}
