using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSoup;
using System.Text.RegularExpressions;
using SZPushService.Infrastructure;

namespace SZPushService.Model
{
    public class Parser
    {
        public static Dictionary<string, Func<string, List<string>, List<Message>>> Parsers;
        static Parser()
        {
            Parsers = new Dictionary<string, Func<string, List<string>, List<Message>>>();
            Parsers.Add("Domestic", ParserForDomestic);
            Parsers.Add("Faxian", ParserForFaxian);
            Parsers.Add("Manmanbuy", ParserForManmanbuy);
        }

        private static List<Message> ParserForDomestic(string htmlPage, List<string> keywords)
        {
            var doc = NSoupClient.Parse(htmlPage);
            var elements = doc.GetElementsByTag("div").Where(e => e.HasClass("list"));
            List<Message> result = new List<Message>();
            using (var db = new SZDbContext())
            {
                foreach (var element in elements)
                {
                    string articleId = element.Attributes["articleid"];
                    var titleElement = element.GetElementsByTag("h4")[0].Child(0);
                    string link = titleElement.Attributes["href"];
                    string title = titleElement.Html();
                    var compareResult = FindKeyword(title, keywords);
                    if (compareResult == null) continue;

                    if (db.Messages.Any(msg => msg.ArticleId == articleId)) continue;
                    //if (db.Messages.Any(msg => msg.Title.TrimEnd(' ') == title.TrimEnd(' ')))
                    //{

                    //    continue;
                    //}

                    Regex priceRegex = new Regex(@"<span class=""red"">([^<]*)</span>");
                    Match m = priceRegex.Match(title);
                    string price = "";
                    if (m.Success)
                    {
                        price = m.Groups[1].Value;
                    }
                    title = priceRegex.Replace(title, "");
                    var detailElement = element.GetElementsByClass("lrInfo");
                    if (detailElement.Count == 0) Console.WriteLine("Can't locate info about details");
                    Regex detailRegex = new Regex(@"<a[^>]+>([^<]*)</a>");
                    string detail = detailElement[0].Html();
                    var ms = detailRegex.Matches(detail);
                    foreach (Match mm in ms)
                    {
                        detail = detail.Replace(mm.Groups[0].Value, mm.Groups[1].Value);
                    }
                    detail = detail.Replace("阅读全文", "");
                    detail = detail.Replace("\n", "");
                    var message = new Message()
                    {
                        ArticleId = articleId,
                        Detail = detail,
                        Keyword = compareResult,
                        Price = price,
                        Title = title,
                        Timestamp = DateTime.Now,
                        Source = "Domestic",
                        Html = element.OuterHtml()
                    };
                    db.Messages.Add(message);
                    db.SaveChanges();
                    result.Add(message);
                    Console.WriteLine("[Domestic] A new item added:{0}", title);
                }
            }
            return result;
        }

        private static List<Message> ParserForFaxian(string htmlPage, List<string> keywords)
        {
            var doc = NSoupClient.Parse(htmlPage);
            var elements = doc.GetElementsByTag("li").Where(e => e.HasClass("list"));
            List<Message> result = new List<Message>();
            using (var db = new SZDbContext())
            {
                foreach (var element in elements)
                {
                    string articleId = element.Attributes["articleid"];
                    var titleElement = element.GetElementsByTag("h2")[0].Child(0);
                    string link = titleElement.Attributes["href"];
                    string title = titleElement.Html();
                    var compareResult = FindKeyword(title, keywords);
                    if (compareResult == null) continue;

                    if (db.Messages.Any(msg => msg.ArticleId == articleId)) continue;
                    //if (db.Messages.Any(msg => msg.Title.TrimEnd(new char[] { ' ' }) == title.TrimEnd(new char[] { ' ' }))) continue;

                    Regex priceRegex = new Regex(@"<span class=""red"">([^<]*)</span>");
                    Match m = priceRegex.Match(title);
                    string price = "";
                    if (m.Success)
                    {
                        price = m.Groups[1].Value;
                    }
                    title = priceRegex.Replace(title, "");

                    Regex titleRegex = new Regex(@"<span class=""black"">([^<]*)</span>");
                    var ms = titleRegex.Matches(title);
                    foreach (Match mm in ms)
                    {
                        title = title.Replace(mm.Groups[0].Value, mm.Groups[1].Value);
                    }

                    var detailElement = element.GetElementsByClass("listItem")[0].GetElementsByTag("p");
                    if (detailElement.Count == 0) Console.WriteLine("Can't locate info about details");
                    Regex detailRegex = new Regex(@"<a[^>]+>([^<]*)</a>");
                    string detail = detailElement[0].Html();
                    ms = detailRegex.Matches(detail);
                    foreach (Match mm in ms)
                    {
                        detail = detail.Replace(mm.Groups[0].Value, mm.Groups[1].Value);
                    }
                    detail = detail.Replace("阅读全文", "");
                    detail = detail.Replace("\n", "");
                    var message = new Message()
                    {
                        ArticleId = articleId,
                        Detail = detail,
                        Keyword = compareResult,
                        Price = price,
                        Title = title,
                        Timestamp = DateTime.Now,
                        Source = "Faxian",
                        Html = element.OuterHtml()
                    };
                    db.Messages.Add(message);
                    db.SaveChanges();
                    result.Add(message);
                    Console.WriteLine("[Faxian] A new item added:{0}", title);
                }
            }
            return result;
        }

        private static List<Message> ParserForManmanbuy(string htmlPage, List<string> keywords)
        {
            var doc = NSoupClient.Parse(htmlPage);
            var elements = doc.GetElementsByTag("div").Where(e => e.HasClass("infolist"));
            List<Message> result = new List<Message>();
            using (var db = new SZDbContext())
            {
                foreach (var element in elements)
                {
                    var smallElement = element.GetElementsByTag("td")[1];
                    string articleId = smallElement.GetElementsByTag("a")[0].Attributes["href"];
                    var titleElement = smallElement.GetElementsByTag("h2")[0];
                    //string link = titleElement.Attributes["href"];
                    string title = GB2312ToUtf8(titleElement.Html());
                    var compareResult = FindKeyword(title, keywords);
                    if (compareResult == null) continue;

                    if (db.Messages.Any(msg => msg.ArticleId == articleId)) continue;

                    Regex priceRegex = new Regex(@"<span>([^<]*)</span>");
                    Match m = priceRegex.Match(title);
                    string price = "";
                    if (m.Success)
                    {
                        price = m.Groups[1].Value.Replace(@"&nbsp;&nbsp;&nbsp;", "");
                    }
                    title = priceRegex.Replace(title, "");

                    var detailElement = smallElement.GetElementsByClass("infoD");
                    if (detailElement.Count == 0) Console.WriteLine("Can't locate info about details");
                    Regex detailRegex = new Regex(@"<a[^>]+>([^<]*)</a>");
                    string detail = detailElement[0].Html();
                    var ms = detailRegex.Matches(detail);
                    foreach (Match mm in ms)
                    {
                        detail = detail.Replace(mm.Groups[0].Value, mm.Groups[1].Value);
                    }
                    detail = detail.Replace("阅读全文", "");
                    detail = detail.Replace("\n", "");

                    string outerHtml = element.OuterHtml();
                    Regex hrefRegex = new Regex(@"href=\""([\w|\.]+)\""");
                    ms = hrefRegex.Matches(outerHtml);
                    foreach (Match mm in ms)
                    {
                        outerHtml = outerHtml.Replace(mm.Groups[0].Value, mm.Groups[0].Value.Replace(mm.Groups[1].Value, UData.Urls["Manmanbuy"][0] + mm.Groups[1].Value));
                    }

                    var message = new Message()
                    {
                        ArticleId = articleId,
                        Detail = detail,
                        Keyword = compareResult,
                        Price = price,
                        Title = title,
                        Timestamp = DateTime.Now,
                        Source = "Manmanbuy",
                        Html = outerHtml
                    };
                    db.Messages.Add(message);
                    db.SaveChanges();
                    result.Add(message);
                    Console.WriteLine("[Manmanbuy] A new item added:{0}", title);
                }
            }
            return result;
        }

        private static string FindKeyword(string content, List<string> keywords)
        {
            content = content.ToLower();
            return keywords.FirstOrDefault(k =>
            {
                k = k.ToLower();
                if (!k.Contains(' '))
                {
                    return content.Contains(k);
                }
                else
                {
                    string[] parts = k.Split(' ');
                    return parts.All(p =>
                    {
                        if (p[0] == '-')
                        {
                            return !content.Contains(p.Substring(1));
                        }
                        else
                        {
                            return content.Contains(p);
                        }
                    });
                }
            });
        }

        private static string GB2312ToUtf8(string gb2312String)
        {
            Encoding fromEncoding = Encoding.GetEncoding("gb2312");
            Encoding toEncoding = Encoding.UTF8;
            return EncodingConvert(gb2312String, fromEncoding, toEncoding);
        }

        private static string EncodingConvert(string fromString, Encoding fromEncoding, Encoding toEncoding)
        {
            byte[] fromBytes = fromEncoding.GetBytes(fromString);
            byte[] toBytes = Encoding.Convert(fromEncoding, toEncoding, fromBytes);

            string toString = toEncoding.GetString(toBytes);
            return toString;
        }
    }
}
