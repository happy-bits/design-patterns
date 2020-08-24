
using System;
using System.Collections.Generic;

namespace DesignPatterns.TemplateMethod.Distances.After
{
    class Client : IClient
    {
        public IEnumerable<double> Calculate()
        {
            ClientCode(new ConcreteClass1());

            ClientCode(new ConcreteClass2());

            return null;
        }

        /*
            Klientkoden kallar template-metoden för att utföra algoritmen
            Klienten behöver veta vilken konkret klass som används
        */

        public static void ClientCode(AbstractClass abstractClass)
        {
            // ...
            abstractClass.TemplateMethod(); // Denna metod ligger den enda som är synlig utåt och sitter på den abstrakta basklassen
            // ...
        }

        public abstract class AbstractClass
        {
            /*
                Denna metod är synlig och är ingången (klienten anropar denna)
                
                Här definieras skelettet av en algoritm

                Ingen (underklass) ska pilla på denna metod
            */
            public void TemplateMethod()
            {
                BaseOperation1();
                RequiredOperations1();
                BaseOperation2();
                Hook1();
                RequiredOperation2();
                BaseOperation3();
                Hook2();
            }

            private void BaseOperation1()
            {
                Console.WriteLine("AbstractClass says: I am doing the bulk of the work");
            }

            private void BaseOperation2()
            {
                Console.WriteLine("AbstractClass says: But I let subclasses override some operations");
            }

            private void BaseOperation3()
            {
                Console.WriteLine("AbstractClass says: But I am doing the bulk of the work anyway");
            }

            // Dessa (primitiva) operationer måste implementeras av underklasserna

            protected abstract void RequiredOperations1();
            protected abstract void RequiredOperation2();

            /*
             Här finns "hooks". Underklasser kan implementera dem men kan strunta i det

             Det gör det möjligt att haka in i algoritmen
            */

            protected virtual void Hook1() { }

            protected virtual void Hook2() { }
        }

        class ConcreteClass1 : AbstractClass
        {
            protected override void RequiredOperations1()
            {
                Console.WriteLine("ConcreteClass1 says: Implemented Operation1");
            }

            protected override void RequiredOperation2()
            {
                Console.WriteLine("ConcreteClass1 says: Implemented Operation2");
            }
        }

        // Normalt så overridear den konkreta klassen bara en del av basklassen
        class ConcreteClass2 : AbstractClass
        {
            protected override void RequiredOperations1()
            {
                Console.WriteLine("ConcreteClass2 says: Implemented Operation1");
            }

            protected override void RequiredOperation2()
            {
                Console.WriteLine("ConcreteClass2 says: Implemented Operation2");
            }

            protected override void Hook1()
            {
                Console.WriteLine("ConcreteClass2 says: Overridden Hook1");
            }

            // Hooks2 struntar vi
        }

    }
}
