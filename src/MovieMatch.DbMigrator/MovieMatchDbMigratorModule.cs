using MovieMatch.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace MovieMatch.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MovieMatchEntityFrameworkCoreModule),
    typeof(MovieMatchApplicationContractsModule)
    )]
public class MovieMatchDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
