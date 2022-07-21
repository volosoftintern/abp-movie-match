using MovieMatch.Movies;
using MovieMatch.MoviesWatchLater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MovieMatch.MoviesWatchLater
{
    public class WatchLaterAppService :
        CrudAppService<
            WatchLater,
            WatchLaterDto, //Used to show WatchLaters
            Guid, //Primary key of the WatchLater entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateWatchLaterDto>, //Used to create/update a WatchLater
        IWatchLaterAppService //implement the IWatchLaterAppService
    {
        public WatchLaterAppService(IRepository<WatchLater,Guid> repository)
            : base(repository)
        {

        }
    }
}
