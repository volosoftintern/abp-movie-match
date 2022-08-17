using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MovieMatch.Messages
{
    public interface IMessageRepository : IRepository<Message, Guid>
    {

    }
}
