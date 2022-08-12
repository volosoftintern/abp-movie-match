using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace MovieMatch.Controllers
{
    public class FileController : AbpController
    {
        private readonly IFileAppService _fileAppService;

        public FileController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }
        [HttpGet]
        [Route("download/{fileName}")]
        public async Task<IActionResult> DownloadAsync(string fileName)
        {
            var fileDto = await _fileAppService.GetBlobAsync(new GetBlobRequestDto { Name = fileName });
            return File(fileDto.Content, "application/octet-stream", fileDto.Name);


        }

        [HttpPost]
        public async Task<string> UploadAsync(SaveBlobInputDto input)
        {
            input.Name = Guid.NewGuid().ToString()+input.Content.FileName;
            await _fileAppService.SaveBlobAsync(input);

            return input.Name;

        }
    }
}
