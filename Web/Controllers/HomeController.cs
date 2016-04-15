using System;
using System.Collections.Generic;
using System.Linq;
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
            return View(messageRepository.Messages.OrderByDescending(m=>m.Timestamp).Take(20));
        }
        public ActionResult Key()
        {
            return View(keywordRepository.Keywords);
        }

        public RedirectToRouteResult Update(Keyword keyword)
        {
            keywordRepository.Save(keyword);
            TempData["message"] = string.Format("{0} has been saved", keyword.Word);
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

        public ViewResult A()
        {
            return View();
        }
    }
}
