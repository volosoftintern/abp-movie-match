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




        public async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {


            ;
            var filteredUsersList = await _userConnectionRepository.GetUsersListAsync(input.SkipCount, input.MaxResultCount, input.Filter);
            var list = await _identityUserRepository.GetListAsync();



            var filteredUsers = filteredUsersList.ToList();
            var filteredUsersCount = filteredUsers;

            var userDtos = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(filteredUsers);




            foreach (var item in filteredUsers)
            {
                await SetisFollowAsync(item.UserName, false);
            }

            var currentUserFollowing = await GetFirstAsync();
            foreach (var item in currentUserFollowing)
            {
                foreach (var user in filteredUsers)
                {
                    if (user.Id == item)
                    {

                        await SetisFollowAsync(user.UserName, true);

                    }


                }
            }
            return new PagedResultDto<IdentityUserDto>(((list.Count) - 1), userDtos);



        }
        public async Task<List<Guid>> GetFirstAsync()
        {
            var res = await _userConnectionRepository.GetListAsync();
            var response = res.Where(n => n.FollowerId == _currentUser.Id).Select(c => (c.FollowingId)).ToList();
            return response;

        }

        public async Task FollowAsync(Guid id, bool isActive)
        {
            var follower = _userConnectionManager.Create(id);

            await _userConnectionRepository.InsertAsync(follower, true);
            var res = await _userConnectionRepository.GetListAsync();

            var finduser = await _identityUserRepository.GetAsync(id);
            await SetisFollowAsync(finduser.UserName, true);

        }



        public async Task UnFollowAsync(Guid id, bool isActive)
        {



            var result = await _userConnectionRepository.GetAsync((c) => c.FollowerId == _currentUser.Id && c.FollowingId == id);

            if (result != null)
            {
                var finduser = await _identityUserRepository.GetAsync(result.FollowingId);
                await SetisFollowAsync(finduser.UserName, false);
                await _userConnectionRepository.DeleteAsync(result, true);


            }

        }


        public async Task<PagedResultDto<FollowerDto>> GetFollowersAsync(GetUsersFollowInfo input)
        {
            var userr = await _userRepository.GetAsync(x => x.UserName == input.username);
            var res = await _userConnectionRepository.GetListAsync();


            var user = await _identityUserRepository.GetListAsync();

            var users = await _identityUserRepository.GetListAsync();


            var response = res.Where(n => n.FollowingId == userr.Id).Select(c => (c.FollowerId)).ToList();
            var q = (from pd in response
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
            int a = count.Items.Count;
            return count.Items.Count;
        }
        public async Task<int> GetFollowingCount(string userName)
        {
            var count = await GetFollowingAsync(new GetUsersFollowInfo() { username = userName });
            int a = count.Items.Count;
            return count.Items.Count;
        }
        public async Task<PagedResultDto<FollowerDto>> GetFollowingAsync(GetUsersFollowInfo input)
        {
            var userr = await _userRepository.GetAsync(x => x.UserName == input.username);
            var res = await _userConnectionRepository.GetListAsync();

            var user = await _identityUserRepository.GetListAsync();

            var users = await _identityUserRepository.GetListAsync();


            var response = res.Where(n => n.FollowerId == userr.Id).Select(c => (c.FollowingId)).ToList();
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
        public async Task UploadAsync(IFormFile file)
        {
            var dir = _env.ContentRootPath;
            using (var fileStream = new FileStream(Path.Combine(dir, file.Name), FileMode.Open, FileAccess.Read))
            {
                file.CopyTo(fileStream);
            }
        }

        public async Task SetPhotoAsync(string userName, string name)
        {
            var user = await _userRepository.GetAsync(u => u.UserName == userName);
            user.SetProperty(ProfilePictureConsts.PhotoProperty, name); //Using the new extension property
            await _userRepository.UpdateAsync(user);
        }

        public async Task<string> GetPhotoAsync(string userName)
        {
            var user = await _userRepository.GetAsync(u => u.UserName == userName);
            return user.GetProperty<string>(ProfilePictureConsts.PhotoProperty); //Using the new extension property
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
