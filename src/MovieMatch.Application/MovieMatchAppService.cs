using System;
using System.Collections.Generic;
using System.Text;
using MovieMatch.Localization;
using Volo.Abp.Application.Services;

namespace MovieMatch;

/* Inherit your application services from this class.
 */
public abstract class MovieMatchAppService : ApplicationService
{
    protected MovieMatchAppService()
    {
        LocalizationResource = typeof(MovieMatchResource);
    }
}
