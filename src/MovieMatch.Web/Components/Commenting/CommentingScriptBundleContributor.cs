using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MovieMatch.Web.Components.Commenting;

public class CommentingScriptBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/client-proxies/cms-kit-common-proxy.js");
        context.Files.AddIfNotContains("/client-proxies/cms-kit-proxy.js");
        context.Files.AddIfNotContains("/Components/Commenting/default.js");
    }
}