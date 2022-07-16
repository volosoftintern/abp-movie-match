using MovieMatch.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MovieMatch.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MovieMatchController : AbpControllerBase
{
    protected MovieMatchController()
    {
        LocalizationResource = typeof(MovieMatchResource);
    }
}
