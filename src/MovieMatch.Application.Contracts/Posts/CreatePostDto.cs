using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch.Posts
{
    public class CreatePostDto
    {
        public int MovieId { get; set; }
        public Guid UserId { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public string MovieTitle { get; set; }

    }
}
