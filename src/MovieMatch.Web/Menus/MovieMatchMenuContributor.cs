using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MovieMatch.Localization;
using MovieMatch.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace MovieMatch.Web.Menus;

public class MovieMatchMenuContributor : IMenuContributor
{

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        if (context.Menu.Name == StandardMenus.User)
        {
            var userMenu = context.Menu.FindMenuItem(IdentityMenuNames.Users);
            if(userMenu == null)
            {
            }                userMenu = new ApplicationMenuItem(IdentityMenuNames.Users, "My Profile", "/UserConnections",icon:"fa fa-user");

            context.Menu.AddItem(userMenu);
        }
    }

   

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration =context.Menu.GetAdministration();
        var l = context.GetLocalizer<MovieMatchResource>();
        

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                MovieMatchMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fas fa-home",
                order: 0
            )
        );

      

        context.Menu.Items.Insert(
            1,
            new ApplicationMenuItem(
                MovieMatchMenus.Search,
                l["Menu:Search"],
                "/Search",
                icon: "fa fa-search",
                order: 1
            )
        );
        context.Menu.Items.Insert(
           2,
            new ApplicationMenuItem(
                MovieMatchMenus.Explore,
                l["Menu:Explore"],
                "/Explore",
                icon: "fa fa-users"
            )
        );
        context.Menu.Items.Insert(5,new ApplicationMenuItem("MovieMatch.Home", l["Menu:Chat"], "/Chat"));

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
    }
}
