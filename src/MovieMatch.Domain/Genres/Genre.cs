using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MovieMatch.Genres
{
    public class Genre:Entity<int>
    {
        public string Name { get; set; }
        private Genre()
        {

        }

        public Genre(int id,string name )
        {
            Id = id;
            Name = name;
        }
    }
}
