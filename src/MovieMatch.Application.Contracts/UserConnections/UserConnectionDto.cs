using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Identity;

namespace MovieMatch.UserConnections
{
    public class UserConnectionDto
    {
        [Required]
        public Guid FollowersId { get; set; }
        [Required]
        public Guid FollowingId { get; set; }
        [CanBeNull]
        public RemoteStreamContent ProfilePictureStreamContent { get; set; }

    }

    public class FollowerDto
    {

        public FollowerDto()
        {
           
           
        }
        public FollowerDto(Guid id)
        {
            Id = id;
         
        }

        public Guid Id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
    }
}
