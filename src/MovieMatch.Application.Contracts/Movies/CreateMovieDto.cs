using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch.Movies
{
    public class CreateMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PosterPath { get; set; }
        public string Overview { get; set; }

        public CreateMovieDto(int id, string title, string posterPath, string overview)
        {
            Id = id;
            Title = title;
            PosterPath = posterPath;
            Overview = overview;
            
        }
    }


}
