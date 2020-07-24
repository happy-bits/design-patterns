using System.Linq;
using System.Text.RegularExpressions;

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

        private static bool IsNormalPlate(string number) => Regex.IsMatch(number, GetValidPlateRegexPattern());

        private static bool ValidDiplomatLicencePlate(string number) => Regex.IsMatch(number, "[A-Z]{2} \\d\\d\\d [A-Z]");

        private static bool PlateIsReservedForTaxi(string number) => number.EndsWith("T");

        private static string ExcludeLetters(string letters, string lettersToRemove) => string.Join("", letters.Where(c => !lettersToRemove.Contains(c)));

        private static string GetValidPlateRegexPattern()
        {
            var allSwedishLetters = "ABCDEFGHIJKLMNOPQRSTWXYZÅÄÖ";
            var invalidLetters = "IQVÅÄÖ";
            var validLetters = ExcludeLetters(allSwedishLetters, invalidLetters);
            var validLastCharacter = validLetters + "0123456789";
            return "[" + validLetters + "]{3} [0-9][0-9][" + validLastCharacter + "]";
        }

        public Result AddLicensePlate(string number, CustomerType customer)
        {
            if (customer == CustomerType.Diplomat && !ValidDiplomatLicencePlate(number))
                return Result.InvalidFormat;

            if (customer != CustomerType.Diplomat)
            {
                if (!IsNormalPlate(number))
                    return Result.InvalidFormat;

                if (PlateIsReservedForTaxi(number) && customer != CustomerType.Taxi)
                    return Result.InvalidFormat;

                if (PlateIsReservedForAdvertisment(number) && customer != CustomerType.Advertisment)
                    return Result.InvalidFormat;
            }

            if (!_repo.IsAvailable(number))
                return Result.NotAvailable;

            _repo.Save(number);
            return Result.Success;
        }

    }

}
