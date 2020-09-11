/*
"Separera sättet du går igenom en kollektion från kollektionen själv"  
  
Du kan gå igenom en kollektion utan att avslöja den underliggande representationen (lista, stack, tree etc)
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DesignPatterns.Iterator
{
    [TestClass]
    public class Guru
    {

        [TestMethod]
        public void Ex1()
        {
            var collection = new WordsCollection();
            collection.AddItem("First");
            collection.AddItem("Second");
            collection.AddItem("Third");

            Console.WriteLine("Straight traversal:");

            // WordsCollection måste ha metoden "IEnumerator GetEnumerator()" för att kunna göra foreach
            foreach (var element in collection)
            {
                Console.WriteLine(element);
            }

            Console.WriteLine("\nReverse traversal:");

            // Inget sker här förutom att kollektionen vet att vi ändrar riktning
            collection.ReverseDirection();

            foreach (var element in collection)
            {
                Console.WriteLine(element);
            }
        }

        /*
           "Konkret iterator"
           Anger på vilket sätt man ska gå igenom kollektionen

            IEnumerator kräver:
            - Current
            - MoveNext()
            - Reset()
        */
        class AlphabeticalOrderIterator : IEnumerator
        {
            private readonly WordsCollection _collection;

            // Sparar nuvarande position
            private int _position = -1;

            private readonly bool _reverse = false;

            object IEnumerator.Current => Current(); // throw new NotImplementedException();

            public AlphabeticalOrderIterator(WordsCollection collection, bool reverse = false)
            {
                _collection = collection;
                _reverse = reverse;

                // "_position" kommer antingen vara -1 eller antalet-element
                if (reverse)
                {
                    _position = collection.GetItems().Count;
                }
            }

            public object Current()
            {
                return _collection.GetItems()[_position];
            }

            public int Key()
            {
                return _position;
            }

            public bool MoveNext()
            {
                int updatedPosition = _position + (_reverse ? -1 : 1);

                if (updatedPosition >= 0 && updatedPosition < _collection.GetItems().Count)
                {
                    // Vi har flyttat oss ett steg framåt
                    _position = updatedPosition;
                    return true;
                }
                else
                {
                    // Det går inte att stega vidare
                    return false;
                }
            }

            public void Reset()
            {
                _position = _reverse ? _collection.GetItems().Count - 1 : 0;
            }
        }

        // "Konkret kollektion"
        // IEnumerable kräver bara en metod: "GetEnumerator"
        class WordsCollection : IEnumerable
        {
            readonly List<string> _collection = new List<string>();

            bool _direction = false;

            public void ReverseDirection()
            {
                _direction = !_direction;
            }

            public List<string> GetItems()
            {
                return _collection;
            }

            public void AddItem(string item)
            {
                _collection.Add(item);
            }

            public IEnumerator GetEnumerator()
            {
                return new AlphabeticalOrderIterator(this, _direction);
            }
        }


    }
}
