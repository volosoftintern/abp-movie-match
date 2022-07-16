using Volo.Abp.Modularity;

namespace MovieMatch;

[DependsOn(
    typeof(MovieMatchApplicationModule),
    typeof(MovieMatchDomainTestModule)
    )]
public class MovieMatchApplicationTestModule : AbpModule
{

}
