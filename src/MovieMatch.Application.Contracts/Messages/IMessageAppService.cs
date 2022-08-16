using MovieMatch.MoviesWatchLater;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MovieMatch.Messages
{
    public interface IMessageAppService :
    ICrudAppService< //Defines CRUD methods
        MessageDto, //Used to show WatchLater
        Guid, //Primary key of the WatchLater entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateMessageDto> //Used to create/update a WatchLater
    {
    }
}
