using DM.MovieApi.MovieDb.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieMatch.Movies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieMatch.Web.Pages.Movies
{
    public class DetailModel : PageModel
    {
        private readonly IMovieAppService _movieAppService;

        [BindProperty(SupportsGet = true)]
        public int MovieId { get; set; }

        public MovieDetailDto MovieDetail { get; set; }
        public IReadOnlyList<MovieDto> SimilarMovies { get; set; }

        public DetailModel(IMovieAppService movieAppService)
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
            MovieDetail = await _movieAppService.GetAsync(MovieId);
            SimilarMovies = await _movieAppService.GetSimilarMoviesAsync(MovieId);
        }
    }
}
