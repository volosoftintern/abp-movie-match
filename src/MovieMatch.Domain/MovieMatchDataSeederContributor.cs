using MovieMatch.Genres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace MovieMatch
{
    public class MovieMatchDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Genre, int> _repository;
        private readonly IdentityUserManager _userManager;
        private readonly IGuidGenerator _guidGenerator;

        public MovieMatchDataSeederContributor(IRepository<Genre, int> repository, IdentityUserManager userManager, IGuidGenerator guidGenerator)
        {
            _repository = repository;
            _userManager = userManager;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _repository.GetCountAsync() <= 0)
            {
                await _repository.InsertAsync(
                    new Genre(28, "Action")
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
                    new Genre(14, "Fantasy")
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

            var user = new IdentityUser(_guidGenerator.Create(), "baris", "baris@abp.io");

            user.SetProperty(ProfilePictureConsts.PhotoProperty, ProfilePictureConsts.DefaultPhotoPath);
            

            await _userManager.CreateAsync(user, "1q2w3E*");

            user = new IdentityUser(_guidGenerator.Create(), "kutay", "kutay@abp.io");

            user.SetProperty(ProfilePictureConsts.PhotoProperty, ProfilePictureConsts.DefaultPhotoPath);

            await _userManager.CreateAsync(user, "1q2w3E*");

            user = new IdentityUser(_guidGenerator.Create(), "cagatay", "cagatay@abp.io");

            user.SetProperty(ProfilePictureConsts.PhotoProperty, ProfilePictureConsts.DefaultPhotoPath);
            await _userManager.CreateAsync(user, "1q2w3E*");

            user = new IdentityUser(_guidGenerator.Create(), "ali", "ali@abp.io");
            user.SetProperty(ProfilePictureConsts.PhotoProperty, ProfilePictureConsts.DefaultPhotoPath);
            await _userManager.CreateAsync(user, "1q2w3E*");

            user = new IdentityUser(_guidGenerator.Create(), "veli", "veli@abp.io");
            user.SetProperty(ProfilePictureConsts.PhotoProperty, ProfilePictureConsts.DefaultPhotoPath);


            await _userManager.CreateAsync(user, "1q2w3E*");
        }
    }
}
