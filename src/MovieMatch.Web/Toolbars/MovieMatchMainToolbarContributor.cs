using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;

namespace MovieMatch.Web.Toolbars
{
    public class MovieMatchMainToolbarContributor: IToolbarContributor
    {
        public async Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name == LeptonXLiteToolbars.Main)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(MovieMatchToolbars)));
            }

            if (context.Toolbar.Name == LeptonXLiteToolbars.MainMobile)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(MovieMatchToolbars)));
            }
        }
    }
}
