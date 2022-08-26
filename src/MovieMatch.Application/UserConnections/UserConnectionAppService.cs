using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieMatch.Movies;
using MovieMatch.Rating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using Volo.Abp.Validation;
using Volo.CmsKit.Ratings;
using static Volo.Abp.Identity.IdentityPermissions;

namespace MovieMatch.UserConnections
{
    public class UserConnectionAppService : ApplicationService, IUserConnectionAppService
    {
        private readonly UserConnectionManager _userConnectionManager;
        private readonly ICurrentUser _currentUser;
        private readonly IUserConnectionRepository _userConnectionRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IBlobContainer<MyFileContainer> _organizationBlobContainer;
        private readonly IHostingEnvironment _env;
        private readonly IUserRepository _userRepository;
        private readonly IRatingPublicAppService _ratingPublicAppService;
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieAppService _movieAppService;
        private readonly IMovieRepository _movieRepository;



        public UserConnectionAppService(IUserRepository userRepository,
            IHostingEnvironment env,
            IBlobContainer<MyFileContainer> organizationBlobContainer,
            IIdentityUserRepository identityUserRepository,
            IUserConnectionRepository userConnectionRepository,
            ICurrentUser currentUser,
            UserConnectionManager userConnectionManager,
            IRatingPublicAppService ratingPublicAppService,
            IRatingRepository ratingRepository,
            IMovieAppService movieAppService,
            IMovieRepository movieRepository
            )
        {
            _env = env;
            _userRepository = userRepository;
            _userConnectionRepository = userConnectionRepository;
            _identityUserRepository = identityUserRepository;
            _currentUser = currentUser;
            _userConnectionManager = userConnectionManager;
            _organizationBlobContainer = organizationBlobContainer;
            _ratingPublicAppService = ratingPublicAppService;
            _ratingRepository= ratingRepository;
            _movieAppService = movieAppService;
            _movieRepository= movieRepository;
        }




        
        public async Task<PagedResultDto<FollowerDto>> GetListAsync(GetIdentityUsersInput input)

        {

            var filteredUsers = await _userConnectionRepository.GetUsersListAsync(input.SkipCount,input.MaxResultCount,input.Filter);

            var userlist =await _identityUserRepository.GetListAsync();



            var filteredUsersList = filteredUsers.ToList();

            var followerdto= filteredUsersList.Select(x => new FollowerDto
             {
                 Id = x.Id,
                 isFollow = x.GetProperty<bool>("isFollow"),
                 Name = x.UserName,
                 Path = x.GetProperty<string>("Photo"),
             }).ToList();
            
    



            foreach (var item in filteredUsersList)
            {
                await SetisFollowAsync(item.UserName, false);
            }

            var currentUserFollowing = await GetCurrentUserFollowingAsync();
            foreach (var item in currentUserFollowing)
            {
                foreach (var user in filteredUsersList)
                {
                    if (user.Id == item)
                    {

                        await SetisFollowAsync(user.UserName, true);

                    }


                }
            }

            return new PagedResultDto<FollowerDto>(((userlist.Count)-1), followerdto);
                           
         
    

        }
        public async Task<List<Guid>> GetCurrentUserFollowingAsync()
        {
            var connections = await _userConnectionRepository.GetListAsync();
            var response = connections.Where(n => n.FollowerId == _currentUser.Id).Select(c => (c.FollowingId)).ToList();
            return response;

        }

        public async Task FollowAsync(Guid id)
        {
            var connection = _userConnectionManager.Create(id);

            await _userConnectionRepository.InsertAsync(connection, true);
            

            var finduser = await _identityUserRepository.GetAsync(id);
            await SetisFollowAsync(finduser.UserName, true);

        }
        public async Task UnFollowAsync(Guid id)
        {

            var connection = await _userConnectionRepository.GetAsync((c) => c.FollowerId == _currentUser.Id && c.FollowingId == id);

            if (connection != null)
            {

                var finduser=  await _identityUserRepository.GetAsync(connection.FollowingId);
                await SetisFollowAsync(finduser.UserName, false);
                await _userConnectionRepository.DeleteAsync(connection, true);
            }

        }

        public async Task<PagedResultDto<FollowerDto>> GetFollowersAsync(GetUsersFollowInfo input)
        {
            var user = await _userRepository.GetAsync(x => x.UserName == input.username);
            var connections = await _userConnectionRepository.GetQueryableAsync();         
            
            
            var users = await _identityUserRepository.GetListAsync();

            var followers = connections.Where(n => n.FollowingId == user.Id).Select(c =>(c.FollowerId)).ToList();

            var q = (from pd in followers
                     join od in users.ToList() on pd equals od.Id
                     select new FollowerDto
                     {
                         Id = pd,
                         Name = od.UserName,
                         Path = od.GetProperty<string>("Photo"),
                         isFollow = od.GetProperty<bool>("isFollow")

                     }).WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.Name.Contains(input.Filter)).ToList();

            return new PagedResultDto<FollowerDto>(q.Count(), q);

        }
        public async Task<int> GetFollowersCount(string userName)
        {

            var user = await _userRepository.GetAsync(x => x.UserName == userName);
            var count= (await _userConnectionRepository.GetQueryableAsync())
            .Where(x=>x.FollowingId==user.Id).Count();
            return count;
        }
        public async Task<int> GetFollowingCount(string userName)
        {
            var user = await _userRepository.GetAsync(x => x.UserName == userName);
            var count= (await _userConnectionRepository.GetQueryableAsync())
            .Where(x=>x.FollowerId==user.Id).Count();
            return count;

        }
        public async Task<PagedResultDto<FollowerDto>> GetFollowingAsync(GetUsersFollowInfo input)
        {
            var user = await _userRepository.GetAsync(x => x.UserName == input.username);
            var res = await _userConnectionRepository.GetListAsync();


            var users = await _identityUserRepository.GetListAsync();


            var response = res.Where(n => n.FollowerId == user.Id).Select(c => (c.FollowingId)).ToList();
            var q = (from pd in response
                     join od in users.ToList() on pd equals od.Id
                     select new FollowerDto
                     {
                         Id = pd,
                         Name = od.UserName,

                         Path = od.GetProperty<string>("Photo"),
                         isFollow = od.GetProperty<bool>("isFollow"),


                     }).WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.Name.Contains(input.Filter)).ToList();
            return new PagedResultDto<FollowerDto>(q.Count(), q);

        }
        

        public async Task SetPhotoAsync(string userName, string name)
        {
            var user = await _userRepository.GetAsync(u => u.UserName == userName);
            user.SetProperty(ProfilePictureConsts.PhotoProperty, name); 
            await _userRepository.UpdateAsync(user);
        }

        public async Task<string> GetPhotoAsync(string userName)
        {
            var user = await _userRepository.GetAsync(u => u.UserName == userName);
            return user.GetProperty<string>(ProfilePictureConsts.PhotoProperty); 
        }
        public async Task SetisFollowAsync(string userName, bool isFollow)
        {
            var user = await _userRepository.GetAsync(u => u.UserName == userName);
            user.SetProperty(IdentityUserConsts.IsFollowProperty, isFollow); //Using the new extension property
            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> GetisFollowAsync(string userName)
        {
            var user = await _userRepository.GetAsync(u => u.UserName == userName);
            return user.GetProperty<bool>(IdentityUserConsts.IsFollowProperty); //Using the new extension property
        }

        public async Task<UserInformationDto> GetUserInfoAsync(string username)
        {
            var user = await _userRepository.GetAsync(x => x.UserName == username);
            int followerCount = await GetFollowersCount(username);
            int followingCount = await GetFollowingCount(username);
            string path = await GetPhotoAsync(username);
            return new UserInformationDto
            {
                FollowersCount = followerCount,
                FollowingCount = followingCount,
                Path = path,
                Username = username
            };
        }

        public async Task<List<IdentityUserDto>> GetRecommendedUsersList(GetIdentityUsersInput input)
        {
            List<IdentityUserDto> userDto = new List<IdentityUserDto>();
            List<double> similarityList = new List<double>();
            int k=0, i=0, j=0, index;
            var movies =await _movieRepository.GetListAsync();
            var users= await _identityUserRepository.GetListAsync();
            int[,] A = new int[users.Count, movies.Count];
            index = users.ToList().FindIndex(x=>x.UserName==CurrentUser.UserName);
            foreach (var user in users)
            {
                foreach (var movie in movies.OrderByDescending(x=>x.Id))
                {
                    var userRating = await _ratingRepository.GetCurrentUserRatingAsync("Movie", movie.Id.ToString(), (Guid)user.Id);
                    if (userRating == null)
                    {
                        A[i,j] = 0;
                    }
                    else
                    {
                        A[i,j] = userRating.StarCount;
                    }
                    j++;
                }
                j = 0;
                i++;
            }

            similarityList = GetCosineSimilarity(A, index, users.Count, movies.Count);
            foreach (var item in users)
            {
                if(k<users.Count-1)
                {
                    if(similarityList.Count!=0 && similarityList[k] > 0.5)
                    {
                        var identityUserDto=ObjectMapper.Map<IdentityUser,IdentityUserDto>(item);
                        userDto.Add(identityUserDto);
                    }
                    else if (similarityList.Count==0)
                    {
                        return null;
                    }
                    k++;
                }
            }
            return userDto;
        }
        public List<double> GetCosineSimilarity(int[,] A,int index, int userCount,int movieCount)
        {
            List<double> similarityList =new List<double>();
            double dot;
            double mag1;
            double mag2;
            int i = 0;
            while(i< userCount)
            {
                if (index!=i)
                {
                    dot = 0.0d;
                    mag1 = 0.0d;
                    mag2 = 0.0d;
                    for (int j = 0; j < movieCount; j++)
                    {
                        dot += A[index, j] * A[i, j];
                        mag1 += Math.Pow(A[index, j], 2);
                        mag2 += Math.Pow(A[i, j], 2);
                    }
                    if(mag1!=0 && mag2!=0)
                    {
                        similarityList.Add(dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2)));
                    }
                    
                }
                i++;
            }
            return similarityList;
        }
 



    }
}
