
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.Builder.Cars.After
{
    class Client : IClient
    {
        public void ManualBuildTest()
        {
            var builder = new ManualBuilder();
            var director = new Director(builder);
            director.MakeSportsCar();

            Manual manual = builder.GetManual();
            Assert.AreEqual(2, manual.Seats);
            Assert.IsTrue(manual.Engine is SportEngine);
            Assert.IsTrue(manual.TripComputer);
            Assert.IsTrue(manual.GPS);
        }

        public void CarBuilderTest()
        {
            var builder = new CarBuilder();
            var director = new Director(builder);
            director.MakeSportsCar();

            Car car = builder.GetCar();
            Assert.AreEqual(2, car.Seats);
            Assert.IsTrue(car.Engine is SportEngine);
            Assert.IsTrue(car.TripComputer);
            Assert.IsTrue(car.GPS);
        }
    }

    interface IBuilder
    {
        void Reset();
        void SetSeats(int nrOfSeats);
        void SetEngine(Engine engine);
        void SetTripComputer();
        void SetGPS();
    }

    class CarBuilder : IBuilder
    {
        private Car _car;

        public void Reset()
        {
            _car = new Car();
        }

        public Car GetCar()
        {
            Car result = _car;
            
            Reset();

            return result;
        }

        public void SetSeats(int nrOfSeats)
        {
            _car.Seats = nrOfSeats;
        }

        public void SetEngine(Engine engine)
        {
            _car.Engine = engine;
        }

        public void SetTripComputer()
        {
            _car.TripComputer = true;
        }

        public void SetGPS()
        {
            _car.GPS = true;
        }
    }

    class ManualBuilder : IBuilder
    {
        private Manual _manual;

        public void Reset()
        {
            _manual = new Manual();
        }

        public Manual GetManual()
        {
            Manual result = _manual;

            Reset();

            return result;
        }

        public void SetSeats(int nrOfSeats)
        {
            _manual.Seats = nrOfSeats;
        }

        public void SetEngine(Engine engine)
        {
            _manual.Engine = engine;
        }

        public void SetTripComputer()
        {
            _manual.TripComputer = true;
        }

        public void SetGPS()
        {
            _manual.GPS = true;
        }
    }
    
    class Car
    {
        public int Seats { get; set; }
        public Engine Engine { get; set; }
        public bool TripComputer { get; set; }
        public bool GPS { get; set; }
    }

    class Manual
    {
        public int Seats { get; set; }
        public Engine Engine { get; set; }
        public bool TripComputer { get; set; }
        public bool GPS { get; set; }
    }


    class Director
    {
        private IBuilder _builder;

        public Director(IBuilder builder)
        {
            _builder = builder;
        }

        // Kan skapa flera varianter av produkter

        public void MakeSUV()
        {
            throw new NotImplementedException();
        }

        public void MakeSportsCar()
        {
            _builder.Reset();
            _builder.SetSeats(2);
            _builder.SetEngine(new SportEngine());
            _builder.SetTripComputer();
            _builder.SetGPS();
        }
    }

    class Engine
    {

    }
    class SportEngine: Engine
    {

    }
}
