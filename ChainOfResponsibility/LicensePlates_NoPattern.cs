
/*

    This exercise is about creating a "RegistrationService" with a method "AddLicensePlate".
    It's about trying to add a (swedish) license plates to a repository and handle problems.
    The license plates have to be on a special format.

    Question: what do you think about the general solution to this problem?

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
            _service = new RegistrationService(new FakeLicensePlateRepository());
        }

        // NORMAL CUSTOMER

        [TestMethod]
        [DataRow("ABC 123")] 
        [DataRow("ABC 12B")] // (last character may be a letter)
        public void return_Success_when_plate_has_correct_format(string plate)
        {
            Assert.AreEqual(Result.Success, _service.AddLicensePlate(plate, CustomerType.Normal));
        }

        [TestMethod]
        [DataRow("ABC X23")] 
        [DataRow("ÅBC 123")] // Å is a swedish letter but not a valid letter anyway
        [DataRow("QBC 123")] // Q is not a valid letter (too similair to O)
        [DataRow("1BC 123")]
        [DataRow("ABC  123")]
        public void return_InvalidFormat_when_format_is_incorrect(string plate)
        {
            var result = _service.AddLicensePlate(plate, CustomerType.Normal);
            Assert.AreEqual(Result.InvalidFormat, result);
        }

        [TestMethod]
        [DataRow("MLB 123")]
        [DataRow("MLB 456")]
        public void return_OnlyForAdvertisment_when_non_advertisment_try_register_plate_starting_with_MLB(string plate)
        {
            var result = _service.AddLicensePlate(plate, CustomerType.Normal);
            Assert.AreEqual(Result.OnlyForAdvertisment, result);
        }

        [TestMethod]
        [DataRow("ABC 12T")]
        [DataRow("CCC 77T")]
        public void return_OnlyForTaxi_when_plate_ends_with_T_and_customer_isnot_taxi(string plate)
        {
            var result = _service.AddLicensePlate(plate, CustomerType.Normal);
            Assert.AreEqual(Result.OnlyForTaxi, result);
        }

        [TestMethod]
        public void return_NotAvailable_when_plate_is_already_registered()
        {
            _service.AddLicensePlate("ABC 123", CustomerType.Normal);

            var result = _service.AddLicensePlate("ABC 123", CustomerType.Normal);
            Assert.AreEqual(Result.NotAvailable, result);
        }

        // ADVERTISMENT CUSTOMER

        [TestMethod]
        [DataRow("ABC 123")]
        [DataRow("ABC 12B")]
        [DataRow("MLB 123")]
        [DataRow("MLB 456")]
        public void return_Success_when_plate_has_correct_format_for_advertisment(string plate)
        {
            var result = _service.AddLicensePlate(plate, CustomerType.Advertisment);

            Assert.AreEqual(Result.Success, result);
        }

        [TestMethod]
        [DataRow("ABC X23")]
        [DataRow("ÅBC 123")]
        [DataRow("QBC 123")]
        [DataRow("1BC 123")]
        [DataRow("ABC  123")]
        public void return_InvalidFormat_when_format_is_incorrect_for_advertisment(string plate)
        {
            var result = _service.AddLicensePlate(plate, CustomerType.Normal);
            Assert.AreEqual(Result.InvalidFormat, result);
        }

        // TAXI CUSTOMER

        [DataRow("ABC 12T")]
        [DataRow("CCC 77T")]
        public void return_Success_when_plate_has_correct_format_for_taxi(string plate)
        {
            Assert.AreEqual(Result.Success, _service.AddLicensePlate(plate, CustomerType.Taxi));
        }

        [TestMethod]
        [DataRow("ABC X23")]
        [DataRow("ÅBC 123")]
        [DataRow("QBC 123")]
        [DataRow("1BC 123")]
        [DataRow("ABC  123")]
        public void return_InvalidFormat_when_format_is_incorrect_for_taxi(string plate)
        {
            var result = _service.AddLicensePlate(plate, CustomerType.Taxi);
            Assert.AreEqual(Result.InvalidFormat, result);
        }

        // DIPLOMAT

        [TestMethod]
        [DataRow("AA 111 A")]
        [DataRow("CD 777 X")]
        public void return_Success_when_plate_has_correct_format_for_diplomats(string plate)
        {
            Assert.AreEqual(Result.Success, _service.AddLicensePlate(plate, CustomerType.Diplomat));
        }

        [TestMethod]
        [DataRow("ABC 123")]
        [DataRow("ABC 12B")]
        [DataRow("A8 111 A")]
        [DataRow("CD A77 X")]
        public void return_InvalidFormat_when_format_is_incorrect_for_diplomat(string plate)
        {
            var result = _service.AddLicensePlate(plate, CustomerType.Diplomat);
            Assert.AreEqual(Result.InvalidFormat, result);
        }

        // EXCEPTIONS

        [TestMethod]
        public void throw_RepositoryException_when_unexpected_problem_with_database()
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

        enum Result
        {
            Success, InvalidFormat, OnlyForAdvertisment, OnlyForTaxi, NotAvailable
        }

        class RegistrationService
        {
            private readonly ILicensePlateRepository _repo;

            public RegistrationService(ILicensePlateRepository repo)
            {
                _repo = repo;
            }

            public int NrOfRegistredPlates => _repo.CountRegisteredPlates();

            private static bool PlateIsReservedForAdvertisment(string number) => number.StartsWith("MLB");

            public Result AddLicensePlate(string number, CustomerType customer)
            {
                if (customer == CustomerType.Diplomat)
                {
                    if (!ValidDiplomatLicencePlate(number))
                        return Result.InvalidFormat;
                }
                else
                {
                    if (!ValidNormalLicensePlate(number))
                        return Result.InvalidFormat;

                    if (PlateIsReservedForTaxi(number) && customer != CustomerType.Taxi)
                        return Result.OnlyForTaxi;

                    if (PlateIsReservedForAdvertisment(number) && customer != CustomerType.Advertisment)
                        return Result.OnlyForAdvertisment;
                }

                if (!_repo.IsAvailable(number))
                    return Result.NotAvailable;

                _repo.Save(number);
                return Result.Success;
            }

            private static bool ValidNormalLicensePlate(string number) => Regex.IsMatch(number, GetValidPlateRegexPattern());

            /*
             First two letters: country code
             Three numbers: the embassy's serial number 
             Last letter: the ambassadors rank
            */
            private static bool ValidDiplomatLicencePlate(string number) => Regex.IsMatch(number, "[A-Z]{2} \\d\\d\\d [A-Z]");

            private static bool PlateIsReservedForTaxi(string number) => number.EndsWith("T");

            private static string GetValidPlateRegexPattern()
            {
                var allSwedishLetters = "ABCDEFGHIJKLMNOPQRSTWXYZÅÄÖ";
                var invalidLetters = "IQVÅÄÖ";
                var validLetters = ExcludeLetters(allSwedishLetters, invalidLetters); 
                var validLastCharacter = validLetters + "0123456789";
                return "[" + validLetters + "]{3} [0-9][0-9][" + validLastCharacter + "]";
            }

            private static string ExcludeLetters(string letters, string lettersToRemove) => string.Join("", letters.Where(c => !lettersToRemove.Contains(c)));

        }

        interface ILicensePlateRepository
        {
            bool IsAvailable(string number);
            void Save(string number);
            int CountRegisteredPlates();
        }

        class RepositoryException : Exception { };

        class FakeLicensePlateRepository : ILicensePlateRepository
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

        public enum CustomerType
        {
            Normal, Advertisment, Taxi, Diplomat
        }
    }
}
