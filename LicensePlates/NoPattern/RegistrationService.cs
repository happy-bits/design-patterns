using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesignPatterns.LicensePlates.NoPattern
{
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

}
