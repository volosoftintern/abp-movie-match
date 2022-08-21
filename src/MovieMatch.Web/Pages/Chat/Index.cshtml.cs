using Abp.Authorization.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Identity;
using MovieMatch.Messages;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Users;
using System.Collections.Generic;
using System.ComponentModel;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using MovieMatch.UserConnections;
using Volo.Abp.Application.Dtos;

namespace MovieMatch.Web.Pages.Chat
{
    
    public class IndexModel : PageModel
    {
        [BindProperty]
        public MessageViewModel MyModel { get; set; }
        public ICurrentUser CurrentUser;
        public IUserConnectionAppService _userConnectionAppService;
        public IMessageAppService _messageAppService;
        public IndexModel(ICurrentUser currentUser,IUserConnectionAppService userConnectionAppService,IMessageAppService messageAppService)
        {
            CurrentUser = currentUser;
            MyModel = new MessageViewModel();
            _userConnectionAppService=userConnectionAppService;
            _messageAppService=messageAppService;
        }

        public async Task OnGetAsync()
        {
            var usersFollowInfo = new GetUsersFollowInfo
            {
                username = CurrentUser.UserName
            };
            MyModel = new MessageViewModel
            {
                Following = await _userConnectionAppService.GetFollowingAsync(usersFollowInfo),
                Messages = await _messageAppService.GetMessagesListAsync()
            };
        }

        public class MessageViewModel
        {

            [DynamicFormIgnore]
            public PagedResultDto<FollowerDto> Following { get; set; }
            [DynamicFormIgnore]
            public List<MessageDto> Messages { get; set; }
        }
    }
}
