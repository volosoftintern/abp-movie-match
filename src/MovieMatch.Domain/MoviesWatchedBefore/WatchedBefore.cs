using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace MovieMatch.Movies
{
    public class WatchedBefore : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public int MovieId { get; set; }
        private WatchedBefore() { }

        public WatchedBefore(Guid userId,int movieId)
        {
            UserId = userId;
            MovieId = movieId;
        }

        public override object[] GetKeys()
        {
            return new object[] { MovieId, UserId};    
        }
    }
}
