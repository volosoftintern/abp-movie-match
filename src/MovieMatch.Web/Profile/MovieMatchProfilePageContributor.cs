using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MovieMatch.Localization;
using MovieMatch.MoviesWatchedBefore;
using MovieMatch.MoviesWatchLater;
using MovieMatch.Web.Components.WatchedBefore;
using MovieMatch.Web.Components.WatchLater;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.Users;

public class MovieMatchProfilePageContributor : IProfileManagementPageContributor
{

    public Task<bool> CheckPermissionsAsync(ProfileManagementPageCreationContext context)
    {
        // You can check the permissions here
        return Task.FromResult(true);
    }

    public async Task ConfigureAsync(ProfileManagementPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<MovieMatchResource>>();
        var watchedBeforeService=context.ServiceProvider.GetRequiredService<IWatchedBeforeAppService>();
        var watchLaterService=context.ServiceProvider.GetRequiredService<IWatchLaterAppService>();
        var currentUser=context.ServiceProvider.GetRequiredService<ICurrentUser>();
        var watchedBeforeCount = await watchedBeforeService.GetCountAsync((Guid)currentUser.Id);
        var watchLaterCount = await watchLaterService.GetCountAsync((Guid)currentUser.Id);
        context.Groups.Add(
          new ProfileManagementPageGroup(
              "Volo.Abp.Account.WatchLater",
              l["ProfileTab:MoviesIWillWatch", new object [] { watchLaterCount }],
              typeof(WatchLaterViewComponent)
          )
      );
        context.Groups.Add(
         new ProfileManagementPageGroup(
            "Volo.Abp.Account.WatchedBefore",
            l["ProfileTab:MoviesIWatchedBefore", new object[] { watchedBeforeCount }],
            typeof(WatchedBeforeViewComponent)
        )
      );

        //return Task.CompletedTask;
    }


}
