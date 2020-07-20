/*

STATE DESIGN PATTERN - https://www.dofactory.com/net/state-design-pattern
(Written as a test)

*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.StateDesignPattern
{
    [TestClass]
    public class DoFactoryAccount
    {
        [TestMethod]
        public void Ex1()
        {
            /*
            Klienten behöver inte veta att det finns klasser som RedState, SilverState, GoldState
            */

            // Open a new account
            Account account = new Account("Jim");

            Assert.AreEqual("Jim", account.Owner);

            // Apply financial transactions

            account.Deposit(500.0);

            AssertAndClearLog(new[] {
                    "Deposited: 500",
                    "Balance: 500",
                    "Status: Silver"
            });

            account.Deposit(300.0);

            AssertAndClearLog(new[] {
                    "Deposited: 300",
                    "Balance: 800",
                    "Status: Silver"
            });

            account.Deposit(550.0);

            AssertAndClearLog(new[] {
                    "Deposited: 550",
                    "Balance: 1350",
                    "Status: Gold"        // Status ändrat till Gold
            });

            account.PayInterest();

            AssertAndClearLog(new[] {
                    "Interest Paid",
                    "Balance: 1417.5",           // 5% ränta som guldkund
                    "Status: Gold"
            });

            account.Withdraw(2000.00);

            AssertAndClearLog(new[] {
                    "Withdrew: 2000",
                    "Balance: -582.5",
                    "Status: Red"            // Status ändrad till Red
            });

            account.Withdraw(1100.00);

            AssertAndClearLog(new[] {
                    "No funds available for withdrawal!",
                    "Withdrew: 1100",
                    "Balance: -582.5",
                    "Status: Red"
            });
        }

        private void AssertAndClearLog(string[] messages)
        {
            CollectionAssert.AreEqual(messages, _log);
            ClearLog();
        }

        private void ClearLog()
        {
            _log = new List<string>();
        }

        // The 'State' abstract class

        abstract class State
        {
            protected double _interest;
            protected double _lowerLimit;
            protected double _upperLimit;

            public Account Account { get; set; }

            public double Balance { get; protected set; }

            public abstract void Deposit(double amount);
            public abstract void Withdraw(double amount);
            public abstract void PayInterest();
        }


        /// A 'ConcreteState' class
        /// Red indicates that account is overdrawn 

        class Red : State
        {
            public Red(State state)
            {
                Balance = state.Balance;
                Account = state.Account;
                Initialize();
            }

            private void Initialize()
            {
                // Should come from a datasource
                _interest = 0.0;
                _lowerLimit = -100.0;
                _upperLimit = 0.0;
            }

            public override void Deposit(double amount)
            {
                Balance += amount;
                StateChangeCheck();
            }

            public override void Withdraw(double amount)
            {
                _log.Add("No funds available for withdrawal!");
            }

            public override void PayInterest()
            {
                // No interest is paid
            }

            private void StateChangeCheck()
            {
                if (Balance > _upperLimit)
                {
                    Account.State = new Silver(this);
                }
            }
        }

        /// A 'ConcreteState' class
        /// Silver indicates a non-interest bearing state

        class Silver : State
        {
            // Overloaded constructors

            public Silver(State state) : this(state.Balance, state.Account)
            {
            }

            public Silver(double balance, Account account)
            {
                Balance = balance;
                Account = account;
                Initialize();
            }

            private void Initialize()
            {
                // Should come from a datasource

                _interest = 0.0;
                _lowerLimit = 0.0;
                _upperLimit = 1000.0;
            }

            public override void Deposit(double amount)
            {
                Balance += amount;
                StateChangeCheck();
            }

            public override void Withdraw(double amount)
            {
                Balance -= amount;
                StateChangeCheck();
            }

            public override void PayInterest()
            {
                Balance += _interest * Balance;
                StateChangeCheck();
            }

            // Om kunden satt in mycket pengar => ändra state till GoldState
            private void StateChangeCheck()
            {
                if (Balance < _lowerLimit)
                {
                    Account.State = new Red(this);
                }
                else if (Balance > _upperLimit)
                {
                    Account.State = new Gold(this);
                }
            }
        }

        // A 'ConcreteState' class
        // Gold indicates an interest bearing state

        class Gold : State

        {
            // Overloaded constructors

            public Gold(State state) : this(state.Balance, state.Account)
            {
            }

            public Gold(double balance, Account account)
            {
                Balance = balance;
                Account = account;
                Initialize();
            }

            private void Initialize()
            {
                // Should come from a database

                _interest = 0.05;
                _lowerLimit = 1000.0;
                _upperLimit = 10000000.0;
            }

            public override void Deposit(double amount)
            {
                Balance += amount;
                StateChangeCheck();
            }

            public override void Withdraw(double amount)
            {
                Balance -= amount;
                StateChangeCheck();
            }

            public override void PayInterest()
            {
                Balance += _interest * Balance;
                StateChangeCheck();
            }

            // Här har vi chans att byta state
            // "this" skickas bara med för att få med "balance" och en hänvisning till Account
            private void StateChangeCheck()
            {
                if (Balance < 0.0)
                {
                    Account.State = new Red(this);
                }
                else if (Balance < _lowerLimit)
                {
                    Account.State = new Silver(this);
                }
            }
        }

        static List<string> _log = new List<string>();

        /*
           The 'Context' class
           Ett konto kan ha olika State: RedState, SilverState eller GoldState. De är olika klasser
           
           Alla State har balans, ränta och gränser. De lovar att implementera metoder för att ta ut och sätta in pengar

           State't är också kopplat till Account'et. Så de hänvisar till varandra.

           Denna klass har bara "_owner" som ett eget fält. Sedan hänvisar klassen bara till State'ts metoder
        */
        class Account
        {
            public Account(string owner)
            {
                // New accounts are 'Silver' by default

                Owner = owner;
                State = new Silver(0.0, this);
            }

            public string Owner { get; }
            public double Balance
            {
                get { return State.Balance; }
            }

            public State State { get; set; }

            public void Deposit(double amount)
            {
                State.Deposit(amount);  // Anropas t.ex Deposit på SilverState
                _log.Add($"Deposited: {amount}");
                _log.Add($"Balance: {Balance}");
                _log.Add($"Status: {State.GetType().Name}");
            }

            public void Withdraw(double amount)
            {
                State.Withdraw(amount);
                _log.Add($"Withdrew: {amount}");
                _log.Add($"Balance: {Balance}");
                _log.Add($"Status: {State.GetType().Name}");
            }

            public void PayInterest()
            {
                State.PayInterest();
                _log.Add($"Interest Paid");
                _log.Add($"Balance: {Balance}");
                _log.Add($"Status: {State.GetType().Name}");
            }
        }
    }
}
