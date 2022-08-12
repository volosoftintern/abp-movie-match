using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace MovieMatch
{
    [Authorize]
    public class ProfilePictureAppService : MovieMatchAppService
    {
        private readonly IBlobContainer<MyFileContainer> _blobContainer;
        private readonly IRepository<IdentityUser, Guid> _repository;
        private readonly CurrentUser _currentUser;


        public ProfilePictureAppService(IBlobContainer<MyFileContainer> blobContainer, IRepository<IdentityUser,Guid> repository,CurrentUser currentUser)
        {
            _currentUser = currentUser;
            _blobContainer = blobContainer;
            _repository = repository;
        }

        public virtual async Task<Guid> UploadAsync(IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream).ConfigureAwait(false);
            if (CurrentUser.Id == null)
            {
                return Guid.Empty;
            }

            var user = await _repository.GetAsync(_currentUser.Id.Value).ConfigureAwait(false);
            var pictureId = user.GetProperty<Guid>(ProfilePictureConsts.ProfilePictureId);

            if (pictureId == Guid.Empty)
            {
                pictureId = Guid.NewGuid();
            }
            var id = pictureId.ToString();
            if (await _blobContainer.ExistsAsync(id).ConfigureAwait(false))
            {
                await _blobContainer.DeleteAsync(id).ConfigureAwait(false);
            }
            await _blobContainer.SaveAsync(id, memoryStream.ToArray()).ConfigureAwait(false);
            user.SetProperty(ProfilePictureConsts.ProfilePictureId, pictureId);
            await _repository.UpdateAsync(user).ConfigureAwait(false);
            return pictureId;
        }

        public async Task<FileResult> GetAsync()
        {
            if (_currentUser.Id == null)
            {
                throw new FileNotFoundException();
            }

            var user = await _repository.GetAsync(CurrentUser.Id.Value).ConfigureAwait(false);
            var pictureId = user.GetProperty<Guid>(ProfilePictureConsts.ProfilePictureId);
            if (pictureId == default)
            {
                throw new FileNotFoundException();
            }

            var profilePicture = await _blobContainer.GetAllBytesOrNullAsync(pictureId.ToString()).ConfigureAwait(false);
            return new FileContentResult(profilePicture, "image/jpeg");

        }
    }
}
