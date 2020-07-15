
/*

    Note: doesn't use chain of responsibility yet

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DesignPatterns.ChainOfResponsibility
{
    [TestClass]
    public class LicensePlates_NoPattern
    {
        private readonly RegistrationService _service;

        public LicensePlates_NoPattern()
        {
            _service = new RegistrationService(new MockLicensePlateRepository());
        }

        [TestMethod]
        public void register_three_valid_license_plates()
        {
            _service.AddLicensePlate("ABC 123", CustomerType.Normal);
            _service.AddLicensePlate("ABC 12A", CustomerType.Normal);
            _service.AddLicensePlate("XYZ 444", CustomerType.Normal);
            _service.AddLicensePlate("PDF 543", CustomerType.Advertisment);

            Assert.AreEqual(4, _service.NrOfRegistredPlates);
        }

        [TestMethod]
        [DataRow("ABC X23")]
        [DataRow("ÅBC 123")]
        [DataRow("QBC 123")]
        [DataRow("1BC 123")]
        [DataRow("ABC  123")]
        public void throw_InvalidFormatException_when_format_is_incorrect(string incorrectFormat)
        {
            Assert.ThrowsException<RegistrationService.InvalidFormatException>(() =>
            {
                _service.AddLicensePlate(incorrectFormat, CustomerType.Normal);
            });
        }

        [TestMethod]
        public void throw_OnlyForAdvertismentException_when_normal_customer_register_plate_starting_with_MLB()
        {
            Assert.ThrowsException<RegistrationService.OnlyForAdvertismentException>(() =>
            {
                _service.AddLicensePlate("MLB 123", CustomerType.Normal);
            });
        }

        [TestMethod]
        public void register_successful_when_advertisment_want_plate_starting_with_MLB()
        {
            _service.AddLicensePlate("MLB 123", CustomerType.Advertisment);
            Assert.AreEqual(1, _service.NrOfRegistredPlates);
        }

        [TestMethod]
        public void throw_NotAvailableException_when_plate_is_already_registered()
        {
            _service.AddLicensePlate("ABC 123", CustomerType.Normal);

            Assert.ThrowsException<RegistrationService.NotAvailableException>(() =>
            {
                _service.AddLicensePlate("ABC 123", CustomerType.Normal);
            });
        }

        [TestMethod]
        public void throw_DatabaseException_when_unexpected_problem_with_database()
        {
            Assert.ThrowsException<RepositoryException>(() =>
            {
                _service.AddLicensePlate("XXX 666", CustomerType.Normal);
            });

            Assert.ThrowsException<RepositoryException>(() =>
            {
                _service.AddLicensePlate("YYY 666", CustomerType.Normal);
            });
        }

        class RegistrationService
        {
            private readonly ILicensePlateRepository _repo;

            public class InvalidFormatException : Exception { }
            public class OnlyForAdvertismentException : Exception { }
            public class NotAvailableException : Exception { }

            public RegistrationService(ILicensePlateRepository repo)
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

            /*
             
                 Question: Is current code a good solution (with exceptions) or should I create a method that return something e.g an enum:

                        public Result AddLicensePlate(....)
                        {
                        }

                        enum Result
                        {
                            Success, InvalidFormat, OnlyForAdvertisment, Database, NotAvailable
                        }

            */

            // (This will changed with "chain of responsibility pattern")

            public void AddLicensePlate(string number, CustomerType customer)
            {
                if (!Regex.IsMatch(number, ValidRegexPattern))
                    throw new InvalidFormatException();

                if (number.StartsWith("MLB") && customer != CustomerType.Advertisment)
                    throw new OnlyForAdvertismentException();

                if (!_repo.IsAvailable(number))
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

        class RepositoryException : Exception { };

        class MockLicensePlateRepository : ILicensePlateRepository
        {
            private readonly List<string> _registered = new List<string>();

            public int CountRegisteredPlates() => _registered.Count;

            public bool IsAvailable(string number)
            {
                // Simulation of database error is some cases
                if (number == "XXX 666")
                    throw new RepositoryException(); 

                return !_registered.Contains(number);
            }

            public void Save(string number)
            {
                // Simulation of database error is some cases
                if (number == "YYY 666")
                    throw new RepositoryException();

                _registered.Add(number);
            }
        }

        enum CustomerType
        {
            Normal, Advertisment
        }
    }
}
