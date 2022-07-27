using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieMatch.Search
{
    public class PopularMovieDto
    {
        [Required]
        public int CurrentPage { get; set; } = 1;
    }
}
