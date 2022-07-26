using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace MovieMatch.MoviesWatchLater
{
    public class WatchLaterDto:EntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public int MovieId { get; set; }
    }
}
