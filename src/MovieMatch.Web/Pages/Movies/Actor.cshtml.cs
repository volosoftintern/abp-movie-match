using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieMatch.Movies;
using System.Threading.Tasks;

namespace MovieMatch.Web.Pages.Movies
{
    public class ActorModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int ActorId { get; set; }
        public PersonDto Actor { get; set; }

        private IMovieAppService _movieAppService;

        public ActorModel(IMovieAppService movieAppService)
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
            Actor = await _movieAppService.GetPersonAsync(ActorId);
        }
    }
}
