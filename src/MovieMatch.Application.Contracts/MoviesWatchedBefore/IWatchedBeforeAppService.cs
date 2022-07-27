using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MovieMatch.MoviesWatchedBefore
{
    public interface IWatchedBeforeAppService :
        ICrudAppService< //Defines CRUD methods
            WatchedBeforeDto, //Used to show WatchedBefore
            Guid, //Primary key of the WatchedBefore entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateWatchedBeforeDto> //Used to create/update a WatchedBefore
    {
        Task<int> GetCountAsync(Guid id);
    }
}
