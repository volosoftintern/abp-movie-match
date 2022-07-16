using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieMatch.Data;
using Volo.Abp.DependencyInjection;

namespace MovieMatch.EntityFrameworkCore;

public class EntityFrameworkCoreMovieMatchDbSchemaMigrator
    : IMovieMatchDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreMovieMatchDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the MovieMatchDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<MovieMatchDbContext>()
            .Database
            .MigrateAsync();
    }
}
