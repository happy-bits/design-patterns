
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.ChainOfResponsibility
{
    [TestClass]
    public class Approvers_NoPattern
    {
        static List<string> _logger = new List<string>();

        [TestMethod]
        public void Ex1()
        {
            /*
               Director can handle purchases below just:   10000
               VicePresident can handle purchases below:   25000
               President can handle purchases below:      100000
              
            */
            Approver larry = new Director();   // har minst makt
            Approver sam = new VicePresident();
            Approver tammy = new President();

            var client = new Client();

            client.SetupChain(larry, sam, tammy);

            client.ProcessPurchase(new Purchase(1, 50, "Assets"));      
            client.ProcessPurchase(new Purchase(2, 12000, "Project X"));  
            client.ProcessPurchase(new Purchase(3, 75000, "Project Y"));
            client.ProcessPurchase(new Purchase(4, 2000000, "Project Z"));

            CollectionAssert.AreEqual(new[] {

            "Director approved request 1",
            "VicePresident approved request 2",
            "President approved request 3",
            "No one can handle request :(",

            }, 
            _logger);
        }

        #region solution

        class Client
        {
            private Approver[] _approvers;

            internal void ProcessPurchase(Purchase p)
            {
                foreach (var approver in _approvers)
                {
                    if (approver.CanProcessRequest(p))
                    {
                        approver.ProcessRequest(p);
                        return;
                    }

                }
                _logger.Add("No one can handle request :(");
            }

            internal void SetupChain(params Approver[] approvers)
            {
                _approvers = approvers;
            }

        }

        abstract class Approver
        {
            abstract internal bool CanProcessRequest(Purchase p);
            abstract internal void ProcessRequest(Purchase p);
        }

        class Director : Approver
        {
            internal override bool CanProcessRequest(Purchase p)
            {
                return p.Amount < 10000;
            }

            internal override void ProcessRequest(Purchase p)
            {
                _logger.Add($"Director approved request {p.Number}");
            }
        }

        class VicePresident : Approver
        {
            internal override bool CanProcessRequest(Purchase p)
            {
                return p.Amount < 25000;
            }

            internal override void ProcessRequest(Purchase p)
            {
                _logger.Add($"VicePresident approved request {p.Number}");
            }
        }


        class President : Approver
        {
            internal override bool CanProcessRequest(Purchase p)
            {
                return p.Amount < 100000;
            }

            internal override void ProcessRequest(Purchase p)
            {
                _logger.Add($"President approved request {p.Number}");
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
