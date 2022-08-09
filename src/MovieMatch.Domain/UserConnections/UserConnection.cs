using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
namespace MovieMatch.UserConnections
{
   public class UserConnection:Entity
    {
        public Guid FollowerId { get;  set; }
        public Guid FollowingId { get;  set; }
        public bool isFollowed { get; set; }
      //  private UserConnection() { }
        

        public UserConnection(Guid followerId, Guid followingId, bool isFollowed)
        {
           this.isFollowed=isFollowed;
            FollowerId = followerId;
            FollowingId = followingId;
        }

        public override object[] GetKeys()
        {
            return new object[] { FollowerId, FollowingId };
        }
    }
}
