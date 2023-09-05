using PathFinder.Domain.Enums;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PathFinder.Domain.Model
{
    public class Position
    {
        private bool _empty;
        Position[,] _map;

        public int X;
        public int Y;
        public bool Empty => _empty;

        public List<Position> Neighbours; //these are neighbouring positions to this one.
        public List<Position> VisitedNeighbours;

        //this matrix enables us to circle a position and find neighbouring positions
        public Point[] PossibleMovesMatrix = new Point[]
       {
            new Point(-1, 0), new Point(0, 1),  new Point(1, 0),  new Point(0, -1)
       };

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="empty">If position is empty so can be used by our moving unit</param>
        /// <param name="mapOfPosition">Map object that this position belongs to</param>
        public Position(int x, int y,  bool empty, Position[,] mapOfPosition)
        {
            X = x;
            Y = y;
            _empty = empty;
            Neighbours = new List<Position>();
            VisitedNeighbours = new List<Position>();
            _map = mapOfPosition;
        }

        public Position GetNeighbour(NeighbourType neighbourType)
        {
            switch (neighbourType)
            {
                case NeighbourType.Top: 
                    return Neighbours.FirstOrDefault(pos => pos.X == X && pos.Y == Y - 1);
                case NeighbourType.Bottom:
                    return Neighbours.FirstOrDefault(pos => pos.X == X && pos.Y == Y + 1);
                case NeighbourType.Left:
                    return Neighbours.FirstOrDefault(pos => pos.X == X -1 && pos.Y == Y);
                case NeighbourType.Right:
                    return Neighbours.FirstOrDefault(pos => pos.X == X+1 && pos.Y == Y);
                default:
                    return null;

            }
             
        }

        public bool HasVisitedNeighbour(NeighbourType neighbourType)
        {
            switch (neighbourType)
            {
                case NeighbourType.Top:
                    return VisitedNeighbours.Any(pos => pos.X == X && pos.Y == Y - 1);
                case NeighbourType.Bottom:
                    return VisitedNeighbours.Any(pos => pos.X == X && pos.Y == Y + 1);
                case NeighbourType.Left:
                    return VisitedNeighbours.Any(pos => pos.X == X - 1 && pos.Y == Y);
                case NeighbourType.Right:
                    return VisitedNeighbours.Any(pos => pos.X == X + 1 && pos.Y == Y);
                default:
                    return false;

            }

        }

        /// <summary>
        /// Function enables us to discover neighbouring positions
        /// </summary>        
        public void DiscoverNeighbours()
        {
            var noOfNeighbours = PossibleMovesMatrix.Length; //this is for performance.
            var maxX = _map.GetLength(0);
            var maxY = _map.GetLength(1);

            for (int i = 0; i < noOfNeighbours; i++)
            {
                var newX = PossibleMovesMatrix[i].X + X;
                var newY = PossibleMovesMatrix[i].Y + Y;

                if ((newX < maxX && newX > -1)//check that new x is within range of map.
                    && (newY < maxY && newY > -1)//check y is also within range of map
                    && _map[newX, newY].Empty) //only empty neighbours we are interested in
                {
                    Neighbours.Add(_map[newX, newY]);
                }                
              
            }
        }
         
        public override bool Equals(object obj)
        {
            Position pos = (Position)obj;
            return (pos.X == X && pos.Y == Y);
        }

        public override int GetHashCode()
        {
            return (X << 2) ^ Y;
        }
    }
}
