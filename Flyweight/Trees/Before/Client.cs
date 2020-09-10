
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.Flyweight.Trees.Before
{
    class Client : IClient
    {
        private static readonly Queue<string> _events = new Queue<string>();

        public void DoStuff()
        {
            var forest = new Forest();

            forest.PlantTree(1, 2, "Elm", "Green", "Rough");
            Assert.AreEqual("Created new TreeType: Elm Green Rough", _events.Dequeue());

            forest.PlantTree(3, 4, "Redbud", "Pink", "Smooth");
            Assert.AreEqual("Created new TreeType: Redbud Pink Smooth", _events.Dequeue());

            forest.PlantTree(5, 6, "Elm", "Green", "Rough");
            Assert.AreEqual("Created new TreeType: Elm Green Rough", _events.Dequeue()); // Nackdel: onödigt att vi skapat en extra

            forest.Draw();

            Assert.AreEqual("Draw tree: Elm Green Rough (1,2)", _events.Dequeue());
            Assert.AreEqual("Draw tree: Redbud Pink Smooth (3,4)", _events.Dequeue());
            Assert.AreEqual("Draw tree: Elm Green Rough (5,6)", _events.Dequeue());


            // Nackdel: vi skapar 3 trädtyper även om det bara är två som är olika (spenderar onödigt med RAM)
            Assert.AreEqual(3, forest.NrOfTreeTypes); // i exemplet "After" hämtar vi detta värde från "factory"

            Assert.AreEqual(0, _events.Count);
        }

        class Tree
        {
            public Tree(double x, double y, TreeType type)
            {
                X = x;
                Y = y;
                Type = type;
            }

            public double X { get; }
            public double Y { get; }
            public TreeType Type { get; }
        }

        class TreeType
        {
            public TreeType(string name, string color, string texture)
            {
                Name = name;
                Color = color;
                Texture = texture;
            }

            public string Name { get; }
            public string Color { get; }
            public string Texture { get; }

            public override string ToString() => $"{Name} {Color} {Texture}";
        }

        class Forest
        {
            private readonly List<Tree> _trees = new List<Tree>();

            public void PlantTree(double x, double y, string name, string color, string texture)
            {
                var treeType = new TreeType(name, color, texture);
                NrOfTreeTypes++;
                _events.Enqueue($"Created new TreeType: {treeType}");
                var tree = new Tree(x, y, treeType);
                _trees.Add(tree);
            }

            public int NrOfTreeTypes { get; private set; }

            public void Draw()
            {
                foreach (var tree in _trees)
                {
                    _events.Enqueue($"Draw tree: {tree.Type} ({tree.X},{tree.Y})");
                }
            }
        }
    }
}