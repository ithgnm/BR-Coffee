using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using brcoffee.Models;

namespace brcoffee.Models
{
    public class Cart
    {
        dbBRCoffeeDataContext data = new dbBRCoffeeDataContext();

        public int ID { set; get; }
        public string Name { set; get; }
        public string Picture { set; get; }
        public string Describe { set; get; }
        public double Price { set; get; }
        public int Count { set; get; }
        public double TotalPrice { get { return Count * Price; } }

        public Cart(int id)
        {
            this.ID = id;
            drink drink = data.drinks.Single(data => data.id == id);
            Name = drink.name;
            Picture = drink.picture;
            Describe = drink.describe;
            Price = double.Parse(drink.price.ToString());
            Count = 1;
        }
    }
}