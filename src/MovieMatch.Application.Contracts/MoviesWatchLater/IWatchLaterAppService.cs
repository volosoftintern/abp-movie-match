using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MovieMatch.MoviesWatchLater
{
    public interface IWatchLaterAppService :
        ICrudAppService< //Defines CRUD methods
            WatchLaterDto, //Used to show WatchLater
            Guid, //Primary key of the WatchLater entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateWatchLaterDto> //Used to create/update a WatchLater
    {
        int GetCount(Guid id);
    }
}
