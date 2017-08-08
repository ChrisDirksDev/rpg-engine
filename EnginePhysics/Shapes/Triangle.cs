using System.Windows;

namespace EnginePhysics
{
    public struct Triangle
    {
        public Vector PointA { get; }
        public Vector PointB { get; }
        public Vector PointC { get; }
        public Vector Center { get; }

        public Triangle(Vector a, Vector b, Vector c)
        {
            PointA = a;
            PointB = b;
            PointC = c;

            Center = new Vector();
        }
    }
}