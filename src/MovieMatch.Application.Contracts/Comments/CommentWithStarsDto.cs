using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Public.Comments;

namespace MovieMatch.Comments
{
    [Serializable]
    public class CommentWithStarsDto 
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

        public List<CommentDto> Replies
        {
            get;
            set;
        }

        public MyCmsUserDto Author {get; set;}

        public string ConcurrencyStamp
        {
            get;
            set;
        } public string Path
        {
            get;
            set;
        }
        public int StarsCount { get; set; }
        public int PageNumber { get; set; }

    }
}