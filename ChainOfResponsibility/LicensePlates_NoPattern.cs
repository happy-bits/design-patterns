
/*

    This exercise is about creating a "RegistrationService" with a method AddLicensePlate.
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

        [TestMethod]
        public void return_Success_when_plate_is_available_and_correct_format()
        {
            _service.AddLicensePlate("ABC 123", CustomerType.Normal);
        }

        [TestMethod]
        public void count_nr_or_plates_to_four_after_four_registred_plates()
        {
            _service.AddLicensePlate("ABC 123", CustomerType.Normal);
            _service.AddLicensePlate("ABC 12B", CustomerType.Normal); // note: the last char don't have to be a number
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
        public void return_InvalidFormat_when_format_is_incorrect(string plate)
        {
            var result = _service.AddLicensePlate(plate, CustomerType.Normal);
            Assert.AreEqual(Result.InvalidFormat, result);
        }

        [TestMethod]
        [DataRow("MLB 123")]
        [DataRow("MLB 456")]
        public void return_OnlyForAdvertisment_when_normal_customer_register_plate_starting_with_MLB(string plate)
        {
            var result = _service.AddLicensePlate(plate, CustomerType.Normal);
            Assert.AreEqual(Result.OnlyForAdvertisment, result);
        }

        [TestMethod]
        public void return_Success_when_advertisment_register_plate_starting_with_MLB()
        {
            var result = _service.AddLicensePlate("MLB 123", CustomerType.Advertisment);
            Assert.AreEqual(1, _service.NrOfRegistredPlates);
            Assert.AreEqual(Result.Success, result);
        }

        [TestMethod]
        public void return_NotAvailable_when_plate_is_already_registered()
        {
            _service.AddLicensePlate("ABC 123", CustomerType.Normal);

            var result = _service.AddLicensePlate("ABC 123", CustomerType.Normal);
            Assert.AreEqual(Result.NotAvailable, result);
            
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

        enum Result
        {
            Success, InvalidFormat, OnlyForAdvertisment, Database, NotAvailable
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
            private static bool PlateHaveCorrectFormat(string number) => Regex.IsMatch(number, GetValidPlateRegexPattern());

            public Result AddLicensePlate(string number, CustomerType customer)
            {
                if (!PlateHaveCorrectFormat(number))
                    return Result.InvalidFormat;

                if (PlateIsReservedForAdvertisment(number) && customer != CustomerType.Advertisment) 
                    return Result.OnlyForAdvertisment;

                if (!_repo.IsAvailable(number))
                    return Result.NotAvailable;

                _repo.Save(number);
                return Result.Success;
            }

            private static string GetValidPlateRegexPattern()
            {
                var allSwedishLetters = "ABCDEFGHIJKLMNOPQRSTWXYZÅÄÖ";
                var invalidLetters = "IQVÅÄÖ";
                var validLetters = string.Join("", allSwedishLetters.Where(c => !invalidLetters.Contains(c)));
                var validLastCharacter = validLetters + "0123456789";
                return "[" + validLetters + "]{3} [0-9][0-9][" + validLastCharacter + "]";
            }
        }

        interface ILicensePlateRepository
        {
            bool IsAvailable(string number);
            void Save(string number);
            int CountRegisteredPlates();
        }

        class RepositoryException: Exception { };

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

        enum CustomerType
        {
            Normal, Advertisment
        }
    }
}
