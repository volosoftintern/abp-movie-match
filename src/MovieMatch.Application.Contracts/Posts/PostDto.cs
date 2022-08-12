using MovieMatch.Movies;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace MovieMatch.Posts
{
    public class PostDto
    {
        public string Comment { get; set; }
        public int MovieId { get; set; }
        public Guid UserId { get; set; }
        public int Rate { get; set; }
        public MovieDto Movie { get; set; }
        public IdentityUserDto User { get; set; }
        public DateTime CreationTime{ get; set; }

    }
}
