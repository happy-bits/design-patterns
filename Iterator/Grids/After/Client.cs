
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DesignPatterns.Iterator.Grids.After
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

            var events = new List<string>();
            
            grid.Direction = Direction.LeftRight;

            foreach (var item in grid)
            {
                events.Add(item);
            }

            return events;


        }
        public List<string> ForEachFromUpToDown()
        {
            var grid = new Grid(new string[,] {
                { "a", "b", "c", "d" },
                { "e", "f", "g", "h" },
            });

            grid.Direction = Direction.UpDown;

            var events = new List<string>();

            foreach (var item in grid)
            {
                events.Add(item);
            }

            return events;

        }
    }

    class Grid: IEnumerable<string>
    {
        public Grid(string[,] items)
        {
            Items = items;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new GridIterator(this);
        }

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

    class GridIterator : IEnumerator<string>
    {
        private Grid _grid;
        private int _positionX = -1;
        private int _positionY = -1;

        private bool _hasMoved = false;

        public GridIterator(Grid grid)
        {
            _grid = grid;
        }

        public string Current => _grid.Items[_positionY, _positionX];//   _position / _grid.Width, _position % _grid.Width];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        // public object Current => ;

        public bool MoveNext()
        {
            switch (_grid.Direction)
            {
                case Direction.LeftRight:

                    _positionX++;
                    if (_positionX >= _grid.Width)
                    {
                        _positionX = 0;
                        _positionY++;
                    }

                    break;

                case Direction.UpDown:

                    _positionY++;
                    if (_positionY >= _grid.Height)
                    {
                        _positionY = 0;
                        _positionX++;
                    }

                    break;
                default:
                    throw new NotImplementedException();
            }

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
