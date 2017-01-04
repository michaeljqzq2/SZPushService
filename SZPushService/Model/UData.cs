using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZPushService.Model
{
    public class UData
    {
        public static Dictionary<string, List<string>> Urls;
        static UData()
        {
            Urls = new Dictionary<string, List<string>>();
            Urls.Add("Domestic", new List<string>() { "http://www.smzdm.com/youhui/", "UTF-8",@"<link rel=""stylesheet"" href=""http://www.smzdm.com/resources/public/css/main.css?v=20151201"" type=""text/css"">"});
            Urls.Add("Faxian", new List<string>() { "http://faxian.smzdm.com/","UTF-8",@"<link rel=""stylesheet"" href=""http://www.smzdm.com/resources/public/css/main.css?v=20151201"" type=""text/css"">" });
            Urls.Add("Manmanbuy", new List<string>() { "http://cu.manmanbuy.com/", "GB2312",@"<link href=""http://cu.manmanbuy.com/css/cx.css"" type=""text/css"" rel=""stylesheet"">" });
        }
    }
}

