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
            if (Session["customer"] != null)
                return RedirectToAction("About", "User");
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
            ViewBag.alert = "Register error, try again!";
            ViewBag.alertStyle = "alert alert-warning";
            return this.Register();
        }

        [HttpGet]

        public ActionResult Login()
        {
            if (Session["customer"] != null)
                return RedirectToAction("Index", "BRCoffee");
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
            ViewBag.alert = "Login error, try again!";
            ViewBag.alertStyle = "alert alert-warning";
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

        public ActionResult About()
        {
            if (Session["customer"] == null)
                return RedirectToAction("Login", "User");
            customer customer = (customer)Session["customer"];
            return View(customer);
        }

        [HttpPost]

        public ActionResult About(FormCollection collection, customer customer)
        {
            customer cus = (customer)Session["customer"];
            customer = data.customers.SingleOrDefault(c => c.id == cus.id);
            var fullName = collection["fullName"];
            var email = collection["email"];
            var current = collection["current"];
            var password = collection["newpass"];
            var confirm = collection["confirm"];
            var address = collection["address"];
            var phoneNumber = collection["phoneNumber"];
            var bornDate = String.Format("{0:mm/dd/yyyy}", collection["bornDate"]);
            if (cus.password.Equals(current))
            {
                customer.fullname = fullName;
                customer.username = cus.username;
                customer.email = email;
                customer.address = address;
                customer.phonenumber = phoneNumber;
                if (password != null || password != "")
                {
                    if (password.Equals(confirm)) customer.password = password;
                    else
                    {
                        ViewBag.alert = "Wrong confirm password!";
                        ViewBag.alertStyle = "alert alert-warning";
                        return this.About();
                    }
                }
                UpdateModel(customer);
                data.SubmitChanges();
                Session["customer"] = customer;
                ViewBag.alert = "Update profile complete!";
                ViewBag.alertStyle = "alert alert-success";
            }
            else
            {
                ViewBag.alert = "Wrong password!";
                ViewBag.alertStyle = "alert alert-warning";
            }
            return this.About();
        }
    }
}