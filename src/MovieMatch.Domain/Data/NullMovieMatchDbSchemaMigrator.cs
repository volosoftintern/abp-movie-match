using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MovieMatch.Data;

/* This is used if database provider does't define
 * IMovieMatchDbSchemaMigrator implementation.
 */
public class NullMovieMatchDbSchemaMigrator : IMovieMatchDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
