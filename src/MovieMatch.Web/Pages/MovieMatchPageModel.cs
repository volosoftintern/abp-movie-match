using MovieMatch.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MovieMatch.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class MovieMatchPageModel : AbpPageModel
{
    protected MovieMatchPageModel()
    {
        LocalizationResourceType = typeof(MovieMatchResource);
    }
}
