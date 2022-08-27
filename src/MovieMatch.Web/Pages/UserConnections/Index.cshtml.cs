using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieMatch.UserConnections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Content;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace MovieMatch.Web.Pages.UserConnections
{
    public class IndexModel : AbpPageModel
    {

        [BindProperty]
        public UploadFileDto UploadFileDto { get; set; }
        [BindProperty(SupportsGet =true)]
        public string UserName { get; set; }
        public bool Uploaded { get; set; } = false;
        private readonly IUserConnectionAppService _userConnectionService;
        private readonly ICurrentUser _currentUser;
        public string filepath { get; set; }
        private readonly IHostingEnvironment _env;
        private readonly IFileAppService _fileAppService;
        private readonly IUserRepository _userRepository;
       
       public bool isActive { get; set; }
        public string path { get; set; }
        public int FollowersCount { get; set; }
        public Guid Id { get; set; }
        public int FollowingCount { get; set; }
        [BindProperty]
        public IFormFile Photo { get; set; }


       

        public IndexModel()
        {


        }
        public IndexModel(IUserRepository userRepository,IFileAppService fileAppService,IHostingEnvironment env, IUserConnectionAppService userConnectionService, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _fileAppService = fileAppService;
            _env= env;
            _currentUser = currentUser;
            _userConnectionService = userConnectionService;

        }
       
        public async Task OnGetAsync()
        {
            var userquery =await _userRepository.GetQueryableAsync();
            if (UserName == null)
            {
                UserName = _currentUser.UserName;
            }
            var user= userquery.FirstOrDefault(x => x.UserName == UserName);

            isActive = (bool)user.GetProperty("isFollow");
            Id = user.Id;

            
                
            FollowersCount = (await _userConnectionService.GetFollowersCount( UserName));
            FollowingCount =(await _userConnectionService.GetFollowingCount(UserName));

            path =await _userConnectionService.GetPhotoAsync(UserName);
          
        }
       public async Task<IActionResult> OnPostAsync()
       {
         
         
           string uniquefilename = null;
           if(Photo!=null)
           {
               string uploadsfolder = Path.Combine(_env.WebRootPath, "images");
               uniquefilename=Guid.NewGuid().ToString()+ "_" +Photo.FileName;
               string filePath = Path.Combine(uploadsfolder, uniquefilename);
               Photo.CopyTo(new FileStream(filePath, FileMode.Create));
           }
         
        
        
         
          await _userConnectionService.SetPhotoAsync(_currentUser.UserName,uniquefilename);
           filepath = await _userConnectionService.GetPhotoAsync(_currentUser.UserName);
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
