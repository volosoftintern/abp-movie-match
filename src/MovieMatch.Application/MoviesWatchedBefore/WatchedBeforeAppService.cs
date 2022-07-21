using MovieMatch.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MovieMatch.MoviesWatchedBefore
{
    public class WatchedBeforeAppService :
        CrudAppService<
            WatchedBefore,
            WatchedBeforeDto, //Used to show WatchedBefores
            Guid, //Primary key of the WatchedBefore entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateWatchedBeforeDto>, //Used to create/update a WatchedBefore
        IWatchedBeforeAppService //implement the IWatchedBeforeAppService
    {
        public WatchedBeforeAppService(IRepository<WatchedBefore, Guid> repository)
            : base(repository)
        {

        }
    }
}
