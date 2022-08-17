using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace MovieMatch.Messages
{
    public class MessageDto:EntityDto<Guid>
    {
        public string TargetUserName { get; set; }
        public string Text { get; set; }
        public DateTime When { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }

    }
}
