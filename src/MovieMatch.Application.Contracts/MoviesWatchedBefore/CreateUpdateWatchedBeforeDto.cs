using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch.MoviesWatchedBefore
{
    public class CreateUpdateWatchedBeforeDto
    {
        public int MovieId { get; set; }
        public Guid UserId { get; set; }
    }
}
