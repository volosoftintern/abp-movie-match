using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MovieMatch.Movies
{
    public class MoviePerson:Entity
    {
        public int MovieDetailId { get; protected set; }
        public int PersonId { get; protected set; }

        private MoviePerson()
        {

        }

        public MoviePerson(int movieId, int personId)
        {
            MovieDetailId = movieId;
            PersonId = personId;
        }

        public override object[] GetKeys()
        {
            return new object[] {MovieDetailId,PersonId};
        }
    }
}
