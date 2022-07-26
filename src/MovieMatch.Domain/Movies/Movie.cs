using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MovieMatch.Movies
{
    public class Movie:Entity<int>
    {
        public string Title { get; set; }
        public string PosterPath { get; set; }
        public string Overview { get; set; }

        private Movie()
        {

        }

        public Movie(int id,string title,string posterPath,string overview)
        {
            Id = id;
            Title = title;  
            PosterPath = posterPath;
            Overview = overview;
        }
    }
}
