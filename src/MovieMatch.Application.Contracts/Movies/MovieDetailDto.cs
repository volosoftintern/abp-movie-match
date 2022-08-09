using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch.Movies
{
    public class MovieDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string ImdbId { get; set; }
        public string Overview { get; set; }
        public string PosterPath { get; set; }
        public double VoteAverage { get; set; }

        public MovieMemeberDto Director { get; set; }
        public IEnumerable<MovieMemeberDto> Stars { get; set; }
        public IReadOnlyList<MovieGenreDto> Genres { get; set; }
        public bool IsActiveWatchLater { get; set; }
        public bool IsActiveWatchedBefore { get; set; }
    }
}
