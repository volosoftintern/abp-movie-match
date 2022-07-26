using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch.MoviesWatchLater
{
    public class CreateUpdateWatchLaterDto
    {
        public int MovieId { get; set; }
        public Guid UserId { get; set; }
    }
}
