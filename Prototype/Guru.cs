/*
 
Du kan kopiera existerande objekt utan att din kod blir beroende av dess klass

Kloningen görs av det objekt som ska klonas (metoden sitter i samma klass). Fördel: du kommer åt allt ink privata fält.

Ett objekt som stödjer kloning kallas "prototype". Om ditt objekt har ett tiotal fält och 100-tal olika konfigurationer så är kloning ett alternativ till arb
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DesignPatterns.Prototype
{
    [TestClass]
    public class Guru
    {

        [TestMethod]
        public void Ex()
        {
            Person p1 = new Person
            {
                Age = 42,
                BirthDate = Convert.ToDateTime("1977-01-01"),
                Name = "Jack Daniels",
                IdInfo = new IdInfo(666)
            };

            Person p2 = p1.ShallowCopy();
            Person p3 = p1.DeepCopy();

            // Triviala test
            AssertAreEqual(p1.Name, p2.Name, p3.Name);
            AssertAreEqual(p1.BirthDate, p2.BirthDate, p3.BirthDate);
            AssertAreEqual(p1.Age, p2.Age, p3.Age);

            // Shallow copy: referense till IdInfo kopieras
            Assert.AreEqual(p1.IdInfo, p2.IdInfo); 
            Assert.AreEqual(p1.IdInfo.IdNumber, p2.IdInfo.IdNumber);  

            // Deep copy: objektet klonas så det blir en ny referens
            Assert.AreNotEqual(p1.IdInfo, p3.IdInfo);
            Assert.AreEqual(p1.IdInfo.IdNumber, p3.IdInfo.IdNumber);

            // Modifiera p1
            p1.Age = 32;
            p1.BirthDate = Convert.ToDateTime("1900-01-01");
            p1.Name = "Frank";
            p1.IdInfo.IdNumber = 7878;

            AssertNoneAreEqualToFirst(p1.Name, p2.Name, p3.Name);
            AssertNoneAreEqualToFirst(p1.BirthDate, p2.BirthDate, p3.BirthDate);
            AssertNoneAreEqualToFirst(p1.Age, p2.Age, p3.Age);

            // Shallow copy: referensen pekar fortfarande till samma adress
            Assert.AreEqual(p1.IdInfo, p2.IdInfo);
            Assert.AreEqual(p1.IdInfo.IdNumber, p2.IdInfo.IdNumber);

            // Deep copy: objekten är nu helt olika
            Assert.AreNotEqual(p1.IdInfo, p3.IdInfo);
            Assert.AreNotEqual(p1.IdInfo.IdNumber, p3.IdInfo.IdNumber);
        }

        private void AssertNoneAreEqualToFirst(object value1, object value2, object value3)
        {
            Assert.AreNotEqual(value1, value2);
            Assert.AreNotEqual(value1, value3);
        }

        private void AssertAreEqual(object value1, object value2, object value3)
        {
            Assert.AreEqual(value1, value2);
            Assert.AreEqual(value1, value3);
        }

        class Person
        {
            public int Age;
            public DateTime BirthDate;
            public string Name;
            public IdInfo IdInfo;

            public Person ShallowCopy()
            {
                // "MemberwiseClone" är en inbyggd metod som gör en "shallow-copy" av objektet
                return (Person)MemberwiseClone();
            }

            // "Prototype pattern"
            public Person DeepCopy()
            {
                Person clone = (Person)MemberwiseClone();
                clone.IdInfo = new IdInfo(IdInfo.IdNumber);
                clone.Name = Name;
                return clone;
            }
        }

        class IdInfo
        {
            public int IdNumber;

            public IdInfo(int idNumber)
            {
                IdNumber = idNumber;
            }
        }

    }
}
