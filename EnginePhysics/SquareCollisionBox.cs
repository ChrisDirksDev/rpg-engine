using System.Windows;

namespace EnginePhysics
{
    public class SquareCollisionBox : BasePhysObject
    {
        public Vector Size;


        public SquareCollisionBox(Vector size)
        {
            Size = size;
            Position = size / -2;
            Init();
        }

        public SquareCollisionBox(Vector size, float x, float y)
        {
            Size = size;
            Position = new Vector(x, y);
            Init();
        }

        public double Left => GetPosition().X;
        public double Right => GetPosition().X + Size.X;
        public double Top => GetPosition().Y;
        public double Bottem => GetPosition().Y + Size.Y;

        private void Init()
        {
            Shape = PhysicShape.Rectangle;
        }
    }
}