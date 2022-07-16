using MovieMatch.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MovieMatch;

[DependsOn(
    typeof(MovieMatchEntityFrameworkCoreTestModule)
    )]
public class MovieMatchDomainTestModule : AbpModule
{

}
