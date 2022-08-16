using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;

namespace MovieMatch
{
    public class GetUsersFollowInfo : GetIdentityUsersInput
    {
       public string username { get; set; }
    }
}
