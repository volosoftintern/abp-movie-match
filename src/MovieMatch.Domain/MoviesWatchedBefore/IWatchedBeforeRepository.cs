using MovieMatch.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MovieMatch.MoviesWatchedBefore
{
    public interface IWatchedBeforeRepository : IRepository<WatchedBefore, Guid>
    {
        Task<WatchedBefore> FindByIdAsync(int id);
    }
}
