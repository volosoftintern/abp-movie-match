using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MovieMatch.Movies
{
    public class MovieGenre:Entity
    {
        public int MovieDetailId { get; protected set; }
        public int GenreId { get; protected set; }

        private MovieGenre()
        {

        }

        public MovieGenre(int movieId, int genreId)
        {
            MovieDetailId = movieId;
            GenreId = genreId;
        }

        public override object[] GetKeys()
        {
            return new object[] {MovieDetailId,GenreId};
        }
    }
}
