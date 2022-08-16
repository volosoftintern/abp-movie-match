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
        //  public OrganizationViewModel Organization { get; set; }
        private readonly IUserConnectionAppService _userConnectionService;
        private readonly ICurrentUser _currentUser;
        public string filepath { get; set; }
        private readonly IHostingEnvironment _env;
        private readonly IFileAppService _fileAppService;
       
       
        public string path { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        [BindProperty]
        public IFormFile Photo { get; set; }


        //    private readonly CurrentUser _currentUser;
        //    public IdentityUser user;

        public IndexModel()
        {


        }
        public IndexModel(IFileAppService fileAppService,IHostingEnvironment env, IUserConnectionAppService userConnectionService, ICurrentUser currentUser)
        {
            _fileAppService= fileAppService;
            _env= env;
            _currentUser = currentUser;
            _userConnectionService = userConnectionService;

        }
       

        public async Task OnGetAsync(string username)
        {

            if (UserName == null)
                UserName = _currentUser.UserName;
            FollowersCount = (await _userConnectionService.GetFollowersAsync(new GetUsersFollowInfo { username = UserName })).Items.Count;
           // FollowersCount = (await _userConnectionService.GetFollowersAsync(new GetIdentityUsersInput(), UserName)).Items.Count;
            FollowingCount =(await _userConnectionService.GetFollowingAsync(new GetUsersFollowInfo { username=UserName})).Items.Count;

           
           
          
  
            
            path =await _userConnectionService.GetPhotoAsync(UserName);
          //  Organization = new OrganizationViewModel();



            //user.Name= _currentUser.Name;
            //return RedirectToPage(user.Name);
        }
       public async Task<IActionResult> OnPostAsync()
       {
         
         
           string uniquefilename = null;
           if(Photo!=null)
           {
               string uploadsfolder = Path.Combine(_env.WebRootPath, "images");
               uniquefilename=Guid.NewGuid().ToString()+ "_" +Photo.FileName;//BÝÞEY DENÝYCEM
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
