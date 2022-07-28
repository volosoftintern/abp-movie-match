using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieMatch.Movies;
using System.Threading.Tasks;

namespace MovieMatch.Web.Pages.Movies
{
    public class DirectorModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int DirectorId { get; set; }
        public PersonDto DirectorDetail { get; set; }

        private IMovieAppService _movieAppService;

        public DirectorModel(IMovieAppService movieAppService)
        {
            _movieAppService = movieAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            await GetDataAsync();
            return Page();
        }

        private async Task GetDataAsync()
        {
            DirectorDetail = await _movieAppService.GetDirector(DirectorId);
        }
    }
}
