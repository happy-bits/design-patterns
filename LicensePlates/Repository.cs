using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.LicensePlates
{

   
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


}
