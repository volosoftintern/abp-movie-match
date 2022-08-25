using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace MovieMatch.UserConnections
{
    public class UserConnectionAppService : ApplicationService, IUserConnectionAppService
    {
        //  private readonly IIdentityUserRepository identityUserRepository;

        private readonly UserConnectionManager _userConnectionManager;
        private readonly ICurrentUser _currentUser;
        private readonly IUserConnectionRepository _userConnectionRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IBlobContainer<MyFileContainer> _organizationBlobContainer;
        private readonly IHostingEnvironment _env;
        private readonly IUserRepository _userRepository;



        public UserConnectionAppService(IUserRepository userRepository, IHostingEnvironment env, IBlobContainer<MyFileContainer> organizationBlobContainer, IIdentityUserRepository identityUserRepository, IUserConnectionRepository userConnectionRepository, ICurrentUser currentUser, UserConnectionManager userConnectionManager)
        {
            _env = env;
            _userRepository = userRepository;
            _userConnectionRepository = userConnectionRepository;
            _identityUserRepository = identityUserRepository;
            _currentUser = currentUser;
            _userConnectionManager = userConnectionManager;
            _organizationBlobContainer = organizationBlobContainer;
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
            var count = await GetFollowersAsync(new GetUsersFollowInfo() { username = userName });
          
            return count.Items.Count;
        }
        public async Task<int> GetFollowingCount(string userName)
        {
            var count = await GetFollowingAsync(new GetUsersFollowInfo() { username = userName });
            
            return count.Items.Count;
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
      


    }
}
