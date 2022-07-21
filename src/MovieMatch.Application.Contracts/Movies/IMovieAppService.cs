using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MovieMatch.Movies
{
    public interface IMovieAppService:IApplicationService
    {
        Task<MovieDetailDto> GetAsync(int id);
    }
}
