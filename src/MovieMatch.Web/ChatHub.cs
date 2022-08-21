using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MovieMatch.Messages;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace MovieMatch.Web
{
    [Authorize]
    public class ChatHub : AbpHub
    {
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly ICurrentUser _currentUser;
        private readonly IMessageAppService _messageAppService;

        public ChatHub(IIdentityUserRepository identityUserRepository, ILookupNormalizer lookupNormalizer,ICurrentUser currentUser,IMessageAppService messageAppService)
        {
            _currentUser = currentUser;
            _identityUserRepository = identityUserRepository;
            _lookupNormalizer = lookupNormalizer;
            _messageAppService = messageAppService;
        }

        public async Task SendMessage(string targetUserName,string text)
        {
            var targetUser = await _identityUserRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName(targetUserName));
            await Clients
                .User(targetUser.Id.ToString())
                .SendAsync("ReceiveMessage", CurrentUser.UserName, text);
        }
    }
}
