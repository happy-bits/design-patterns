using System;
using System.Collections.Generic;

namespace DesignPatterns.Iterator.Grids.Before
{
    class Client : IClient
    {
        public List<string> ForEachFromLeftToRight()
        {
            var grid = new Grid(new string[,] {
                { "a", "b", "c", "d" },
                { "e", "f", "g", "h" },
            });

            var cells = new List<string>();

            grid.Direction = Direction.LeftRight;

            foreach (var cell in grid.GetCells())
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

            var cells = new List<string>();

            grid.Direction = Direction.UpDown;

            foreach (var cell in grid.GetCells())
            {
                cells.Add(cell);
            }

            return cells;
        }
    }

    enum Direction
    {
        LeftRight, UpDown
    }

    class Grid
    {
        public Grid(string[,] items)
        {
            Items = items;
        }

        public string[,] Items { get; }

        public int Height => Items.GetUpperBound(0) + 1;
        public int Width => Items.GetUpperBound(1) + 1;
        public int NrOrCells => Width * Height;

        public Direction Direction { get; internal set; }

        // Nackdel: dessa tre extra fält här
        private int _positionX = -1;
        private int _positionY = -1;
        private bool _hasMoved = false;

        // Nackdel: denna avancerade funktion som har att göra mer hur man itererar över en grid. "Grid" får för mycket ansvar.

        internal IEnumerable<string> GetCells()
        {
            while(true)
            {
                switch (Direction)
                {
                    case Direction.LeftRight:

                        _positionX++;
                        if (_positionX >= Width)
                        {
                            _positionX = 0;
                            _positionY++;
                        }

                        break;

                    case Direction.UpDown:

                        _positionY++;
                        if (_positionY >= Height)
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

                if (_positionX >= Width || _positionY >= Height)
                    break;

                yield return Items[_positionY, _positionX]; 
            }
        }

        private void Reset()
        {
            _positionX = 0;
            _positionY = 0;
        }
    }
}