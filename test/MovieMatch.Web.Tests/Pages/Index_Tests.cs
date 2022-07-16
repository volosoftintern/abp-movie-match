using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace MovieMatch.Pages;

public class Index_Tests : MovieMatchWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
