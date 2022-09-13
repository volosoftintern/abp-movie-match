using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace MovieMatch.Movies
{
    public class MoviePagedAndSortedResultRequestDto: PagedAndSortedResultRequestDto
    {
        public string username { get; set; }
    }
}
