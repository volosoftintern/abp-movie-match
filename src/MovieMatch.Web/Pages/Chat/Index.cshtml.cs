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

namespace MovieMatch.Web.Pages.Chat
{
    
    public class IndexModel : PageModel
    {
        public List<Message> Messages = new List<Message>();
        public string TargetUserName { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime When { get; set; }
        public int Count { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        private readonly IMessageAppService _messageAppService;
        private readonly IMessageRepository _messageRepository;
        private readonly ICurrentUser _currentUser;
        public IndexModel(IMessageAppService messageAppService, IMessageRepository messageRepository,ICurrentUser currentUser)
        {
            When = DateTime.Now;
            _messageAppService = messageAppService;
            _currentUser = currentUser;
            _messageRepository = messageRepository;
        }
        public void OnGetAsync()
        {
            
           // Messages = await _messageRepository.GetListAsync();
        }
        //private async Task GetDataAsync()
        //{
            
        //}
    }
}
