using Abp.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MovieMatch.Messages
{
    public class Message :Entity<Guid>
    {
        private Message() { }
        public string TargetUserName { get; set; }
        public string Text { get; set; }
        public DateTime When { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Message(string targetUserName, string text, DateTime when, Guid userId, string userName)
        {
            Id = Guid.NewGuid();
            TargetUserName = targetUserName;
            Text = text;
            When = when;
            UserId = userId;
            UserName = userName;
        }

     
    }
}
