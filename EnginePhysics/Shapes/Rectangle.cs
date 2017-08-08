using System.Windows;

namespace EnginePhysics
{
    public struct Rectangle
    {
        public Vector Size;
        private readonly Vector _start;
        public Vector Center => _start + Size * 0.5;

        public Rectangle(double x, double y, double w, double h)
        {
            _start = new Vector(x, y);
            Size = new Vector(w, h);
        }
    }
}