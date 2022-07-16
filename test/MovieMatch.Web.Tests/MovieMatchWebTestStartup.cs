using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace MovieMatch;

public class MovieMatchWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<MovieMatchWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
