using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace MovieMatch.MoviesWatchedBefore
{
    public class WatchedBeforeDto: EntityDto<Guid>
    {
        public int MovieId { get; set; }
        public Guid UserId { get; set; }
    }
}
