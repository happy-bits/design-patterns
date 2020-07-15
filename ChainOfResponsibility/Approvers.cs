/*
 Chain of Responsibility

 Variant av något jag hittat på nätet

 Avoid coupling the sender of a request to its receiver 
 by giving more than one object a chance to handle the request. 
 Chain the receiving objects and pass the request along the chain 
 until an object handles it.


 - Ge flera objekt möjligheten att hantera ett request
 - Om inte jag kan ta requestet så skickar jag det vidare
 - Fördel: slipper bindning mellan den som skickar och den som tar emot requestet. Klientkoden kan bestämma ordningen hur requesten hanteras
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.ChainOfResponsibility
{
    [TestClass]
    public class Approvers
    {
        static List<string> _logger = new List<string>();

        [TestMethod]
        public void Ex1()
        {
            /*
               Director can handle purchases below just:   10000
               VicePresident can handle purchases below:   25000
               President can handle purchases below:      100000
              
               Setup Chain of Responsibility
               All are Approvers
            */
            Approver larry = new Director();   // har minst makt
            Approver sam = new VicePresident();
            Approver tammy = new President();

            /*
            Setup chain
            - Larry has Sam as successor (efterträdare). Larry has the least power.
            - Sam has Tammy as successor
            */
            larry.SetSuccessor(sam); 
            sam.SetSuccessor(tammy); 

            // Small purchase is okay for the Director Larry

            Purchase p = new Purchase(1, 50, "Assets"); // Amount 50, Request number 1
            larry.ProcessRequest(p);
            Assert.AreEqual($"Director approved request 1", _logger.Last());

            // Director can't handle => VicePresident cant handle => President can handle

            p = new Purchase(2, 75000, "Project X"); // Amount 75000, Request number 2
            larry.ProcessRequest(p);
            Assert.AreEqual($"President approved request 2", _logger.Last());

            // Too expensive! No one can handle!

            p = new Purchase(3, 2000000, "Project Y"); // Amount 2000000, Request number 3
            larry.ProcessRequest(p);
            Assert.AreEqual($"No one can handle request :(", _logger.Last());

            Assert.AreEqual(3, _logger.Count);

        }

        #region solution

        // The 'Handler' abstract class
        // All Approvers has a "successor" and should be able to process a request

        abstract class Approver
        {
            protected Approver successor;

            public void SetSuccessor(Approver successor)
            {
                this.successor = successor;
            }

            public abstract void ProcessRequest(Purchase purchase);

            protected void Next(Purchase purchase)
            {
                if (successor == null)
                {
                    _logger.Add($"No one can handle request :(");
                }
                else
                { 
                    successor.ProcessRequest(purchase);
                }
            }
        }

        /*
           The 'ConcreteHandler' class
           The approvers can be connected in different ways to each other.
           The Director class don't know which "successor" it has
        */
        class Director : Approver
        {
            public override void ProcessRequest(Purchase purchase)
            {
                if (purchase.Amount < 10000.0)
                {
                    _logger.Add($"{GetType().Name} approved request {purchase.Number}");
                }
                else
                {
                    Next(purchase);
                }


            }
        }

        // The 'ConcreteHandler' class

        class VicePresident : Approver
        {
            public override void ProcessRequest(Purchase purchase)
            {
                if (purchase.Amount < 25000.0)
                {
                    _logger.Add($"{GetType().Name} approved request {purchase.Number}");
                }
                else
                {
                    Next(purchase);
                }

            }
        }

        // The 'ConcreteHandler' class

        class President : Approver
        {
            public override void ProcessRequest(Purchase purchase)
            {
                if (purchase.Amount < 100000.0)
                {
                    _logger.Add($"{GetType().Name} approved request {purchase.Number}");
                }
                else
                {
                    Next(purchase);
                }

            }
        }

        // Simple class holding request details

        class Purchase
        {
            public Purchase(int number, double amount, string purpose)
            {
                Number = number;
                Amount = amount;
                Purpose = purpose;
            }

            public int Number { get; }
            public double Amount { get; }
            public string Purpose { get; }
        }

        #endregion
    }
}
