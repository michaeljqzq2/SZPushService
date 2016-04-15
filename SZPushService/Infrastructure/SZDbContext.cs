using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SZPushService.Model;

namespace SZPushService.Infrastructure
{
    public class SZDbContext : DbContext
    {
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
