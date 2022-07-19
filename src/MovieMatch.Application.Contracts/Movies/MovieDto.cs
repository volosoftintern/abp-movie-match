using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch.Movies
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PosterPath { get; set; }
        public string Overview { get; set; }
    }
}
