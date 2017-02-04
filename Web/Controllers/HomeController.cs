using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SZPushService.Infrastructure;
using SZPushService.Model;

namespace Web.Controllers
{

    public class HomeController : Controller
    {
        // GET: Home
        private KeywordRepository keywordRepository;
        private MessageRepository messageRepository;
        public HomeController()
        {
            keywordRepository = new KeywordRepository();
            messageRepository = new MessageRepository();
        }
        public ViewResult Index()
        {
            // Add Index for column Timestamp
            // Decrease rows to search
            Stopwatch s = Stopwatch.StartNew();
            var messages = messageRepository.Messages.Where(m => m.Timestamp > DateTime.Now.AddDays(-2))
                .OrderByDescending(m => m.Timestamp).Take(20);
            //var messages = messageRepository.Messages.Where(m => m.Id == 1);
            s.Stop();
            ViewBag.debugInfo = String.Format("fetch data from db cost {0}. ",s.ElapsedMilliseconds);
            return View(messages);
        }
        public ActionResult Key()
        {
            return View(keywordRepository.Keywords);
        }

        public ViewResult Single(int id)
        {
            var messages = messageRepository.Messages.Where(m=>m.Id == id);
            return View("Index",messages);
        }

        public RedirectToRouteResult Update(Keyword keyword)
        {
            keywordRepository.Save(keyword);
            TempData["message"] = string.Format("{0} has been saved", keyword.Word);
            return RedirectToAction("Key");
        }

        public RedirectToRouteResult Toggle(Keyword keyword)
        {
            Keyword result = keywordRepository.Toggle(keyword);
            if (result != null)
            {
                TempData["message"] = string.Format("{0} has been {1}", result.Word, result.IsEnabled ? "enabled" : "disabled"); 
            }
            return RedirectToAction("Key");
        }

        public RedirectToRouteResult Delete(Keyword keyword)
        {
            Keyword result = keywordRepository.Remove(keyword);
            if (result != null)
            {
                TempData["message"] = string.Format("{0} has been deleted", result.Word);
            }
            return RedirectToAction("Key");
        }

        public RedirectToRouteResult Create(Keyword keyword)
        {
            keywordRepository.Save(keyword);
            TempData["message"] = string.Format("{0} has been created", keyword.Word);
            return RedirectToAction("Key");
        }

        public void Ping204()
        {
            Response.Status = "204 No Content";
        }

        public void PingWebAppSCM()
        {
            string websiteName = "szmj";
            string webjobName = "SZPushService";
            string userName = "$szmj";
            string userPWD = "Jt3Dxc5c7WfWGuHrondgYuRpnS7cbkfkB3fxTMiAk1njvsckjqGPuu1qaWb9";
            string webjobUrl = string.Format("https://{0}.scm.azurewebsites.net/api/continuouswebjobs/{1}", websiteName, webjobName);

            HttpClient client = new HttpClient();
            string auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(userName + ':' + userPWD));
            client.DefaultRequestHeaders.Add("authorization", auth);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var data = client.GetAsync(webjobUrl).Result;
        }

        public void SendEmail(string title,string body)
        {
            SZPushService.Email.Send(title,body);
        }

        public ViewResult A()
        {
            return View();
        }
    }
}
