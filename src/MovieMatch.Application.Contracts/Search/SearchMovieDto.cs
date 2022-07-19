using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieMatch.Search
{
    public class SearchMovieDto
    {
        [Required]
        public string Name { get; set; }
    }
}
