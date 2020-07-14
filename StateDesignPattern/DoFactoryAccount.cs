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
            Account account = new Account("Jim Johnson");

            // Apply financial transactions

            account.Deposit(500.0);

            AssertAndClearLog(new[] {
                    "Deposited: 500",
                    "Balance: 500",
                    "Status: SilverState"
            });

            account.Deposit(300.0);

            AssertAndClearLog(new[] {
                    "Deposited: 300",
                    "Balance: 800",
                    "Status: SilverState"
            });

            account.Deposit(550.0);

            AssertAndClearLog(new[] {
                    "Deposited: 550",
                    "Balance: 1350",
                    "Status: GoldState"
            });

            account.PayInterest();

            AssertAndClearLog(new[] {
                    "Interest Paid",
                    "Balance: 1417.5",           // 5% ränta som guldkund
                    "Status: GoldState"
            });

            account.Withdraw(2000.00);

            AssertAndClearLog(new[] {
                    "Withdrew: 2000",
                    "Balance: -582.5",
                    "Status: RedState"
            });

            account.Withdraw(1100.00);

            AssertAndClearLog(new[] {
                    "No funds available for withdrawal!",
                    "Withdrew: 1100",
                    "Balance: -582.5",
                    "Status: RedState"
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
            protected Account account;
            protected double balance;

            protected double interest;
            protected double lowerLimit;
            protected double upperLimit;

            // Properties

            public Account Account
            {
                get { return account; }
                set { account = value; }
            }

            public double Balance
            {
                get { return balance; }
                set { balance = value; }
            }

            public abstract void Deposit(double amount);
            public abstract void Withdraw(double amount);
            public abstract void PayInterest();
        }


        /// A 'ConcreteState' class
        /// Red indicates that account is overdrawn 

        class RedState : State  // **Won**
        {
            private double _serviceFee;

            public RedState(State state)
            {
                balance = state.Balance;
                account = state.Account;
                Initialize();
            }

            private void Initialize()
            {
                // Should come from a datasource
                interest = 0.0;
                lowerLimit = -100.0;
                upperLimit = 0.0;
                _serviceFee = 15.00;
            }

            public override void Deposit(double amount)
            {
                balance += amount;
                StateChangeCheck();
            }

            public override void Withdraw(double amount)
            {
                amount = amount - _serviceFee;
                _log.Add("No funds available for withdrawal!");
            }

            public override void PayInterest()
            {
                // No interest is paid
            }

            private void StateChangeCheck()
            {
                if (balance > upperLimit)
                {
                    account.State = new SilverState(this);
                }
            }
        }

        /// A 'ConcreteState' class
        /// Silver indicates a non-interest bearing state

        class SilverState : State
        {
            // Overloaded constructors

            public SilverState(State state) : this(state.Balance, state.Account)
            {
            }

            public SilverState(double balance, Account account)
            {
                this.balance = balance;
                this.account = account;
                Initialize();
            }

            private void Initialize()
            {
                // Should come from a datasource

                interest = 0.0;
                lowerLimit = 0.0;
                upperLimit = 1000.0;
            }

            public override void Deposit(double amount)
            {
                balance += amount;
                StateChangeCheck();
            }

            public override void Withdraw(double amount)
            {
                balance -= amount;
                StateChangeCheck();
            }

            public override void PayInterest()
            {
                balance += interest * balance;
                StateChangeCheck();
            }

            // Om kunden satt in mycket pengar => ändra state till GoldState
            private void StateChangeCheck()
            {
                if (balance < lowerLimit)
                {
                    account.State = new RedState(this);
                }
                else if (balance > upperLimit)
                {
                    account.State = new GoldState(this);
                }
            }
        }

        // A 'ConcreteState' class
        // Gold indicates an interest bearing state

        class GoldState : State

        {
            // Overloaded constructors

            public GoldState(State state) : this(state.Balance, state.Account)
            {
            }

            public GoldState(double balance, Account account)
            {
                this.balance = balance;
                this.account = account;
                Initialize();
            }

            private void Initialize()
            {
                // Should come from a database

                interest = 0.05;
                lowerLimit = 1000.0;
                upperLimit = 10000000.0;
            }

            public override void Deposit(double amount)
            {
                balance += amount;
                StateChangeCheck();
            }

            public override void Withdraw(double amount)
            {
                balance -= amount;
                StateChangeCheck();
            }

            public override void PayInterest()
            {
                balance += interest * balance;
                StateChangeCheck();
            }

            // Här har vi chans att byta state
            // "this" skickas bara med för att få med "balance" och en hänvisning till Account
            private void StateChangeCheck()
            {
                if (balance < 0.0)
                {
                    account.State = new RedState(this);
                }
                else if (balance < lowerLimit)
                {
                    account.State = new SilverState(this);
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
        class Account      // ***Hangman***
        {
            private readonly string _owner;

            public Account(string owner)
            {
                // New accounts are 'Silver' by default

                _owner = owner;
                State = new SilverState(0.0, this);
            }

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
