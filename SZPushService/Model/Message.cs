using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZPushService.Model
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ArticleId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Price { get; set; }
        public string Keyword { get; set; }
        public string Source { get; set; }
        public string Html { get; set; }
    }
}
