using System.Windows;

namespace EnginePhysics
{
    public struct Circle
    {
        public float Radius;
        public Vector Center;

        public Circle(Vector T, float r)
        {
            Center = T;
            Radius = r;
        }
    }
}