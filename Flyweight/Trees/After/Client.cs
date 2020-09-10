
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Flyweight.Trees.After
{
    class Client : IClient
    {
        private static readonly Queue<string> _events = new Queue<string>();

        public void DoStuff()
        {
            var factory = new TreeTypeFactory();
            var forest = new Forest(factory);

            forest.PlantTree(1, 2, "Elm", "Green", "Rough");
            Assert.AreEqual("Created new TreeType: Elm Green Rough",   _events.Dequeue());

            forest.PlantTree(3, 4, "Redbud", "Pink", "Smooth");
            Assert.AreEqual("Created new TreeType: Redbud Pink Smooth", _events.Dequeue());

            forest.PlantTree(5, 6, "Elm", "Green", "Rough");
            Assert.AreEqual("Reused TreeType: Elm Green Rough", _events.Dequeue());

            forest.Draw();

            Assert.AreEqual("Draw tree: Elm Green Rough (1,2)", _events.Dequeue());
            Assert.AreEqual("Draw tree: Redbud Pink Smooth (3,4)", _events.Dequeue());
            Assert.AreEqual("Draw tree: Elm Green Rough (5,6)", _events.Dequeue());


            // Endast två trädtyper är i minnet även av vi skapat tre träd (sparat minne)
            Assert.AreEqual(2, factory.NrOfTreeTypes);

            // Verifiera att det inte är något extra event kvar
            Assert.AreEqual(0, _events.Count);
        }

        // Ett träd har en position och info (som ligger i TreeType)

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

            public static string Hash(string name, string color, string texture) => $"{name}_{color}_{texture}";
        }

        // Skapa trädtyper (ej beroende av position)
        class TreeTypeFactory
        {
            private readonly Dictionary<string, TreeType> _treeTypes = new Dictionary<string, TreeType>();

            public TreeType GetTreeType(string name, string color, string texture)
            {
                string hash = TreeType.Hash(name, color, texture);
                bool foundInFactory = _treeTypes.TryGetValue(hash, out TreeType treeType);

                if (foundInFactory)
                {
                    _events.Enqueue($"Reused TreeType: {treeType}");
                    return treeType;
                }

                var newTreeType = new TreeType(name, color, texture);
                _treeTypes.Add(hash, newTreeType);
                _events.Enqueue($"Created new TreeType: {newTreeType}");

                return newTreeType;
            }

            public int NrOfTreeTypes => _treeTypes.Count();
        }

        class Forest
        {
            private readonly List<Tree> _trees = new List<Tree>();

            private readonly TreeTypeFactory _treeTypeFactory;

            public Forest(TreeTypeFactory treeTypeFactory)
            {
                _treeTypeFactory = treeTypeFactory;
            }
            public void PlantTree(double x, double y, string name, string color, string texture)
            {
                var treeType = _treeTypeFactory.GetTreeType(name, color, texture);
                var tree = new Tree(x, y, treeType);
                _trees.Add(tree);
            }

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