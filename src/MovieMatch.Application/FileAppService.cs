using MovieMatch.UserConnections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Data;
using Volo.Abp.Users;

namespace MovieMatch
{
    public class FileAppService : ApplicationService, IFileAppService
    {
        private readonly IBlobContainer<MyFileContainer> _fileContainer;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public FileAppService(ICurrentUser currentUser,IUserRepository userRepository,IBlobContainer<MyFileContainer> fileContainer)
        {
            _currentUser = currentUser;
            _userRepository = userRepository;
            _fileContainer = fileContainer;
        }

        public async Task<BlobDto> GetBlobAsync(GetBlobRequestDto input)
        {
            var blob = await _fileContainer.GetAllBytesAsync(input.Name);
            return new BlobDto{ Name = input.Name,Content = blob};
        }

        public async Task SaveBlobAsync(SaveBlobInputDto input)
        {
            await _fileContainer.SaveAsync(input.Name, input.Content.GetStream(), true);
            var user = await _userRepository.GetAsync(u => u.UserName == _currentUser.UserName);
            user.SetProperty(ProfilePictureConsts.PhotoProperty, input.Name); //Using the new extension property
            await _userRepository.UpdateAsync(user);


            //user.poster=inut.
            
        }
    }
}
