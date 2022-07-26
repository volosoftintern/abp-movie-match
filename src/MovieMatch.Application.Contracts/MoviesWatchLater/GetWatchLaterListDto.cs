using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace MovieMatch.MoviesWatchLater
{
    public class GetWatchLaterListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
