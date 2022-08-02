using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;

namespace MovieMatch
{
    public class FileAppService : ApplicationService, IFileAppService
    {
        private readonly IBlobContainer<MyFileContainer> _fileContainer;

        public FileAppService(IBlobContainer<MyFileContainer> fileContainer)
        {
            _fileContainer = fileContainer;
        }

        public async Task<BlobDto> GetBlobAsync(GetBlobRequestDto input)
        {
            var blob = await _fileContainer.GetAllBytesAsync(input.Name); return new BlobDto{ Name = input.Name,Content = blob};
        }

        public async Task SaveBlobAsync(SaveBlobInputDto input)
        {
            await _fileContainer.SaveAsync(input.Name, input.Content, true);
        }
    }
}
