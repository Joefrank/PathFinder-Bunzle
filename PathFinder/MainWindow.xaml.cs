using PathFinder.Domain;
using PathFinder.Domain.Model;
using PathFinder.Factory;
using PathFinder.Service.Abstract;
using PathFinding.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PathFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {  


        //let's create a matrix we'll use to test our application. To save memory, we use an array of bool
        // true value means cell is empty and unit can be placed on this cell. false means occupied.
        bool[,] mapMatrix = MapConstants.MapMatrix;

        Position[,] _actualMap;

        private IMapFactory _mapFactory;
        private IPathFindingService _pathFindingService;

        public MainWindow(IMapFactory mapFactory, IPathFindingService pathFindingService)
        {
            InitializeComponent();
            _mapFactory = mapFactory;
            _pathFindingService = pathFindingService;
        }

        /// <summary>
        /// Generates the map with all cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GenerateMap_Click(object sender, RoutedEventArgs e)
        {
             _actualMap = await _mapFactory.BuildMap(canvas, mapMatrix, MapConstants.OCCUPIED_CELL_COLOR, MapConstants.EMPTY_CELL_COLOR, 
                new Dimension(MapConstants.UNIT_WIDTH, MapConstants.UNIT_HEIGHT));
        }

        /* I am going to assume that our unit starts from the top most left side of the matrix/grid
            * and that the destination point is the right-most point on the bottom row.
            * For this reason, my algorithm will navigate the unit to go from that start point to the end point assumed.
            * I also assume that Units don't move in diagonals so they move either left, right, top or bottom neighbours
            * */
        private async void NavigateMap_Click(object sender, RoutedEventArgs e)
        {
            if(_actualMap == null)
            {
                MessageBox.Show("Please generate map before navigating.");
                return;
            }            
           
            Position startPoint = _actualMap[0,0];
            Position endPoint = _actualMap[(MapConstants.MAX_CANVAS_ROWS -1), (MapConstants.MAX_CANVAS_COLS -1)];

            //Display starting position
            AddCellToCanvas(canvas, startPoint, $"#{MapConstants.START_POINT_COLOR}");
            // display ending position
            AddCellToCanvas(canvas, endPoint, $"#{MapConstants.END_POINT_COLOR}");

            List<Position> resultPath = await _pathFindingService.NavigateToPath(_actualMap, startPoint, endPoint);
                       
            ShowPath(resultPath);
          
        }

        private void AddCellToCanvas(Canvas canvas, Position position, string color)
        {
            Rectangle rec = new Rectangle()
            {
                Width = MapConstants.UNIT_WIDTH,
                Height = MapConstants.UNIT_HEIGHT,
                Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(color)
            };

            //display starting point
            canvas.Children.Add(rec);
            Canvas.SetTop(rec, position.Y * MapConstants.UNIT_HEIGHT);
            Canvas.SetLeft(rec, position.X * MapConstants.UNIT_WIDTH);

        }

        private void ShowPath(List<Position> pathPositions)
        {
            foreach(Position pos in pathPositions)
            {
                AddCellToCanvas(canvas, pos, $"#{MapConstants.PATH_UNIT_COLOR}");
            }
        }
    }
}
