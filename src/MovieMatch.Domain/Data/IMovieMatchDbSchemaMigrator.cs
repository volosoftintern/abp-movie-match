using System.Threading.Tasks;

namespace MovieMatch.Data;

public interface IMovieMatchDbSchemaMigrator
{
    Task MigrateAsync();
}
