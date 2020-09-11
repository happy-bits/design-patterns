/*
"Spara RAM-minne genom att inte duplicera feta objekt"

Låter dej packa ihop flera objekt i RAM-minnet genom att dela gemensamma delar

Har ett enda syfte: att minska RAM-minnes användning

Ett objekt kan bestå av en del som alltid/ofta är unik och en del som sällan ändras.

"Extrinsic" state sparas inte längre i objektet (ex trädets textur)
"Intrinsic" state är t.ex trädets koordinat

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
                    new Car { Company = "BMW", Model = "M5", Color = "red" },  // denna bil återkommer längre ner
                    new Car { Company = "BMW", Model = "X6", Color = "white" }
            );
            factory.ListFlyweights();

            AddCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",  // skickas inte in till factoryn ändå
                Owner = "James Doe", // skickas inte in till factoryn ändå
                Company = "BMW",     // samma som ovan
                Model = "M5",        // samma som ovan
                Color = "red"        // samma som ovan
            });

            AddCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "BMW",
                Model = "X1",       // ej samma
                Color = "red"
            });

            factory.ListFlyweights();
        }

        // Flyweight sparar en gemensam del av statet - kallad "intrinsic state" (som tillhör flera business-entiteter) (inre state)
        // Resten av statet kallas "extrinsic state"

        class Flyweight
        {
            private readonly Car _sharedState;

            public Flyweight(Car car)
            {
                _sharedState = car;
            }

            // Gör en operation med denna bilen och en till
            public void Operation(Car uniqueState)
            {
                string s = JsonConvert.SerializeObject(_sharedState);
                string u = JsonConvert.SerializeObject(uniqueState);
                Console.WriteLine($"Flyweight: Displaying shared {s} and unique {u} state.");
            }
        }

        /*
           Fabriken skapar och hanterar Flyweight-objekt
        */
        class FlyweightFactory
        {
            private readonly List<Tuple<Flyweight, string>> _flyweights = new List<Tuple<Flyweight, string>>();

            public FlyweightFactory(params Car[] cars)
            {
                // För varje bil skapa en tuppel av 
                // Flyweight(car), Hash
                foreach (var car in cars)
                {
                    _flyweights.Add(new Tuple<Flyweight, string>(new Flyweight(car), GetKey(car)));
                }
            }

            // Returnera en hash av bilen, t.ex "Camaro2018_pink_Checkrolet"
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

            /*
             Returnerar Flyweight + lägger till den i "_flyweights" om den inte redan finns där
            */
            public Flyweight GetFlyweight(Car car)
            {
                string key = GetKey(car);

                // Item2 = hashade värdet
                // Kolla bland befintliga flyweights om vi redan har den
                if (!_flyweights.Any(t => t.Item2 == key))
                {
                    Console.WriteLine("FlyweightFactory: Can't find a flyweight, creating new one.");
                    _flyweights.Add(new Tuple<Flyweight, string>(new Flyweight(car), key));
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
                    Console.WriteLine(flyweight.Item2); // hashen
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

            // All info om bilen skickas inte med
            var flyweight = factory.GetFlyweight(new Car
            {
                Color = car.Color,
                Model = car.Model,
                Company = car.Company
            });

            flyweight.Operation(car);
        }
    }
}