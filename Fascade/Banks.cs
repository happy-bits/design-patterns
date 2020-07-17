/*
   Facade Design Pattern.
  
   Från https://www.dofactory.com/net/facade-design-pattern
  
   Fasaden ger ett förenat interface till en mängd subsystem

   Fasaden hanterar livscykeln på sina subsystemen

   Fascaden känns igen för den har ett enkelt interface. Du slipper hantera de komplex subsystemen.

   Förenklar för konsumenten (som inte behöver känna till de andra klasserna)
*/
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.Fascade
{
    [TestClass]
    public class Demo1_Bank
    {

        [TestMethod]
        public void Ex1()
        {
            // Mortgage använder klasserna Bank, Loan och Credit (detta behöver inte vi känna till härifrån)

            // Facade
            Mortgage mortgage = new Mortgage();

            // Evaluate mortgage eligibility for customer

            Customer customer = new Customer(1, "Ann McKinsey"); // superenkel klass
            bool eligible = mortgage.IsEligible(customer, 5000);

            Assert.IsTrue(eligible);
        }

        // Customer class

        class Customer
        {
            public Customer(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public string Name { get; }
            public int Id { get; }
        }

        // Kolla att kunden har tillräckligt sparat och inga dåliga lån och bra kreditvärdighet

        // The 'Facade' class

        class Mortgage
        {
            private Bank _bank = new Bank();
            private Loan _loan = new Loan();
            private Credit _credit = new Credit();

            public bool IsEligible(Customer cust, int amount)
            {
                // Check creditworthyness of applicant

                if (!_bank.HasSufficientSavings(cust, amount))
                    return false;
                else if (!_loan.HasNoBadLoans(cust))
                    return false;
                else if (!_credit.HasGoodCredit(cust))
                    return false;

                return true;
            }
        }

        // The 'Subsystem ClassA' class

        class Bank
        {
            public bool HasSufficientSavings(Customer c, int amount)
            {
                if (c.Id == 1 && amount < 10000)
                    return true;
                return false;
            }
        }

        // The 'Subsystem ClassB' class

        class Credit
        {
            public bool HasGoodCredit(Customer c)
            {
                if (c.Id == 1)
                    return true;
                return false;
            }
        }

        // The 'Subsystem ClassC' class

        class Loan
        {
            public bool HasNoBadLoans(Customer c)
            {
                if (c.Id == 1)
                    return true;
                return false;
            }
        }
    }
}
