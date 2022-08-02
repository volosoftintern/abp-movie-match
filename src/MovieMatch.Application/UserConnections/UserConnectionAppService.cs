using Microsoft.AspNetCore.Mvc;
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
using Volo.Abp.Validation;

namespace MovieMatch.UserConnections
{
    public class UserConnectionAppService : ApplicationService, IUserConnectionAppService
    {
        //  private readonly IIdentityUserRepository identityUserRepository;
        
        private readonly UserConnectionManager _userConnectionManager; 
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly ICurrentUser _currentUser;
        private readonly IUserConnectionRepository _userConnectionRepository;

       

        public UserConnectionAppService( IIdentityUserAppService identityUserAppService, IUserConnectionRepository userConnectionRepository,ICurrentUser currentUser, UserConnectionManager userConnectionManager)
        {
              
            _userConnectionRepository = userConnectionRepository;
            _identityUserAppService = identityUserAppService;
            _currentUser = currentUser;
            _userConnectionManager= userConnectionManager;
        }

       

        [DisableValidation]
        public async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
     
        
       var res = await _userConnectionRepository.GetListAsync();
            var users= await  _identityUserAppService.GetListAsync(input);
            var filteredUsers = users.Items.Where(u => u.Id != _currentUser.Id);



            //foreach (var item in filteredUsers)
            //{
            //    var result = await _userConnectionRepository.InsertAsync(follower);
            //    //isActive = true;
            //    return true;
            //    item.IsActive = false;
            //}

            var currentUserFollowing =await GetFirstAsync();
            foreach (var item in currentUserFollowing)
            {
                foreach (var user in filteredUsers)
                {
                    if(user.Id==item)
                    {
                        res.Where(x=>x.FollowingId==user.Id).FirstOrDefault().isFollowed=true;
                        
                            user.IsActive = true;

                    }
                    
     

                }
            }
           
            return new PagedResultDto<IdentityUserDto>(filteredUsers.Count(),filteredUsers.ToList());
                           
         
    
        }
        public async Task<List<Guid>> GetFirstAsync()
        {
            var res = await _userConnectionRepository.GetListAsync();
            var response =res.Where(n => n.FollowerId == _currentUser.Id).Select(c => (c.FollowingId)).ToList();
            return response;

        }
        [DisableValidation]
        public async Task FollowAsync(Guid id,bool isActive)
        {
            var follower = await _userConnectionManager.CreateAsync(id,true);


             await _userConnectionRepository.InsertAsync(follower,true);
             var res= await _userConnectionRepository.GetListAsync();
          
            res.Where(x => x.FollowerId == _currentUser.Id && x.FollowingId == id).FirstOrDefault().isFollowed = true;
           var finduser= await _identityUserAppService.GetAsync(id);
            finduser.IsActive = !isActive;


        }


       [DisableValidation]
        public async Task UnFollowAsync(Guid id,bool isActive) { 
                 


            var result = await _userConnectionRepository.GetAsync((c) => c.FollowerId == _currentUser.Id && c.FollowingId == id);
            result.isFollowed = false;

            if (result != null)
                          {
             var finduser=  await _identityUserAppService.GetAsync(result.FollowingId);
                finduser.IsActive = !isActive;
                                await _userConnectionRepository.DeleteAsync(result, true);
                
                
                      }

}


        public async Task<PagedResultDto<FollowerDto>> GetFollowersAsync(GetIdentityUsersInput input)
        {


            var res = await _userConnectionRepository.GetListAsync();
            var user = await _identityUserAppService.GetListAsync(input);


            var response = res.Where(n => n.FollowingId == _currentUser.Id).Select(c => new FollowerDto(c.FollowerId)).ToList();
            var q = (from pd in response
                     join od in user.Items.ToList() on pd.Id equals od.Id 
                     select new FollowerDto
                     {
                         Id = pd.Id,
                         name = od.UserName
                     }).ToList();
            return new PagedResultDto<FollowerDto>(q.Count, q);



           
            
    }

        public async Task<PagedResultDto<FollowerDto>> GetFollowingAsync(GetIdentityUsersInput input)
        {
            var res = await _userConnectionRepository.GetListAsync();
            var user =await _identityUserAppService.GetListAsync(input);
           

            var response= res.Where(n => n.FollowerId==_currentUser.Id).Select(c=>new FollowerDto(c.FollowingId)).ToList();
            var q = (from pd in response
                     join od in user.Items.ToList() on pd.Id equals od.Id
                     select new FollowerDto
                     {
                         Id=pd.Id,
                         name=od.UserName
                     }).ToList();
            return new PagedResultDto<FollowerDto>(q.Count, q);

        }

    }
}
