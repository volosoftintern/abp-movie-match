using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Public.Comments;

namespace MovieMatch.Comments
{
    [Serializable]
    public class CommentDto : IHasConcurrencyStamp
    {
        public Guid Id
        {
            get;
            set;
        }

        public string EntityType
        {
            get;
            set;
        }

        public string EntityId
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public Guid? RepliedCommentId
        {
            get;
            set;
        }

        public Guid CreatorId
        {
            get;
            set;
        }

        public DateTime CreationTime
        {
            get;
            set;
        }

        public CmsUserDto Author
        {
            get;
            set;
        }
        public List<CommentDto> Replies
        {
            get;
            set;
        }

        public string ConcurrencyStamp
        {
            get;
            set;
        }
    }
}