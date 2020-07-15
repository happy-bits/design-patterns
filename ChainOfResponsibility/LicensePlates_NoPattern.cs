using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesignPatterns.ChainOfResponsibility
{
    [TestClass]
    public class LicensePlates_NoPattern
    {
        static LicenePlateRegistration Registration => new LicenePlateRegistration(new MockLicensePlateRepository());

        [TestMethod]
        public void abc_123_should_be_registered()
        {
            var r = Registration;

            Assert.AreEqual(2, r.NrOfRegistredPlates); // trivial test

            r.TryRegister("ABC 123", Customer.Normal);
            r.TryRegister("XYZ 444", Customer.Normal);
            r.TryRegister("PDF 543", Customer.Advertisment);

            Assert.AreEqual(5, r.NrOfRegistredPlates);
        }

        [TestMethod]
        [DataRow("ABC X23")]
        [DataRow("1BC 123")]
        [DataRow("ABC  123")]
        public void incorrect_format_should_throw_InvalidFormatException(string incorrectFormat)
        {
            Assert.ThrowsException<LicenePlateRegistration.InvalidFormatException>(() =>
            {
                Registration.TryRegister(incorrectFormat, Customer.Normal);
            });
        }

        [TestMethod]
        public void normal_customer_should_not_register_numbers_starting_with_MLB()
        {
            Assert.ThrowsException<LicenePlateRegistration.OnlyForAdvertismentException>(() =>
            {
                Registration.TryRegister("MLB 123", Customer.Normal);
            });
        }

        [TestMethod]
        public void advertisment_customer_may_register_numbers_starting_with_MLB()
        {
            Registration.TryRegister("MLB 123", Customer.Advertisment);
        }

        [TestMethod]
        public void not_available_numbers_should_throw_NotAvailableException()
        {
            Assert.ThrowsException<LicenePlateRegistration.NotAvailableException>(() =>
            {
                Registration.TryRegister("CCC 777", Customer.Normal);
            });
        }

        [TestMethod]
        public void database_exception_should_be_thrown_if_problem_with_database()
        {
            Assert.ThrowsException<LicenePlateRegistration.DatabaseException>(() =>
            {
                Registration.TryRegister("AAA 666", Customer.Normal);
            });
        }

        class LicenePlateRegistration
        {
            private readonly ILicensePlateRepository _repo;

            public class InvalidFormatException : Exception { }
            public class OnlyForAdvertismentException : Exception { }
            public class NotAvailableException : Exception { }
            public class DatabaseException : Exception { }

            public LicenePlateRegistration(ILicensePlateRepository repo)
            {
                _repo = repo;
            }

            private static string ValidRegexPattern
            {
                get
                {
                    var allSwedishLetters = "ABCDEFGHIJKLMNOPQRSTWXYZÅÄÖ";
                    var invalidLetters = "IQVÅÄÖ";
                    var validLetters = string.Join("", allSwedishLetters.Where(c => !invalidLetters.Contains(c)));
                    var validLastCharacter = validLetters + "0123456789";
                    return "[" + validLetters + "]{3} [0-9][0-9][" + validLastCharacter + "]";
                }
            }

            public int NrOfRegistredPlates => _repo.CountRegisteredPlates();

            public void TryRegister(string number, Customer customer)
            {
                if (!Regex.IsMatch(number, ValidRegexPattern))
                    throw new InvalidFormatException();

                if (number.StartsWith("MLB") && customer != Customer.Advertisment)
                    throw new OnlyForAdvertismentException();

                bool isAvailable;

                try
                {
                    isAvailable = _repo.IsAvailable(number);
                }
                catch
                {
                    throw new DatabaseException();
                }

                if (!isAvailable)
                    throw new NotAvailableException();

                _repo.Save(number);

            }
        }

        interface ILicensePlateRepository
        {
            bool IsAvailable(string number);
            void Save(string number);
            int CountRegisteredPlates();
        }

        class MockLicensePlateRepository : ILicensePlateRepository
        {
            readonly List<string> _alreadyRegistered = new List<string> {
                    "AAA 123",
                    "CCC 777",
                };

            public int CountRegisteredPlates() => _alreadyRegistered.Count;

            public bool IsAvailable(string number)
            {
                // Simulation of database error is some cases
                if (number == "AAA 666")
                    throw new Exception();

                return !_alreadyRegistered.Contains(number);
            }

            public void Save(string number)
            {
                // Simulation of database error is some cases
                if (number == "BBB 666")
                    throw new Exception();

                _alreadyRegistered.Add(number);
            }
        }

        enum Customer
        {
            Normal, Advertisment
        }
    }
}
