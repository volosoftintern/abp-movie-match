using MovieMatch.EntityFrameworkCore;
using MovieMatch.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MovieMatch.Messages
{
    public class EfCoreMessageRepository : EfCoreRepository<MovieMatchDbContext, Message, Guid>, IMessageRepository
    {
        public EfCoreMessageRepository(IDbContextProvider<MovieMatchDbContext> dbContextProvider) : base(dbContextProvider)
        {
            
        }
    }
}
