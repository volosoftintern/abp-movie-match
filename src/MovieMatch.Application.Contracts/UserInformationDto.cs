using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatch
{
    public class UserInformationDto
    {
        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }
        public string Path { get; set; }
        public string Username { get; set; }        

    }
}
