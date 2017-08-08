using System.Windows;

namespace EnginePhysics
{
    public class TriangleCollisionBox : BasePhysObject
    {
        public Triangle Triangle;

        public TriangleCollisionBox(Triangle tri)
        {
            Init();
            Triangle = tri;
        }

        public TriangleCollisionBox(Vector pointA, Vector pointB, Vector pointC)
        {
            Init();
            Triangle = new Triangle(pointA, pointB, pointC);
        }

        private void Init()
        {
            Shape = PhysicShape.Triangle;
        }
    }
}