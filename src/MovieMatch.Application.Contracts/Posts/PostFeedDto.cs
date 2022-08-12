using System;

namespace MovieMatch.Posts
{
    public class PostFeedDto
    {
        public Guid UserId { get; set; }
        public int MaxCount { get; set; }
        public int SkipCount { get; set; }

    }
}