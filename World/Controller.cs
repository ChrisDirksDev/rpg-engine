using System;
using System.Windows;
using Utility;

namespace World
{
    public class Controller
    {
        private readonly float _drag;
        private readonly int MaxVelocity;
        private readonly float MovementSpeed;

        public Controller()
        {
            MovementSpeed = 0.50f;
            MaxVelocity = 5;
            _drag = 0.25f;
        }


        internal Vector DoStep(float timestep, Keys inputs, Vector velocity)
        {
            var mVec = new Vector();
            //bool space;

            //Bitwise Comparisons
            if ((inputs & Keys.A) == Keys.A) mVec.X += -1;
            if ((inputs & Keys.D) == Keys.D) mVec.X += 1;
            if ((inputs & Keys.S) == Keys.S) mVec.Y += 1;
            if ((inputs & Keys.W) == Keys.W) mVec.Y += -1;

            //Get baseline acceleration from movement vector and speed
            var totalAcc = Vector.Multiply(MovementSpeed, mVec);

            //Get new velocity from acceleration and existing velocity
            var finalvelocity = Vector.Multiply(timestep, totalAcc) + velocity;

            //Do percentage based drag
            finalvelocity = Vector.Multiply(1 - _drag, finalvelocity);

            //Make sure we dont go past the speed limit
            if (finalvelocity.Length > MaxVelocity)
            {
                finalvelocity.Normalize();
                finalvelocity = Vector.Multiply(MaxVelocity, finalvelocity);
            }
            else
            {
                //If we are nearly zero then just reset the velocity to zero
                if (Math.Abs(finalvelocity.X) < 0.1)
                    finalvelocity.X = 0;
                if (Math.Abs(finalvelocity.Y) < 0.1)
                    finalvelocity.Y = 0;
            }

            return finalvelocity;
        }
    }
}