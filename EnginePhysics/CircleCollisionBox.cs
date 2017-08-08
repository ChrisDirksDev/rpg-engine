namespace EnginePhysics
{
    public class CircleCollisionBox : BasePhysObject
    {
        private readonly Circle Circle;

        public CircleCollisionBox(Circle c)
        {
            Circle = c;
        }

        public Circle GetCircle()
        {
            return Circle;
        }

        private void Init()
        {
            Shape = PhysicShape.Circle;
        }
    }
}