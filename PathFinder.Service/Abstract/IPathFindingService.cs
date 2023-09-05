using PathFinder.Domain.Enums;
using PathFinder.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFinder.Service.Abstract
{
    public interface IPathFindingService
    {
        bool TryMoveToNeighbour(Position[,] map, NeighbourType type, ref Position position, out Position lastPosition);

        Task<List<Position>> NavigateToPath(Position[,] map, Position start, Position end);
    }
}
