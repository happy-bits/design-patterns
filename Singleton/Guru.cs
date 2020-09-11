/*
 "För kontroll av dina globala variabler"

 Använd när det bara ska finnas en instans, t.ex ett databasobjekt som delas

 Om du vill ha striktare kontroll över globala variabler. Garanterar att det bara finns en instans av klassen
 
 Nackdel:

- Kan maskera dålig design (komponenter vet för mycket om varandra)
- Svårt att enhetstesta p.g.a många ramverk bygger på arv för att bygga mock-objekt. Så det är svårt/omöjligt att mocka en statisk metod
- (Behöver tänka på multitrådad version)
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.Singleton
{
    [TestClass]
    public class Guru
    {
        [TestMethod]
        public void Ex1()
        {
            Singleton s1 = Singleton.GetInstance();
            Singleton s2 = Singleton.GetInstance();

            Assert.IsTrue(s1 == s2);

            Assert.AreEqual(42, Singleton.SomeBusinessLogic());

            Assert.AreEqual(1, s1.AddOne());
            Assert.AreEqual(2, s1.AddOne());
            Assert.AreEqual(3, s2.AddOne()); // här används "s2", men det funkar ändå
            Assert.AreEqual(4, s1.AddOne());
        }

        class Singleton
        {
            // Privat konstruktor så du inte kan använda new-operator
            private Singleton() { }

            // Nackdel: denna lösning funkar inte i multitrådat  program
            private static Singleton _instance;

            // Alternativ till konstruktorn
            public static Singleton GetInstance()
            {
                if (_instance == null)
                {
                    // Detta sker bara en gång
                    _instance = new Singleton();
                }
                return _instance;
            }

            // Affärslogik som kan exekveras av instansen
            public static int SomeBusinessLogic()
            {
                return 42;
            }

            private int _number = 0;

            public int AddOne()
            {
                _number++;
                return _number;
            }
        }

                
    }
}
