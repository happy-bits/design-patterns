/*
 
 Alternativ med två klasser

 Liknar "After"-lösningen bättre

 Fördel: om en ny save-typ tillkommer så växer inte någon klass

 Nackdel: de två "TrySave"-metoderna liknar varandra mycket (upprepning av kod)

 */
using System.Text.RegularExpressions;

namespace DesignPatterns.TemplateMethod.Savers.Before2
{
    class Client : IClient
    {
        public Result License(string licensnumber)
        {
            var saver = new LicensePlateService();
            return saver.TrySave(licensnumber);
        }

        public Result Product(string productname)
        {
            var saver = new ProductService();
            return saver.TrySave(productname);
        }

        public abstract class SaveService
        {

            public abstract Result TrySave(string name);

            // Methods that "TrySave" always will call

            protected static string CleanBase(string name) => name.Trim();
            protected static bool IsValidBase(string name) => !string.IsNullOrEmpty(name);

        }

        class LicensePlateService : SaveService
        {
            public override Result TrySave(string name)
            {
                name = CleanBase(name);
                name = Clean(name);

                if (!IsValidBase(name) || !IsValid(name))
                {
                    return Result.Invalid;
                }

                bool success = Save(name);

                return success ? Result.Success : Result.SaveError;
            }

            private bool Save(string name)
            {
                // Simulate error when name is 666
                if (name == "AAA 666")
                    return false;

                // Save licenseplate to database
                return true;
            }

            private string Clean(string name) => Regex.Replace(name, " {2,}", " "); // Replace two spaces with one

            private bool IsValid(string name) => Regex.IsMatch(name, "[A-Z]{3} [0-9]{3}");
        }

        class ProductService : SaveService
        {
            public override Result TrySave(string name)
            {
                name = CleanBase(name);
                // name = Clean(name); (ingen clean sker här)

                if (!IsValidBase(name) || !IsValid(name))
                {
                    return Result.Invalid;
                }

                bool success = Save(name);

                return success ? Result.Success : Result.SaveError;
            }

            private bool Save(string name)
            {
                // Simulate error when name is 666
                if (name == "666666")
                    return false;

                // Save product to text file
                return true;
            }

            private bool IsValid(string name) => Regex.IsMatch(name, "[0-9]{6}");

        }

    }
}
