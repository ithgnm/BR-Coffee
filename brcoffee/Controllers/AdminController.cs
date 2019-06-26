using brcoffee.Common;
using brcoffee.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace brcoffee.Controllers
{
    public class AdminController :  Controller
    {
        // GET: Admin

        dbBRCoffeeDataContext data = new dbBRCoffeeDataContext();

        [SessionCheck]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Login(FormCollection collection)
        {
            var userName = collection["userName"];
            var password = collection["password"];
            account account = data.accounts.SingleOrDefault(acc => acc.username == userName && acc.password == password);
            if (account != null)
            {
                Session["account"] = account;
                return RedirectToAction("Index", "Admin");
            }
            else return View();
        }

        public ActionResult Logout()
        {
            Session["account"] = null;
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult AccountPartial()
        {
            return PartialView();
        }

        [SessionCheck]

        public ActionResult Order()
        {
            return View(data.bills.ToList());
        }

        [SessionCheck]

        public ActionResult OrderDetails(int id)
        {
            ViewBag.IDBill = id;
            ViewBag.Text = "Delivery Now";
            bill bill = data.bills.SingleOrDefault(b => b.id == id);
            if (bill.payment == true)
            {
                ViewBag.Text = "Deliveried";
                ViewBag.Disable = "disabled";
            }
            return View(data.billinfos.Where(bi => bi.idbill == id).ToList());
        }

        [HttpPost, ActionName("OrderDetails")]

        public ActionResult OrderDelivery(int id)
        {
            bill bill = data.bills.SingleOrDefault(b => b.id == id);
            bill.payment = true;
            UpdateModel(bill);
            data.SubmitChanges();
            return RedirectToAction("Order");
        }

        [SessionCheck]

        public ActionResult Drink()
        {
            return View(data.drinks.ToList());
        }

        [SessionCheck]
        [HttpGet]

        public ActionResult DrinkCreate()
        {
            ViewBag.idcategory = new SelectList(data.categories.ToList().OrderBy(cd => cd.name), "id", "name");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult DrinkCreate(drink drink, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileName(fileUpload.FileName);
                string path = Path.Combine(Server.MapPath("~/Resources/Drink"), fileName);
                fileUpload.SaveAs(path);
                drink.picture = fileName;
                data.drinks.InsertOnSubmit(drink);
                data.SubmitChanges();
            }
            return RedirectToAction("Drink");
        }

        [SessionCheck]
        [HttpGet]

        public ActionResult DrinkDelete(int id)
        {
            drink drink = data.drinks.SingleOrDefault(dr => dr.id == id);
            ViewBag.id = drink.id;
            ViewBag.picture = drink.picture;
            return View(drink);
        }
        
        [HttpPost, ActionName("DrinkDelete")]

        public ActionResult DeleteConfirm(int id)
        {
            drink drink = data.drinks.SingleOrDefault(dr => dr.id == id);
            ViewBag.id = drink.id;
            if (drink != null)
            {
                data.drinks.DeleteOnSubmit(drink);
                data.SubmitChanges();
            }
            return RedirectToAction("Drink");
        }

        [SessionCheck]
        [HttpGet]

        public ActionResult DrinkEdit(int id)
        {
            ViewBag.idcategory = new SelectList(data.categories.ToList().OrderBy(cd => cd.name), "id", "name");
            drink drink = data.drinks.SingleOrDefault(dr => dr.id == id);
            ViewBag.drinkName = drink.name;
            ViewBag.describe = drink.describe;
            ViewBag.picture = drink.picture;
            return View(drink);
        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult DrinkEdit(drink drink, int id, HttpPostedFileBase fileUpload)
        {
            drink = data.drinks.SingleOrDefault(dr => dr.id == id);
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileName(fileUpload.FileName);
                string path = Path.Combine(Server.MapPath("~/Resources/Drink"), fileName);
                fileUpload.SaveAs(path);
                drink.picture = fileName;
                UpdateModel(drink);
                data.SubmitChanges();
            }
            return RedirectToAction("Drink");
        }

        [SessionCheck]

        public ActionResult News()
        {
            return View(data.news.ToList());
        }

        [SessionCheck]
        [HttpGet]

        public ActionResult NewsCreate()
        {
            return View();
        }

        [SessionCheck]

        public ActionResult Customer()
        {
            return View(data.customers.ToList());
        }

        [SessionCheck]

        public ActionResult Admin()
        {
            return View(data.accounts.ToList());
        }
    }
}