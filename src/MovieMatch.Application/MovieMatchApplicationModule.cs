
using Microsoft.Extensions.DependencyInjection;
using MovieMatch.UserConnections;
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
using Volo.Abp.BlobStoring;
using Volo.CmsKit;
using Volo.Abp.BlobStoring.FileSystem;

namespace MovieMatch;

[DependsOn(

    typeof(MovieMatchDomainModule),
    typeof(AbpBlobStoringFileSystemModule),
    typeof(AbpAccountApplicationModule),
    typeof(MovieMatchApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(CmsKitApplicationModule)
    )]

[DependsOn(typeof(CmsKitApplicationModule))]

    public class MovieMatchApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<MovieMatchApplicationModule>();
        });

            //  context.Services.AddSingleton<IUserConnectionRepository>();
        
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = "wwwroot/images";
                });
            });
        });

        MovieDbFactory.RegisterSettings(MovieApiConstants.ApiKey);

       
    }
}

