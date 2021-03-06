﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SZPushService.Model;
using System.Net;
using System.Data.Entity;
using SZPushService.Infrastructure;
using System.IO;
using uPLibrary.Networking.M2Mqtt;

namespace SZPushService
{
    public class Crawler
    {
        private static int threadNum=-1;

        public void Start(object source, System.Timers.ElapsedEventArgs e)
        {
            threadNum = ( threadNum + 1 ) % UData.Urls.Keys.Count;
            string stype = (string) UData.Urls.Keys.ElementAt(threadNum);
            Console.WriteLine("Crawler started with type:{0}",stype);
            //fetch from web
            if (!UData.Urls.ContainsKey(stype)) return;
            Console.WriteLine("[{0}] Fetching web page...",stype);
            string pageHtml = FetchHTML(stype);
            if(pageHtml != null) Console.WriteLine("[{0}] Web page fetched", stype);
            else
            {
                Console.WriteLine("[{0}] Web page fetch failed", stype);
                return;
            }
            //fetch key word
            var query = GetKeywords();

            //compare and call db if there's update
            Console.WriteLine("[{0}] Starting parser...", stype);
            Func<string, List<string>, List<Message>> parser = Parser.Parsers[stype];
            var result = parser(pageHtml, query);
            Console.WriteLine("[{0}] Detect {1} updates.", stype, result.Count);
            //CreateSendEmail(result, UData.Urls[stype][2]);
            MqttPush(result);
        }

        private string FetchHTML(string type)
        {
            try
            {
                string url = UData.Urls[type][0];
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                WebProxy proxy = new WebProxy("http://cache.sjtu.edu.cn:8080");
                //MyWebClient.Proxy = proxy;
                MyWebClient.Headers.Add(HttpRequestHeader.Cookie, "web_fx_ab=fx6");
                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据
                string pageHtml = null;
                if (UData.Urls[type][1] == "GB2312")
                {
                    pageHtml = Encoding.GetEncoding("gb2312").GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句  
                }else if (UData.Urls[type][1] == "UTF-8")
                {
                    pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
                }
                return pageHtml;           
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
                return null;
            }
        }

        private List<string> GetKeywords()
        {
            //Database.SetInitializer<SZDbContext>(null);
            using (var db = new SZDbContext())
            {
                var query = from x in db.Keywords
                            where x.IsEnabled == true
                            select x.Word;
                return query.ToList<string>();
            }
        }

        private bool isKeywordRemind(string keyword)
        {
            using (var db = new SZDbContext())
            {
                var query = (from x in db.Keywords
                            where x.Word == keyword
                            select x.Remind).FirstOrDefault();
                return query;
            }
        }

        private void CreateSendEmail(List<Message> messages,string styles)
        {
            if (messages.Count == 0) return;
            Console.WriteLine("Writing Email...");
            string title = string.Format("{0}",messages[0].Source.Substring(0,1));
            string body = "";
            foreach (var message in messages)
            {
                if (!isKeywordRemind(message.Keyword))
                {
                    continue;
                }
                title = message.Keyword;
                body += message.Title + "\n\n" + message.Detail;
                body += (" http://szmj.azurewebsites.net/s?id=" + message.Id);
                Email.Send(title, body);
            }
            Console.WriteLine("Sending Email...");
            Console.WriteLine("Email successfully sent");
        }

        private void MqttPush(List<Message> messages)
        {
            if (messages.Count > 0)
            {
                MqttClient client = new MqttClient("fqmj.cloudapp.net");
                client.Connect(Guid.NewGuid().ToString());
                foreach (var message in messages)
                {
                    if (!isKeywordRemind(message.Keyword))
                    {
                        continue;
                    }
                    string content = message.Source.Substring(0, 1) + " " + message.Keyword + " " + message.Title;
                    client.Publish("p/sz", Encoding.UTF8.GetBytes(content));
                    Console.WriteLine("Mqtt message has been sent about " + message.Title);
                }
                System.Threading.Thread.Sleep(2000);
                client.Disconnect();
            }
        }
    }
}
