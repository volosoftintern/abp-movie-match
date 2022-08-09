using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch.Movies
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfilePath { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime? DeathDay { get; set; }
        public string Biography { get; set; }

        public IReadOnlyList<MovieDto> Movies { get; set; }
    }
}
