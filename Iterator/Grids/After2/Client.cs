// Samma som "After" men har snyggat till "MoveNext"-metoden 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DesignPatterns.Iterator.Grids.After2
{
    class Client : IClient
    {
        public List<string> ForEachFromLeftToRight()
        {
            var grid = new Grid(new string[,] {
                { "a", "b", "c", "d" },
                { "e", "f", "g", "h" },
            });

            // Testar bara basic saker med griden
            Assert.AreEqual(4, grid.Width);
            Assert.AreEqual(2, grid.Height);
            Assert.AreEqual(8, grid.NrOrCells);
            Assert.AreEqual("d", grid.Items[0,3]);

            var cells = new List<string>();
            
            grid.Direction = Direction.LeftRight;

            foreach (var cell in grid)
            {
                cells.Add(cell);
            }

            return cells;


        }
        public List<string> ForEachFromUpToDown()
        {
            var grid = new Grid(new string[,] {
                { "a", "b", "c", "d" },
                { "e", "f", "g", "h" },
            });

            grid.Direction = Direction.UpDown;

            var cells = new List<string>();

            foreach (var cell in grid)
            {
                cells.Add(cell);
            }
            return cells;

        }
    }

    class Grid: IEnumerable<string>
    {
        public Grid(string[,] items)
        {
            Items = items;
        }

        public IEnumerator<string> GetEnumerator() => Direction switch
        {
            Direction.LeftRight => new GridIteratorLeftRight(this),
            Direction.UpDown => new GridIteratorUpDown(this),
            _ => throw new NotImplementedException()
        };

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public string[,] Items { get; }

        public int Height => Items.GetUpperBound(0)+1;
        public int Width => Items.GetUpperBound(1)+1;
        public int NrOrCells => Width * Height;

        public Direction Direction { get; internal set; }
    }

    enum Direction
    {
        LeftRight, UpDown
    }

    class GridIteratorLeftRight : GridIterator
    {
        public GridIteratorLeftRight(Grid grid) : base(grid)
        {
        }

        protected override void MovePositions()
        {
            _positionX++;
            if (_positionX >= _grid.Width)
            {
                _positionX = 0;
                _positionY++;
            }
        }
    }

    class GridIteratorUpDown : GridIterator
    {
        public GridIteratorUpDown(Grid grid) : base(grid)
        {
        }

        protected override void MovePositions()
        {
            _positionY++;
            if (_positionY >= _grid.Height)
            {
                _positionY = 0;
                _positionX++;
            }
        }
    }
    abstract class GridIterator : IEnumerator<string>
    {
        protected readonly Grid _grid;
        protected int _positionX = -1;
        protected int _positionY = -1;

        private bool _hasMoved = false;

        public GridIterator(Grid grid)
        {
            _grid = grid;
        }

        public string Current => _grid.Items[_positionY, _positionX];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        protected abstract void MovePositions();

        // Fördel: denna metod blir mycket tajtare
        public bool MoveNext()
        {
            MovePositions();

            if (!_hasMoved)
            {
                Reset();
                _hasMoved = true;
            }

            return _positionX < _grid.Width && _positionY < _grid.Height;
        }

        public void Reset()
        {
            _positionX = 0;
            _positionY = 0;
        }
    }

}
