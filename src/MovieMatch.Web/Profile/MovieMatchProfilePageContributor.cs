using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MovieMatch.Localization;
using MovieMatch.Web.Components.WatchedBefore;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

public class MovieMatchProfilePageContributor : IProfileManagementPageContributor
{
    public Task<bool> CheckPermissionsAsync(ProfileManagementPageCreationContext context)
    {
        // You can check the permissions here
        return Task.FromResult(true);
    }

    public Task ConfigureAsync(ProfileManagementPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<MovieMatchResource>>();
        context.Groups.Add(
          new ProfileManagementPageGroup(
              "Volo.Abp.Account.WatchLater",
              l["ProfileTab:MoviesWillWatch"],
              typeof(WatchLaterViewComponent)
          )
      );
        context.Groups.Add(
         new ProfileManagementPageGroup(
            "Volo.Abp.Account.WatchedBefore",
            l["ProfileTab:MoviesWatchedBefore"],
            typeof(WatchedBeforeViewComponent)
        )
      );

        return Task.CompletedTask;
    }
}
