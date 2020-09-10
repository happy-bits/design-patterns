/*

Låter dej packa ihop flera objekt i RAM-minnet genom att dela gemensamma delar
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Flyweight
{
    [TestClass]
    public class Guru
    {

        [TestMethod]
        public void Ex1()
        {
            // The client code usually creates a bunch of pre-populated
            // flyweights in the initialization stage of the application.
            var factory = new FlyweightFactory
                (
                    new Car { Company = "Chevrolet", Model = "Camaro2018", Color = "pink" },
                    new Car { Company = "Mercedes Benz", Model = "C300", Color = "black" },
                    new Car { Company = "Mercedes Benz", Model = "C500", Color = "red" },
                    new Car { Company = "BMW", Model = "M5", Color = "red" },
                    new Car { Company = "BMW", Model = "X6", Color = "white" }
            );
            factory.ListFlyweights();

            AddCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "BMW",
                Model = "M5",
                Color = "red"
            });

            AddCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "BMW",
                Model = "X1",
                Color = "red"
            });

            factory.ListFlyweights();
        }
        // The Flyweight stores a common portion of the state (also called intrinsic
        // state) that belongs to multiple real business entities. The Flyweight
        // accepts the rest of the state (extrinsic state, unique for each entity)
        // via its method parameters.
        class Flyweight
        {
            private Car _sharedState;

            public Flyweight(Car car)
            {
                _sharedState = car;
            }

            public void Operation(Car uniqueState)
            {
                string s = JsonConvert.SerializeObject(_sharedState);
                string u = JsonConvert.SerializeObject(uniqueState);
                Console.WriteLine($"Flyweight: Displaying shared {s} and unique {u} state.");
            }
        }

        // The Flyweight Factory creates and manages the Flyweight objects. It
        // ensures that flyweights are shared correctly. When the client requests a
        // flyweight, the factory either returns an existing instance or creates a
        // new one, if it doesn't exist yet.
        class FlyweightFactory
        {
            private readonly List<Tuple<Flyweight, string>> _flyweights = new List<Tuple<Flyweight, string>>();

            public FlyweightFactory(params Car[] cars)
            {
                foreach (var car in cars)
                {
                    _flyweights.Add(new Tuple<Flyweight, string>(new Flyweight(car), GetKey(car)));
                }
            }

            // Returnera en hash av bilen, t.ex Camaro2018_pink_Checkrolet 
            public static string GetKey(Car car)
            {
                List<string> elements = new List<string>
                {
                    car.Model,
                    car.Color,
                    car.Company
                };

                if (car.Owner != null && car.Number != null)
                {
                    elements.Add(car.Number);
                    elements.Add(car.Owner);
                }

                elements.Sort();

                return string.Join("_", elements);
            }

            // Returns an existing Flyweight with a given state or creates a new
            // one.
            public Flyweight GetFlyweight(Car sharedState)
            {
                string key = GetKey(sharedState);

                if (_flyweights.Where(t => t.Item2 == key).Count() == 0)
                {
                    Console.WriteLine("FlyweightFactory: Can't find a flyweight, creating new one.");
                    _flyweights.Add(new Tuple<Flyweight, string>(new Flyweight(sharedState), key));
                }
                else
                {
                    Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
                }
                return _flyweights.Where(t => t.Item2 == key).FirstOrDefault().Item1;
            }

            public void ListFlyweights()
            {
                var count = _flyweights.Count;
                Console.WriteLine($"\nFlyweightFactory: I have {count} flyweights:");
                foreach (var flyweight in _flyweights)
                {
                    Console.WriteLine(flyweight.Item2);
                }
            }
        }

        class Car
        {
            public string Owner { get; set; }

            public string Number { get; set; }

            public string Company { get; set; }

            public string Model { get; set; }

            public string Color { get; set; }
        }

        static void AddCarToPoliceDatabase(FlyweightFactory factory, Car car)
        {
            Console.WriteLine("\nClient: Adding a car to database.");

            var flyweight = factory.GetFlyweight(new Car
            {
                Color = car.Color,
                Model = car.Model,
                Company = car.Company
            });

            // The client code either stores or calculates extrinsic state and
            // passes it to the flyweight's methods.
            flyweight.Operation(car);
        }
    }
}