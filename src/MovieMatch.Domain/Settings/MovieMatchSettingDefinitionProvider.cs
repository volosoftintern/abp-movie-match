using Volo.Abp.Settings;

namespace MovieMatch.Settings;

public class MovieMatchSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MovieMatchSettings.MySetting1));
    }
}
