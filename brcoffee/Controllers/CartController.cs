using brcoffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace brcoffee.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart

        dbBRCoffeeDataContext data = new dbBRCoffeeDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public List<Cart> getCart()
        {
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart == null)
            {
                listCart = new List<Cart>();
                Session["Cart"] = listCart;
            }
            return listCart;
        }

        public ActionResult addCart(int id, string url)
        {
            List<Cart> listCart = getCart();
            Cart product = listCart.Find(pd => pd.ID == id);
            if (product == null)
            {
                product = new Cart(id);
                listCart.Add(product);
                return Redirect(url);
            }
            else
            {
                product.Count++;
                return Redirect(url);
            }
        }

        private int totalCount()
        {
            int totalCount = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null) totalCount = listCart.Sum(pd => pd.Count);
            return totalCount;
        }

        private double totalPrice()
        {
            double totalPrice = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null) totalPrice = listCart.Sum(pd => pd.TotalPrice);
            return totalPrice;
        }

        public ActionResult Cart()
        {
            List<Cart> listCart = getCart();
            if (listCart.Count == 0) return RedirectToAction("Index", "BRCoffee");
            ViewBag.totalCount = totalCount();
            ViewBag.totalPrice = totalPrice();
            return View(listCart);
        }

        public ActionResult CartPartial()
        {
            ViewBag.totalCount = totalCount();
            return PartialView();
        }

        public ActionResult removeCart(int id)
        {
            List<Cart> listCart = getCart();
            Cart product = listCart.SingleOrDefault(pd => pd.ID == id);
            if (product != null)
            {
                listCart.RemoveAll(pd => pd.ID == id);
                return RedirectToAction("Cart");
            }
            if (listCart != null) return RedirectToAction("Index", "BRCoffee");
            return RedirectToAction("Cart");
        }

        [HttpGet]

        public ActionResult Order()
        {
            if (Session["customer"] == null || Session["customer"].ToString() == "")
                return RedirectToAction("Login", "User");
            if (Session["Cart"] == null)
                return RedirectToAction("Index", "BRCoffee");
            List<Cart> listCart = getCart();
            ViewBag.totalCount = totalCount();
            ViewBag.totalPrice = totalPrice();
            return View(listCart);
        }

        public ActionResult Order(FormCollection collection)
        {
            bill bill = new bill();
            customer customer = (customer)Session["customer"];
            List<Cart> listCart = getCart();
            bill.idcustomer = customer.id;
            bill.date = DateTime.Now;
            bill.status = true;
            data.bills.InsertOnSubmit(bill);
            data.SubmitChanges();
            foreach (var item in listCart)
            {
                billinfo billinfo = new billinfo();
                billinfo.idbill = bill.id;
                billinfo.iddrink = item.ID;
                billinfo.count = item.Count;
                billinfo.price = (decimal)item.Price;
                data.billinfos.InsertOnSubmit(billinfo);
            }
            data.SubmitChanges();
            Session["Cart"] = null;
            return RedirectToAction("Index", "BRCoffee");
        }
    }
}