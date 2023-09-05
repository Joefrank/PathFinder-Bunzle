using PathFinder.Domain.Enums;
using PathFinder.Domain.Model;
using PathFinder.Service.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFinder.Service.Implementation
{
    public class PathFindingService : IPathFindingService
    {

        /// <summary>
        /// Moves unit to neighbouring cell.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <param name="lastPosition"></param>
        /// <returns></returns>
        public bool TryMoveToNeighbour(Position[,] map, NeighbourType type, ref Position position, out Position lastPosition)
        {
            Position neighbour = position.GetNeighbour(type);

            if (neighbour != null && neighbour.Empty)//move current pos to below if empty
            {
                if (!position.VisitedNeighbours.Contains(neighbour))
                    position.VisitedNeighbours.Add(neighbour);

                lastPosition = map[position.X, position.Y];
                position = neighbour;
                
                return true;
            }
            lastPosition = position;
            return false;
        }

        /// <summary>
        /// Navigates unit from start position to end position stepping on empty cells to find path
        /// </summary>
        /// <param name="map"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Task<List<Position>> NavigateToPath(Position[,] map, Position start, Position end)
        {
            int noRows = map.GetLength(0);
            int noCols = map.GetLength(1);
            Position lastPosition = start;
            Position currentPosition = start;
            List<Position> pathPositions = new List<Position>();
            bool blocked = false;

            return Task.Run(() =>
            {
                //first check that start point is on the map
                if (start.X >= noRows || start.X < 0)//this is the invalid range.
                {
                    //build error message and return.
                    return null;
                }

                //check that end point is also on the map
                if (end.X >= noRows || end.X < 0)//this is the invalid range.
                {
                    return null;
                }
           
              
                    // this loop keeps updating the position/cell of our unit until we reach the end point or get blocked(can't move)
                do
                {
                    //we build our path by recording postions we passed through
                    if(currentPosition != start && !pathPositions.Contains(currentPosition))//we have moved at least once
                    {
                        pathPositions.Add(currentPosition);
                    }
                    else if(currentPosition != start && pathPositions.Contains(lastPosition))// we have back-tracked, remove last position
                    {
                        pathPositions.Remove(lastPosition);
                    }

                    //try getting neighbour below. because we want to move downward first.
                    if (!currentPosition.HasVisitedNeighbour(NeighbourType.Bottom)
                        && TryMoveToNeighbour(map, NeighbourType.Bottom, ref currentPosition, out lastPosition))
                        continue;

                    if (!currentPosition.HasVisitedNeighbour(NeighbourType.Right)
                        && TryMoveToNeighbour(map, NeighbourType.Right, ref currentPosition, out lastPosition))
                        continue;

                    if (!currentPosition.HasVisitedNeighbour(NeighbourType.Left)
                        && TryMoveToNeighbour(map, NeighbourType.Left, ref currentPosition, out lastPosition))
                        continue;

                    if (!currentPosition.HasVisitedNeighbour(NeighbourType.Top)
                        && TryMoveToNeighbour(map, NeighbourType.Top, ref currentPosition, out lastPosition))
                        continue;


                    //if we still haven't processed this then we are stock and can't move. It means there is no path between our start and end position.                
                    blocked = true;


                }
                while (!blocked && !currentPosition.Equals(end));


                ////Let's work our way from start to end point. We are going from top to bottom so from coordinates (0,0) to (maxX, maxY)
                ////Check the cell directly below if it is free. then move to that. if not move along the row to the right.

                return pathPositions;
            });

        }

    }
}
