using System;
using System.Text.RegularExpressions;

namespace DesignPatterns.TemplateMethod.Distances.Before
{
    class Client : IClient
    {
        public Result License(string licensnumber)
        {
            var saver = new SaveService();
            return saver.TrySave(licensnumber, SaveType.LicensePlate);
        }

        public Result Product(string productname)
        {
            var saver = new SaveService();
            return saver.TrySave(productname, SaveType.Product);
        }

        enum SaveType
        {
            LicensePlate, Product
        }

        // Nackdel: om fler "SaveTypes" tillkommer så växter denna klass och blir mer och mer avancerad

        class SaveService
        {
            public Result TrySave(string name, SaveType saveType)
            {
                name = CleanBase(name);

                // nackdel: avancerad logik
                if (saveType == SaveType.LicensePlate) 
                    name = LicensePlateClean(name);

                // nackdel: avancerad logik
                if (!IsValidBase(name) ||  
                    saveType == SaveType.LicensePlate && !LicensePlateIsValid(name) ||
                    saveType == SaveType.Product && !ProductIsValid(name)
                )

                {
                    return Result.Invalid;
                }

                // nackdel: avancerad logik

                var success = saveType switch
                {
                    SaveType.Product => ProductSave(name),
                    SaveType.LicensePlate => LicensePlateSave(name),
                    _ => throw new ArgumentException(),// nackdel: detta kan ske i runtime om vi har otur
                };
                return success ? Result.Success : Result.SaveError;
            }

            // Methods that "TrySave" always will call

            private static string CleanBase(string name) => name.Trim();
            private static bool IsValidBase(string name) => !string.IsNullOrEmpty(name);

            // License plate

            private static bool LicensePlateSave(string name)
            {
                // Simulate error when name is 666
                if (name == "AAA 666")
                    return false;

                // Save licenseplate to database
                return true;
            }

            private static string LicensePlateClean(string name) => Regex.Replace(name, " {2,}", " "); // Replace two spaces with one

            private static bool LicensePlateIsValid(string name) => Regex.IsMatch(name, "[A-Z]{3} [0-9]{3}");

            // Products

            private static bool ProductSave(string name)
            {
                // Simulate error when name is 666
                if (name == "666666")
                    return false;

                // Save product to text file
                return true;
            }

            private static bool ProductIsValid(string name) => Regex.IsMatch(name, "[0-9]{6}");

        }

    }
}
