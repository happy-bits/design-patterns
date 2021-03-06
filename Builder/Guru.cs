﻿/*
"Skapa inkompatibla objekt på samma sätt"

Du kan konstruera komplexa objekt steg för steg.

Du kan producera flera typer av objekt genom samma "construction code"

Det är bara meningsfullt att använda builder-pattern om dina produkter är rätt komplexa och kräver avancerad konfiguration. Många fält och många nästade objekt. (codesmell: konstruktorer med en massa parameterar och många är null i många fall)

Fördel med builder pattern: du anropar bara de builder's som du behöver

Olika konkreta builders kan skapa orelaterade produkter (detta skiljer mönstret fårn andra creational-mönster)

*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.Builder
{
    [TestClass]
    public class Guru
    {

        [TestMethod]
        public void Ex1()
        {
            // Buildern skickas till "directorn"
            var director = new Director();
            var builder = new ConcreteBuilder();
            director.Builder = builder; 

            // Initiera konstruktionsprocessen
            Console.WriteLine("Standard basic product:");
            director.BuildMinimalViableProduct();

            // Slutresultatet kommer från byggobjektet
            Console.WriteLine(builder.GetProduct().ListParts()); 

            // Skapa något mer avancerat

            Console.WriteLine("Standard full featured product:");
            director.BuildFullFeaturedProduct();

            // Samma kod som innan för att generera resultat
            Console.WriteLine(builder.GetProduct().ListParts());

            // Det går att använda "builder pattern" utan "director"
            Console.WriteLine("Custom product:");
            builder.BuildPartA();
            builder.BuildPartC();
            Console.Write(builder.GetProduct().ListParts());
        }

        /*

        Standard basic product:
        Product parts: PartA1

        Standard full featured product:
        Product parts: PartA1, PartB1, PartC1

        Custom product:
        Product parts: PartA1, PartC1
        
        */

        // The Builder interface specifies methods for creating the different parts
        // of the Product objects.
        interface IBuilder
        {
            void BuildPartA();
            void BuildPartB();
            void BuildPartC();
        }

        class ConcreteBuilder : IBuilder
        {
            private Product _product = new Product();

            // En ny builderinstans ska innehålla ett tomt produktobjekt
            public ConcreteBuilder()
            {
                Reset(); // Behövs detta?
            }

            public void Reset()
            {
                _product = new Product();
            }

            // Alla produktsteg jobba med samma produkt
            public void BuildPartA()
            {
                _product.Add("PartA1");
            }

            public void BuildPartB()
            {
                _product.Add("PartB1");
            }

            public void BuildPartC()
            {
                _product.Add("PartC1");
            }

                /*
                 Konkreta builders ska ha sin eget metod för att hämta resultatet. Detta deklareras inte i bas-builder-interfacet
                 */
            public Product GetProduct()
            {
                Product result = _product;


                // Inget krav att anropa "reset", men det är normalt
                Reset();

                return result;
            }
        }

        // Produkten är ovetande om de övriga klasserna
        class Product
        {
            private List<object> _parts = new List<object>();

            public void Add(string part)
            {
                _parts.Add(part);
            }

            public string ListParts()
            {
                return $"Product parts: {string.Join(",", _parts)}\n";
            }
        }


        // Är bara ansvarig för att köra byggstegen i en viss ordning
        // Den kan användas för att tillverka populära produkter
        // Du måste inte ha en director
        class Director
        {
            private IBuilder _builder;

            public IBuilder Builder
            {
                set { _builder = value; }
            }

            // Kan skapa flera varianter av produkter

            public void BuildMinimalViableProduct()
            {
                _builder.BuildPartA();
            }

            public void BuildFullFeaturedProduct()
            {
                _builder.BuildPartA();
                _builder.BuildPartB();
                _builder.BuildPartC();
            }
        }

    }
}
