using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace MovieMatch.Posts
{
    public class PostDto:EntityDto<int>
    {
        public string Comment { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public int Rate { get; set; }

    }
}
