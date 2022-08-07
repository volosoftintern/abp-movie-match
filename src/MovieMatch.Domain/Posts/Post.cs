using MovieMatch.Movies;
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace MovieMatch.Posts
{
    public class Post:Entity<int>
    {
        public Guid UserId { get; set; }
        public int MovieId { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }

        private Post()
        {

        }

        public Post(Guid userId, int movieId, string comment, int rate)
        {
            UserId = userId;
            MovieId = movieId;
            Comment = comment;
            Rate = rate;
        }
    }
}
