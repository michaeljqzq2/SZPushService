using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SZPushService.Model;

namespace SZPushService.Infrastructure
{
    public class KeywordRepository
    {
        private SZDbContext db;
        public KeywordRepository()
        {
            db = new SZDbContext();
        }
        public IEnumerable<Keyword> Keywords { get { return db.Keywords; } }
        public void Save(Keyword k)
        {
            if (k.Id == 0)
            {
                db.Keywords.Add(k);
            }
            else
            {
                Keyword dbentry = db.Keywords.Find(k.Id);
                if (dbentry != null)
                {
                    dbentry.Word = k.Word;
                }
            }
            db.SaveChanges();
        }

        public Keyword Remove(Keyword k)
        {
            Keyword dbentry = db.Keywords.Find(k.Id);
            if (dbentry != null)
            {
                db.Keywords.Remove(dbentry);
                db.SaveChanges();
            }
            return dbentry;
        }
    }
}
