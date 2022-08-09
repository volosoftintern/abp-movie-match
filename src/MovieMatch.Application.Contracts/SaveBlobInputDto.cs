using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Content;

namespace MovieMatch
{
    public class SaveBlobInputDto
    {
        public IRemoteStreamContent Content { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
