using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MovieMatch.Movies
{
    public class WatchLater : Entity<Guid>
    {
        public int MovieId { get; set; }
        public Guid UserId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { MovieId, UserId };
        }
    }
}
