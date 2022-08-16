using MovieMatch.Genres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace MovieMatch
{
    public class MovieMatchDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Genre,int> _repository;

        public MovieMatchDataSeederContributor(IRepository<Genre,int> repository)
        {
            _repository = repository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if(await _repository.GetCountAsync() <= 0)
            {
                await _repository.InsertAsync(
                    new Genre(28,"Action")
                );

                await _repository.InsertAsync(
                    new Genre(12, "Adventure")
                );

                await _repository.InsertAsync(
                    new Genre(16, "Animation")
                );
                
                await _repository.InsertAsync(
                    new Genre(35, "Comedy")
                );

                await _repository.InsertAsync(
                    new Genre(80, "Crime")
                );

                await _repository.InsertAsync(
                    new Genre(99, "Documentary")
                );

                await _repository.InsertAsync(
                    new Genre(18, "Drama")
                );

                await _repository.InsertAsync(
                    new Genre(10751, "Family")
                );

                await _repository.InsertAsync(
                    new Genre(36, "History")
                );
                await _repository.InsertAsync(
                    new Genre(27, "Horror")
                );
                await _repository.InsertAsync(
                    new Genre(10402, "Music")
                );
                await _repository.InsertAsync(
                    new Genre(9648, "Mystery")
                );

                await _repository.InsertAsync(
                    new Genre(10749, "Romance")
                );

                await _repository.InsertAsync(
                    new Genre(878, "Science Fiction")
                );

                await _repository.InsertAsync(
                    new Genre(10770, "TV Movie")
                );

                await _repository.InsertAsync(
                    new Genre(53, "Thriller")
                );
                await _repository.InsertAsync(
                    new Genre(10752, "War")
                );

                await _repository.InsertAsync(
                    new Genre(37, "Western")
                );
            }
        }
    }
}
