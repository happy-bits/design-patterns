/*
 Du kan konstruera komplexa objekt steg för steg.

 Du kan producera flera typer av objekt genom samma "construction code"

  */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.Builder
{
    [TestClass]
    public class GuruConceptual
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

        // The Concrete Builder classes follow the Builder interface and provide
        // specific implementations of the building steps. Your program may have
        // several variations of Builders, implemented differently.
        class ConcreteBuilder : IBuilder
        {
            private Product _product = new Product();

            // A fresh builder instance should contain a blank product object, which
            // is used in further assembly.
            public ConcreteBuilder()
            {
                Reset();
            }

            public void Reset()
            {
                _product = new Product();
            }

            // All production steps work with the same product instance.
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

            // Concrete Builders are supposed to provide their own methods for
            // retrieving results. That's because various types of builders may
            // create entirely different products that don't follow the same
            // interface. Therefore, such methods cannot be declared in the base
            // Builder interface (at least in a statically typed programming
            // language).
            //
            // Usually, after returning the end result to the client, a builder
            // instance is expected to be ready to start producing another product.
            // That's why it's a usual practice to call the reset method at the end
            // of the `GetProduct` method body. However, this behavior is not
            // mandatory, and you can make your builders wait for an explicit reset
            // call from the client code before disposing of the previous result.
            public Product GetProduct()
            {
                Product result = _product;

                Reset();

                return result;
            }
        }

        // It makes sense to use the Builder pattern only when your products are
        // quite complex and require extensive configuration.
        //
        // Unlike in other creational patterns, different concrete builders can
        // produce unrelated products. In other words, results of various builders
        // may not always follow the same interface.
        class Product
        {
            private List<object> _parts = new List<object>();

            public void Add(string part)
            {
                _parts.Add(part);
            }

            public string ListParts()
            {
                string str = string.Empty;

                for (int i = 0; i < _parts.Count; i++)
                {
                    str += _parts[i] + ", ";
                }

                str = str.Remove(str.Length - 2); // removing last ",c"

                return "Product parts: " + str + "\n";
            }
        }

        // The Director is only responsible for executing the building steps in a
        // particular sequence. It is helpful when producing products according to a
        // specific order or configuration. Strictly speaking, the Director class is
        // optional, since the client can control builders directly.
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
