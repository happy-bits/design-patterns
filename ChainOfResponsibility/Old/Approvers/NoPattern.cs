using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.ChainOfResponsibility.Approvers
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

            public void ProcessPurchase(Purchase p)
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

            public void SetupChain(params Approver[] approvers)
            {
                _approvers = approvers;
            }

        }

        abstract class Approver
        {
            abstract public bool CanProcessRequest(Purchase p);
            abstract public void ProcessRequest(Purchase p);
        }

        class Director : Approver
        {
            public override bool CanProcessRequest(Purchase p)
            {
                return p.Amount < 10000;
            }

            public override void ProcessRequest(Purchase p)
            {
                _logger.Add($"Director approved request {p.Number}");
            }
        }

        class VicePresident : Approver
        {
            public override bool CanProcessRequest(Purchase p)
            {
                return p.Amount < 25000;
            }

            public override void ProcessRequest(Purchase p)
            {
                _logger.Add($"VicePresident approved request {p.Number}");
            }
        }


        class President : Approver
        {
            public override bool CanProcessRequest(Purchase p)
            {
                return p.Amount < 100000;
            }

            public override void ProcessRequest(Purchase p)
            {
                _logger.Add($"President approved request {p.Number}");
            }
        }

        #endregion
    }
}
