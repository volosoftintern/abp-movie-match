using MovieMatch.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MovieMatch.Permissions;

public class MovieMatchPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(MovieMatchPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MovieMatchPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MovieMatchResource>(name);
    }
}
