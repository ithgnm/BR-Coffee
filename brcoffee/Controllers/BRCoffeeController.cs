using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using brcoffee.Models;

namespace brcoffee.Controllers
{
    public class BRCoffeeController : Controller
    {
        // GET: BRCoffee

        dbBRCoffeeDataContext data = new dbBRCoffeeDataContext();

        private List<drink> GetNewDrink(int id)
        {
            return data.drinks.OrderByDescending(dr => dr.date).Take(id).ToList();
        }

        public ActionResult Index()
        {
            var newDrink = GetNewDrink(3);
            return View(newDrink);
        }

        public ActionResult Category()
        {
            var category = from ct in data.categories select ct;
            return PartialView(category);
        }

        public ActionResult DrinkByCategory(int id)
        {
            var drink = from dr in data.drinks where dr.idcategory == id select dr;
            return View(drink);
        }

        public ActionResult DrinkDetail(int id)
        {
            var drink = from dr in data.drinks where dr.id == id select dr;
            return View(drink.Single());
        }
    }
}