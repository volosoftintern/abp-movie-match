using System;
using MovieMatch.Genres;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using System.Collections.ObjectModel;

namespace MovieMatch.Movies
{
    public class MovieDetail:Entity<int>
    {
        public string Title { get; set; }
        public string PosterPath { get; set; }
        public string Overview { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double VoteAverage { get; set; }
        public string ImdbId { get; set; }
        public int DirectorId { get; set; }
        public ICollection<MoviePerson> Stars { get; set; }
        public ICollection<MovieGenre> Genres { get; set; }

        private MovieDetail()
        {

        }

        public MovieDetail(int id,string title, string posterPath, string overview, DateTime releaseDate, double voteAverage, string imdbId, int directorId)
        {
            Id = id;
            Title = title;
            PosterPath = posterPath;
            Overview = overview;
            ReleaseDate = releaseDate;
            VoteAverage = voteAverage;
            ImdbId = imdbId;
            DirectorId = directorId;
            Genres=new Collection<MovieGenre>();
            Stars =new Collection<MoviePerson>();
        }
    }
}
