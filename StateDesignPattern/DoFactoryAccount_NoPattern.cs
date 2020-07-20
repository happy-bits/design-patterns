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
    public class DoFactoryAccount_NoPattern
    {
        static List<string> _log = new List<string>();

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

        enum State
        {
            Red, Silver, Gold
        }

        class Account     
        {
            private readonly string _owner;
            private double _balance;
            public Account(string owner)
            {
                _owner = owner;
                State = State.Silver;
            }

            public State State { get; private set; }

            public void Deposit(double amount)
            {
                _balance += amount;
                StateChangeCheck();
                _log.Add($"Deposited: {amount}");
                _log.Add($"Balance: {_balance}");
                _log.Add($"Status: {State}");
            }

            public void Withdraw(double amount)
            {
                if (State == State.Red)
                {
                    _log.Add("No funds available for withdrawal!");
                } 
                else
                {
                    _balance -= amount;
                }
                StateChangeCheck();
                _log.Add($"Withdrew: {amount}");
                _log.Add($"Balance: {_balance}");
                _log.Add($"Status: {State}");
            }

            public void PayInterest()
            {
                double interest = 0;
                if (State == State.Gold)
                    interest = 0.05;
                StateChangeCheck();
                _balance += interest * _balance;
                _log.Add($"Interest Paid");
                _log.Add($"Balance: {_balance}");
                _log.Add($"Status: {State}");

            }
            private void StateChangeCheck()
            {
                if (_balance < 0)
                    State = State.Red;
                else if (_balance < 1000)
                    State = State.Silver;
                else
                    State = State.Gold;

            }
        }
    }
}
