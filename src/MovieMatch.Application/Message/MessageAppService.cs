using DM.MovieApi.MovieDb.Movies;
using IdentityServer4.Models;
using MovieMatch.Messages;
using MovieMatch.Movies;
using MovieMatch.MoviesWatchLater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace MovieMatch.Messages
{
    public class MessageAppService :
        CrudAppService<
            Message,
            MessageDto, //Used to show WatchLaters
            Guid, //Primary key of the WatchLater entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateMessageDto>, //Used to create/update a WatchLater
        IMessageAppService //implement the IWatchLaterAppService
        {
        private readonly ICurrentUser _currentUser;
        private readonly IMessageRepository _messageRepository;
        public MessageAppService(IRepository<Message, Guid> repository,
             IMessageRepository messageRepository,
             ICurrentUser currentUser) : base(repository)
        {
            _currentUser = currentUser;
            _messageRepository = messageRepository;
        }
        public override async Task<MessageDto> CreateAsync(CreateMessageDto input)
        {
            
            var createMessage = new CreateMessageDto(input.Id,input.TargetUserName, input.Text, input.When, input.UserId, input.UserName);
            var message=ObjectMapper.Map<CreateMessageDto, Message>(createMessage);
            await _messageRepository.InsertAsync(message);
            return null;
        }
    }
}

