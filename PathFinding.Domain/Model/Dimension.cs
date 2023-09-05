namespace PathFinder.Domain.Model
{
    public class Dimension
    {
        private int _width;
        private int _height;

        public int Width => _width;
        public int Height => _height;

        public Dimension(int width, int height)
        {
            _width = width;
            _height = height;
        }
    }
}
