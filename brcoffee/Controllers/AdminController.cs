using brcoffee.Models;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace brcoffee.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        dbBRCoffeeDataContext data = new dbBRCoffeeDataContext();

        public ActionResult Index()
        {
            if (Session["account"] == null || Session["account"].ToString() == "")
                return RedirectToAction("Login", "Admin");
            return View();
        }

        [HttpGet]

        public ActionResult Login()
        {
            if (Session["account"] != null)
                return RedirectToAction("Index", "Admin");
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

        public ActionResult Drink()
        {
            if (Session["account"] == null || Session["account"].ToString() == "")
                return RedirectToAction("Login", "Admin");
            return View(data.drinks.ToList());
        }

        [HttpGet]

        public ActionResult DrinkCreate()
        {
            if (Session["account"] == null || Session["account"].ToString() == "")
                return RedirectToAction("Login", "Admin");
            ViewBag.idcategory = new SelectList(data.categories.ToList().OrderBy(cd => cd.name), "id", "name");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult DrinkCreate(drink drink, HttpPostedFileBase fileUpload)
        {
            if (Session["account"] == null || Session["account"].ToString() == "")
                return RedirectToAction("Login", "Admin");
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

        [HttpGet]

        public ActionResult DrinkDelete(int id)
        {
            if (Session["account"] == null || Session["account"].ToString() == "")
                return RedirectToAction("Login", "Admin");
            drink drink = data.drinks.SingleOrDefault(dr => dr.id == id);
            ViewBag.id = drink.id;
            ViewBag.picture = drink.picture;
            return View(drink);
        }
        
        [HttpPost, ActionName("DrinkDelete")]

        public ActionResult DeleteConfirm(int id)
        {
            if (Session["account"] == null || Session["account"].ToString() == "")
                return RedirectToAction("Login", "Admin");
            drink drink = data.drinks.SingleOrDefault(dr => dr.id == id);
            ViewBag.id = drink.id;
            if (drink != null)
            {
                data.drinks.DeleteOnSubmit(drink);
                data.SubmitChanges();
            }
            return RedirectToAction("Drink");
        }

        [HttpGet]

        public ActionResult DrinkEdit(int id)
        {
            if (Session["account"] == null || Session["account"].ToString() == "")
                return RedirectToAction("Login", "Admin");
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
            if (Session["account"] == null || Session["account"].ToString() == "")
                return RedirectToAction("Login", "Admin");
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
    }
}