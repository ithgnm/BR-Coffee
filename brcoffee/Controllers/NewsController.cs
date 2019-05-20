using brcoffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace brcoffee.Controllers
{
    public class NewsController : Controller
    {
        // GET: News

        dbBRCoffeeDataContext data = new dbBRCoffeeDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult News(int id)
        {
            var news = from ns in data.news where ns.id == id select ns;
            return View(news.Single());
        }

        public ActionResult ListNews()
        {
            var listNews = from ln in data.news select ln;
            return PartialView(listNews);
        }
    }
}