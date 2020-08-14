using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class ScooterCalculators
    {
        abstract class CalculatorCreator
        {
            protected abstract ICalculator Create();

            internal decimal CalculatePrice(Model model, double distance)
            {
                var calculator = Create();
                decimal price = calculator.CalculatePrice(model, distance);
                return price;
            }
        }

        // "Concrete Creators" 
        // Vad är vitsen med detta, det blir bara ett mellansteg?
        class SwedishCalculatorCreator : CalculatorCreator
        {
            protected override ICalculator Create()
            {
                return new SwedishCalculator();
            }
        }

        class DanishCalculatorCreator : CalculatorCreator
        {
            protected override ICalculator Create()
            {
                return new DanishCalculator();
            }
        }

        interface ICalculator
        {
            decimal CalculatePrice(Model model, double distance);
        }

        class SwedishCalculator : ICalculator
        {
            public decimal CalculatePrice(Model model, double distance)
            {
                throw new NotImplementedException();
            }
        }

        class DanishCalculator : ICalculator
        {
            public decimal CalculatePrice(Model model, double distance)
            {
                throw new NotImplementedException();
            }
        }

        enum Model { A,B,C}

        class Client
        {
            public void Run(string country)
            {

            }
            public void ClientCode(CalculatorCreator calculator)
            {
                decimal price = calculator.CalculatePrice(Model.A, 500);
            }
        }
    }
}
