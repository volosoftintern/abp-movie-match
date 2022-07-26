using MovieMatch.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MovieMatch.MoviesWatchLater
{
    public interface IWatchLaterRepository : IRepository<WatchLater, Guid>
    {
        Task<WatchLater> FindByIdAsync(int id);
    }
}
