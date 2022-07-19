using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MovieMatch.Movies
{
    public class WatchLater : Entity
    {
        public Guid UserId { get; set; }
        public int MovieId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { UserId, MovieId };
        }
    }
}
