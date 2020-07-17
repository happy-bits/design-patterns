/*
 
 https://code-maze.com/facade/

 Göm undersystem som har dålig design eller är komplicerade

*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.Fascade
{
    [TestClass]
    public class Demo2
    {
        static List<string> _log = new List<string>();

        [TestMethod]
        public void Ex1()
        {

            var facade = new Facade();

            var chickenOrder = new Order() { DishName = "Chicken", DishPrice = 20.0, User = "User1", ShippingAddress = "Street 123" };
            var sushiOrder = new Order() { DishName = "Sushi", DishPrice = 52.0, User = "User2", ShippingAddress = "Street 444" };

            Assert.AreEqual(0, _log.Count); // Inget har hänt so far

            facade.OrderFood(new List<Order>() { chickenOrder, sushiOrder });

            CollectionAssert.AreEqual(new[] {
                "Orders completed. Dispatch in progress...",
                "User User1 ordered Chicken. The full price is 35.5 dollars.",
                "Order is being shipped to Street 123",
                "User User2 ordered Sushi. The full price is 67.5 dollars.",
                "Order is being shipped to Street 444",

        }, _log);
        }

        public class Facade
        {
            private readonly OnlineRestaurant _restaurant = new OnlineRestaurant();
            private readonly ShippingService _shippingService = new ShippingService();

            // Fasaden har en enkel metod som i sin tur anropar undersystem

            public void OrderFood(List<Order> orders)
            {
                foreach (var order in orders)
                {
                    _restaurant.AddOrderToCart(order);
                }

                _restaurant.CompleteOrders();

                foreach (var order in orders)
                {
                    _shippingService.AcceptOrder(order);
                    _shippingService.CalculateShippingExpenses();
                    _shippingService.ShipOrder();
                }
            }
        }

        public class ShippingService
        {
            private Order _order;

            public void AcceptOrder(Order order)
            {
                _order = order;
            }

            public void CalculateShippingExpenses()
            {
                _order.ShippingPrice = 15.5;
            }

            public void ShipOrder()
            {
                _log.Add(_order.ToString());
                _log.Add($"Order is being shipped to {_order.ShippingAddress}");
            }
        }

        public class OnlineRestaurant
        {
            private readonly List<Order> _cart;

            public OnlineRestaurant()
            {
                _cart = new List<Order>();
            }

            public void AddOrderToCart(Order order)
            {
                _cart.Add(order);
            }

            public void CompleteOrders()
            {
                _log.Add("Orders completed. Dispatch in progress...");
            }
        }

        public class Order
        {
            public string DishName { get; set; }
            public double DishPrice { get; set; }
            public string User { get; set; }
            public string ShippingAddress { get; set; }
            public double ShippingPrice { get; set; }

            public override string ToString()
            {
                return string.Format("User {0} ordered {1}. The full price is {2} dollars.",
                    User, DishName, DishPrice + ShippingPrice);
            }
        }
    }

}