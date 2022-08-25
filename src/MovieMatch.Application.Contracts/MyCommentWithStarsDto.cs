using MovieMatch.Comments;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch
{
    public class MyCommentWithStarsDto:CommentWithStarsDto
    {
        public string path { get; set; }
    }
}
