using System.Windows;

namespace EnginePhysics
{
    public struct CollisionResolutionData
    {
        public Vector PositionChange;
        public bool CollisionAxisX, CollisionAxisY;
        public float RotationChange;
        public bool Mount;
        public BasePhysObject obj;
    }
}