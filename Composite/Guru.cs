// "En gren är samma som ett löv"

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.Composite
{
    [TestClass]
    public class Guru
    {
        [TestMethod]
        public void Ex1()
        {
            Client client = new Client();

            // Det går att skicka in ett löv
            Leaf leaf = new Leaf();
            Console.WriteLine("Client: I get a simple component:");
            client.ClientCode(leaf);

            /*
             ...eller en komplex "composite": tree.

            Trädet har två grenar. 
            Första grenen har två löv. 
            Andra grenen ett löv
            */

            Composite tree = new Composite();
            Composite branch1 = new Composite();
            branch1.Add(new Leaf());
            branch1.Add(new Leaf());
            Composite branch2 = new Composite();
            branch2.Add(new Leaf());
            tree.Add(branch1);
            tree.Add(branch2);
            Console.WriteLine("Client: Now I've got a composite tree:");
            client.ClientCode(tree);

            // Skicka in en blandning av löv och träd (båda är "components")
            Console.Write("Client: I don't need to check the components classes even when managing the tree:\n");
            client.ClientCode2(tree, leaf);
        }
        /*
         
        Client: I get a simple component:
        RESULT: Leaf

        Client: Now I've got a composite tree:
        RESULT: Branch(Branch(Leaf+Leaf)+Branch(Leaf))

        Client: I don't need to check the components classes even when managing the tree:
        RESULT: Branch(Branch(Leaf+Leaf)+Branch(Leaf)+Leaf)

         */

        abstract class Component
        {
            // Operationerna nedan gäller för både komplexa (composites) och enkla objekt

            public abstract string Operation(); // Kan alternativt vara "virtual"

            public virtual void Add(Component component)
            {
                throw new NotImplementedException();
            }

            public virtual void Remove(Component component)
            {
                throw new NotImplementedException();
            }

            // En enkel metod som talar om för klienten och en komponent kan ha barn
            public virtual bool IsComposite()
            {
                return true;
            }
        }

        /*
         Lövet kan inte ha´några barn

         I normala fall är det löven som utför arbetet. Composite delegerar bara arbetet
        */
        class Leaf : Component
        {
            public override string Operation()
            {
                return "Leaf";
            }

            public override bool IsComposite()
            {
                return false;
            }
        }
        /*
        Ett träd eller en gren är exempel på "composites"

        Normalt så delegerar den jobbet till barnen och "summerar" sedan resultatet    

        */
        class Composite : Component
        {
            private readonly List<Component> _children = new List<Component>();

            public override void Add(Component component)
            {
                _children.Add(component);
            }

            public override void Remove(Component component)
            {
                _children.Remove(component);
            }

            /*
             Går rekursivt igenom alla dess barn. Samlar ihop och summerar till ett resultat.
            */
            public override string Operation()
            {
                int i = 0;
                string result = "Branch(";

                foreach (Component component in _children)
                {
                    // Här anropas ett barn
                    result += component.Operation();
                    if (i != _children.Count - 1)
                    {
                        result += "+";
                    }
                    i++;
                }

                return result + ")";
            }
        }

        class Client
        {
            // Klienten behöver inte bry sig om det kommer ett löv eller en "Composite"
            public void ClientCode(Component leaf)
            {
                Console.WriteLine($"RESULT: {leaf.Operation()}\n");
            }

            public void ClientCode2(Component component1, Component component2)
            {
                if (component1.IsComposite())
                {
                    component1.Add(component2);
                }

                Console.WriteLine($"RESULT: {component1.Operation()}");
            }
        }

    }
}
