using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MovieMatch.Web.Pages.Files
{
    public class Index : AbpPageModel
    {
        [BindProperty]
        public UploadFileDto UploadFileDto { get; set; }
        private readonly IFileAppService _fileAppService;

        public Index(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        public bool Uploaded { get; set; } = false;
        
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            using(var memoryStream=new MemoryStream())
            {
                await UploadFileDto.File.CopyToAsync(memoryStream);
                await _fileAppService.SaveBlobAsync(
                    new SaveBlobInputDto
                    {
                        Name = UploadFileDto.Name,
                        Content = memoryStream.ToArray()
                    });
            }
            return Page();
        }



    }


  
        public class UploadFileDto
        {
            [Required]
            [Display(Name = "File")]
            public IFormFile File { get; set; }

            [Required]
            [Display(Name = "Filename")]
            public string Name { get; set; }
        }
    




}
