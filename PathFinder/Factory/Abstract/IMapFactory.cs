using PathFinder.Domain;
using PathFinder.Domain.Model;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PathFinder.Factory
{
    public interface IMapFactory
    {
        Task<Position[,]> BuildMap(Canvas canvas, bool[,] matrix, string occupiedCellColor, string emptyCellColor, Dimension dimension);
    }
}
