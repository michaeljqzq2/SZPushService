using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SZPushService.Model;

namespace SZPushService.Infrastructure
{
    public class MessageRepository
    {
        private SZDbContext db;
        public MessageRepository()
        {
            db = new SZDbContext();
        }

        public IEnumerable<Message> Messages { get { return db.Messages; } }
    }
}
