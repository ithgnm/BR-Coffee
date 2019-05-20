using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using brcoffee.Models;

namespace brcoffee.Controllers
{
    public class UserController : Controller
    {
        // GET: User

        dbBRCoffeeDataContext data = new dbBRCoffeeDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Register(FormCollection collection, customer customer)
        {
            var fullName = collection["fullName"];
            var userName = collection["userName"];
            var email = collection["email"];
            var password = collection["password"];
            var confirm = collection["confirm"];
            var address = collection["address"];
            var phoneNumber = collection["phoneNumber"];
            var bornDate = String.Format("{0:mm/dd/yyyy}", collection["bornDate"]);
            if (password.Equals(confirm))
            {
                customer.fullname = fullName;
                customer.username = userName;
                customer.email = email;
                customer.password = password;
                customer.address = address;
                customer.phonenumber = phoneNumber;
                customer.borndate = DateTime.Parse(bornDate);
                data.customers.InsertOnSubmit(customer);
                data.SubmitChanges();
                return RedirectToAction("Login");
            }
            return this.Register();
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
            customer customer = data.customers.SingleOrDefault(cus => cus.username == userName && cus.password == password);
            if (customer != null)
            {
                Session["customer"] = customer;
                return RedirectToAction("Index", "BRCoffee");
            }
            return View();
        }

        public ActionResult UserPartial()
        {
            if (Session["customer"] != null)
            {
                customer customer = (customer)Session["customer"];
                ViewBag.fullName = customer.fullname;
            }
            else ViewBag.fullName = "Login";
            return PartialView();
        }

        public ActionResult Logout()
        {
            Session["customer"] = null;
            return RedirectToAction("Index", "BRCoffee");
        }
    }
}