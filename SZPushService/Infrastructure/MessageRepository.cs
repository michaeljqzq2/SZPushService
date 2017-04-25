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

        public IEnumerable<Message> GetPart(int i,int n)
        {
            return db.Messages.OrderByDescending(m => m.Timestamp).Skip(n * i).Take(n);
        }

        public IEnumerable<Message> GetPartKeyword(int i, int n, string key)
        {
            return db.Messages.Where(m => m.Keyword == key).OrderByDescending(m => m.Timestamp).Skip(n * i).Take(n);
        }
    }
}
