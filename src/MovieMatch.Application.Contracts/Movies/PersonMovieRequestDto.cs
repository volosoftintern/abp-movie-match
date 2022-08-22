using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch.Movies
{
    public class PersonMovieRequestDto
    {
        public int MaxCount { get;}=20;

        public int PersonId { get; set; }
        public bool IsDirector { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int SkipCount { get; set; } = 0;
        
    }
}
