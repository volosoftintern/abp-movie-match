using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace MovieMatch.Web;

[Dependency(ReplaceServices = true)]
public class MovieMatchBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "MovieMatch";
}
