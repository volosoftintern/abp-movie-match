using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieMatch.UserConnections
{
    public class CreateUpdateUserConnectionDto
    {
        [Required]
        public Guid FollowerId { get; set; }
        [Required]
        public Guid FollowingId { get; set; }
    }
}
