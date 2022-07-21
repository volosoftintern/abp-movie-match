using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch.Search
{
    public class SearchResponseDto<T> where T : class
    {

        public SearchResponseDto(int page, int totalResults, int totalPages, IReadOnlyList<T> results)
        {
            Page = page;
            TotalResults = totalResults;
            TotalPages = totalPages;
            Results = results;
        }

        public int Page { get; set; }
        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
        public IReadOnlyList<T> Results { get; set; }
    }
}
