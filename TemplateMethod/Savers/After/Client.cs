using System.Text.RegularExpressions;

namespace DesignPatterns.TemplateMethod.Savers.After
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
            public Result TrySave(string name)
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

            // Subclasses have to implement "Save"

            public abstract bool Save(string name);

            // "Hooks"

            protected virtual string Clean(string name) => name;
            protected virtual bool IsValid(string name) => true;

            // Methods that "TrySave" always will call

            private static string CleanBase(string name) => name.Trim();
            private static bool IsValidBase(string name) => !string.IsNullOrEmpty(name);

        }

        class LicensePlateService : SaveService
        {
            public override bool Save(string name)
            {
                // Simulate error when name is 666
                if (name == "AAA 666")
                    return false;

                // Save licenseplate to database
                return true;
            }

            protected override string Clean(string name) => Regex.Replace(name, " {2,}", " "); // Replace two spaces with one

            protected override bool IsValid(string name) => Regex.IsMatch(name, "[A-Z]{3} [0-9]{3}");
        }

        class ProductService : SaveService
        {
            public override bool Save(string name)
            {
                // Simulate error when name is 666
                if (name == "666666")
                    return false;

                // Save product to text file
                return true;
            }

            // (struntar i att implementer Clean i detta fall)

            protected override bool IsValid(string name) => Regex.IsMatch(name, "[0-9]{6}");

        }

    }
}
