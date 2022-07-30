using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace MovieMatch.UserConnections
{
    public class UserConnectionAppService : ApplicationService, IUserConnectionAppService
    {
        //  private readonly IIdentityUserRepository identityUserRepository;

        private readonly UserConnectionManager _userConnectionManager; 
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly ICurrentUser _currentUser;
        private readonly IUserConnectionRepository _userConnectionRepository;
        

        public UserConnectionAppService(IIdentityUserAppService identityUserAppService, IUserConnectionRepository userConnectionRepository,ICurrentUser currentUser, UserConnectionManager userConnectionManager)
        {
            
           _userConnectionRepository = userConnectionRepository;
            _identityUserAppService = identityUserAppService;
            _currentUser = currentUser;
            _userConnectionManager= userConnectionManager;
        }


        public async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
     
        
           //var users=await _identityUserManager
      //   var asd= ObjectMapper.Map<List<IdentityUser>,List<IdentityUserDto>>((List<IdentityUser>)users);
            var users= await  _identityUserAppService.GetListAsync(input);
           var filteredUsers=users.Items.Where(u => u.Id != _currentUser.Id);
           
            return new PagedResultDto<IdentityUserDto>(filteredUsers.Count(),filteredUsers.ToList());
                           
              //     List<IdentityUserDto> userDtos= ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>((List<IdentityUser>)users);

        //    return new ListResultDto<IdentityUserDto>(userDtos);
            


        }

        public async Task<bool> AddFollowerAsync(Guid id,bool isActive)
        {
            var follower = await _userConnectionManager.CreateAsync(id);
            if(isActive)
            {
                var result = await _userConnectionRepository.InsertAsync(follower);
                //isActive = true;
                return true;
            }
            else
            {
                var result = await _userConnectionRepository.GetAsync((c) => c.FollowerId == _currentUser.Id && c.FollowingId == id);
                if (result != null)
                {
                    await _userConnectionRepository.DeleteAsync(result, true);
                }
            //    isActive = false;
                return false;
            }

            


           // var user= await _identityUserAppService.GetAsync(id);
           // return user;

        }

        //public async Task<bool> RemoveFollowerAsync(Guid id,bool isActive)
        //{


        //    var follower = await _userConnectionManager.CreateAsync(id);


            
            

        //  //  var user = await _identityUserAppService.GetAsync(id);
        //  //  return user;

        //}

        public async Task<PagedResultDto<FollowerDto>> GetFollowersAsync(GetIdentityUsersInput input)
        {


            var res = await _userConnectionRepository.GetListAsync();
          //  var result = res.ToList();
            //        var results = result.GroupBy(
            //p=>p.FollowerId,
            //p => p.FollowingId,
            // (key, g) => new { = key, UserConnections = g.ToList() });
            
            var response= res.Where(n => n.FollowingId==_currentUser.Id).Select(c=>new FollowerDto(c.FollowerId)).ToList();
            return new PagedResultDto<FollowerDto>(response.Count(), response);
            
    }

        public async Task<PagedResultDto<FollowerDto>> GetFollowingAsync(GetIdentityUsersInput input)
        {
            var res = await _userConnectionRepository.GetListAsync();
            var user =await _identityUserAppService.GetListAsync(input);
            //  var result = res.ToList();
            //        var results = result.GroupBy(
            //p=>p.FollowerId,
            //p => p.FollowingId,
            // (key, g) => new { = key, UserConnections = g.ToList() });

            var response= res.Where(n => n.FollowerId==_currentUser.Id).Select(c=>new FollowerDto(c.FollowingId)).ToList();
            var q = (from pd in response
                     join od in user.Items.ToList() on pd.Id equals od.Id
                     select new FollowerDto
                     {

                         name=od.UserName
                     }).ToList();
            return new PagedResultDto<FollowerDto>(q.Count(), q);

        }

    }
}
