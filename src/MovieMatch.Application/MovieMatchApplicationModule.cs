
using DM.MovieApi;
using MovieMatch.Movies;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.CmsKit;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.BlobStoring;

namespace MovieMatch;

[DependsOn(
    typeof(MovieMatchDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(MovieMatchApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(CmsKitApplicationModule),
    typeof(AbpBlobStoringFileSystemModule)
    )]

    public class MovieMatchApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<MovieMatchApplicationModule>();
        });

        Configure<AbpBlobStoringOptions>(options =>
        {

            options.Containers.ConfigureDefault(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = "C:\\my-files";
                });
            });
        });


        MovieDbFactory.RegisterSettings(MovieApiConstants.ApiKey);
    }
}
