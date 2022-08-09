using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch.Movies
{
    public class DirectorDto
    {
        public DirectorDto(PersonDto personDto,IReadOnlyList<MovieDto> movies)
        {
            Person = personDto;
            Movies = movies;
        }
        

        public PersonDto Person { get; set; }
        public IReadOnlyList<MovieDto> Movies { get; set; }

    }
}
