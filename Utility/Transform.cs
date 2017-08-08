using System;
using System.Windows;

namespace Utility
{
    /// <summary>
    ///     Used to track an objects postion, rotation, and velocity within the game environment
    /// </summary>
    public class Transform
    {
        public Transform()
        {
            Velocity = new Vector();
            Position = new Vector();
            Rotation = 0.0f;
        }

        public Vector Velocity { get; private set; }

        public Vector Position { get; private set; }

        public float Rotation { get; private set; }

        public void SetPosition(float x, float y)
        {
            SetPosition(new Vector(x, y));
        }

        public void SetPosition(Vector p)
        {
            var x = Math.Round(p.X, 2);
            var y = Math.Round(p.Y, 2);
            var newV = new Vector(x, y);

            Position = newV;
        }

        public void SetVelocity(float x, float y)
        {
            Velocity = new Vector(x, y);
        }

        public void SetVelocity(Vector p)
        {
            Velocity = p;
        }

        public void SetRotation(float r)
        {
            Rotation = r;
        }

        public void Reset()
        {
            Velocity = new Vector();
            Position = new Vector();
            Rotation = 0.0f;
        }
    }
}