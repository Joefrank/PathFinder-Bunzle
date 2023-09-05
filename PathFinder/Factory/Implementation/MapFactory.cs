using PathFinder.Domain;
using PathFinder.Domain.Model;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PathFinder.Factory.Implementation
{
    public class MapFactory : IMapFactory
    {

        /// <summary>
        /// Method buils a map on a given canvass based on 2D matrix and colors provided
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="matrix"></param>
        /// <param name="occupiedCellColor"></param>
        /// <param name="emptyCellColor"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public Task<Position[,]> BuildMap(Canvas canvas, bool[,] matrix, string occupiedCellColor, string emptyCellColor, Dimension cellDimension)
        {
            int top;
            int left;
           
            //clear all children in the canvass
            canvas.Children.Clear();

            //for better performance, let's store array dimensions in variables
            int noRows = matrix.GetLength(0);
            int noCols = matrix.GetLength(1);

           Position[,] map = new Position[noRows, noCols];

            //let's define our color brushes for the rectangles.
            SolidColorBrush occupiedCellBrush = (SolidColorBrush)new BrushConverter().ConvertFrom($"#{occupiedCellColor}");
            SolidColorBrush emptyCellBrush = (SolidColorBrush)new BrushConverter().ConvertFrom($"#{emptyCellColor}");

           

            //MessageBox.Show($"Rows:{noRows} - Cols:{noCols}");
            return Task.Run(() => {

                for (int x = 0; x < noRows; x++)
                {
                    for (int y = 0; y < noCols; y++)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate {
                            // your code
                            Rectangle rec = new Rectangle()
                            {
                                Width = cellDimension.Width,
                                Height = cellDimension.Height,
                                StrokeThickness = 2,
                                //color rectangle/cell according to related value in matrix
                                Fill = (matrix[x, y] ? emptyCellBrush : occupiedCellBrush)
                            };

                            //now work our position of our cell/rectangle
                            left = x * cellDimension.Width;
                            top = cellDimension.Height * y;

                            canvas.Children.Add(rec);
                            Canvas.SetTop(rec, top);
                            Canvas.SetLeft(rec, left);

                            map[x, y] = new Position(x, y, matrix[x, y], map);

                            //var textBlock = GetTextBlock($"({x}, {y})");
                            //canvas.Children.Add(textBlock);
                            //Canvas.SetTop(textBlock, top);
                            //Canvas.SetLeft(textBlock, left);
                        });

                    }
                }

                //make sure each cell knows their neighbours before returning map
                InitializeMap(map);

               return map; 
            
            });//success
        }

        private void InitializeMap(Position[,] map)
        {
            int noRows = map.GetLength(0);
            int noCols = map.GetLength(1);

            for (int x = 0; x < noRows; x++)
            {
                for (int y = 0; y < noCols; y++)
                {
                    map[x, y].DiscoverNeighbours();
                }
            }

        }


        private TextBlock GetTextBlock(string text)
        {
            return new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0))
            };
        }


        
    }
}
