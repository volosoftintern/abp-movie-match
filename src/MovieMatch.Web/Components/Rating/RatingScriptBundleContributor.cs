using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.StarRatingSvg;
using Volo.Abp.Modularity;

namespace MovieMatch.Web.Components.Rating;

[DependsOn(typeof(StarRatingSvgScriptContributor))]
public class RatingScriptBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/client-proxies/cms-kit-common-proxy.js");
        context.Files.AddIfNotContains("/client-proxies/cms-kit-proxy.js");
        context.Files.AddIfNotContains("/Components/Rating/default.js");
    }
}
