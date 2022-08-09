using Microsoft.AspNetCore.Authorization;
using MovieMatch.Posts;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace MovieMatch.Web.Pages;

[Authorize]
public class IndexModel : MovieMatchPageModel
{
    public IndexModel()
    {
       
    }

    public void OnGet()
    {
        
    }
}
