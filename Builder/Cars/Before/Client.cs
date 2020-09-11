
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.Builder.Cars.Before
{
    class Client : IClient
    {
        public void CreateManual()
        {
            Manual manual = Manual.CreateSportsCar();

            Assert.AreEqual(2, manual.Seats);
            Assert.IsTrue(manual.Engine is SportEngine);
            Assert.IsTrue(manual.TripComputer);
            Assert.IsTrue(manual.GPS);
        }

        public void CreateCar()
        {
            Car car = Car.CreateSportsCar();

            Assert.AreEqual(2, car.Seats);
            Assert.IsTrue(car.Engine is SportEngine);
            Assert.IsTrue(car.TripComputer);
            Assert.IsTrue(car.GPS);
        }
    }

  
    class Car
    {
        public int Seats { get; set; }
        public Engine Engine { get; set; }
        public bool TripComputer { get; set; }
        public bool GPS { get; set; }

        internal static Car CreateSportsCar()
        {
            // Nackdel: denna kod upprepas i Manual-klassen
            return new Car
            {
                Seats = 2,
                Engine = new SportEngine(),
                TripComputer = true,
                GPS = true
            };
        }

    }

    class Manual
    {
        public int Seats { get; set; }
        public Engine Engine { get; set; }
        public bool TripComputer { get; set; }
        public bool GPS { get; set; }

        internal static Manual CreateSportsCar()
        {
            return new Manual
            {
                Seats = 2,
                Engine = new SportEngine(),
                TripComputer = true,
                GPS = true
            };
        }
    }
    class Engine
    {
    }
    class SportEngine : Engine
    {
    }
}
