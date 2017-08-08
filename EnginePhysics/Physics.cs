using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EnginePhysics
{
    public static class Physics
    {
        /// <summary>
        ///     Bad and stupid, this logic should be handled via the game code
        /// </summary>
        /// <param name="physList"></param>
        public static void CheckResolveCollision(IEnumerable<BasePhysObject> physList)
        {
            var basePhysObjects = physList as IList<BasePhysObject> ?? physList.ToList();
            var mList1 = new List<BasePhysObject>(basePhysObjects);
            var mList2 = new List<BasePhysObject>(basePhysObjects);

            foreach (var item in mList1)
            {
                mList2.Remove(item);
                CheckResolveCollision(item, mList2);
            }
        }

        public static void CheckResolveCollision(BasePhysObject obj, IEnumerable<BasePhysObject> physList)
        {
            var mList = new List<BasePhysObject>(physList);

            var ia = obj;

            foreach (var ib in mList.Where(ib => IsColliding(ia, ib)))
            {
                var dataA = new CollisionResolutionData();
                var dataB = new CollisionResolutionData();

                var collisionType = CollisionType.Trigger;

                if (ia.Type == CollisionType.Solid && ib.Type == CollisionType.Solid)
                {
                    Collision(ia, ib, out dataA, out dataB);

                    collisionType = CollisionType.Solid;
                }

                ia.Collision(ib, collisionType, dataA);
                ib.Collision(ia, collisionType, dataB);
            }
        }

        /// <summary>
        ///     Main collision detection function that identifies the logic needed and routes it appropriatly
        /// </summary>
        /// <param name="objA">The first object to be compared</param>
        /// <param name="objB">The second object to be compared</param>
        /// <returns></returns>
        public static bool IsColliding(BasePhysObject objA, BasePhysObject objB)
        {
            var Return = false;
            if (objA.Shape != objB.Shape)
            {
                //Do SAT here
            }
            else
            {
                switch (objA.Shape)
                {
                    case PhysicShape.Circle:
                    {
                        var a = (CircleCollisionBox) objA;
                        var b = (CircleCollisionBox) objB;
                        Return = Cc(a.GetCircle(), b.GetCircle());
                    }
                        break;
                    case PhysicShape.Rectangle:
                    {
                        var a = (SquareCollisionBox) objA;
                        var b = (SquareCollisionBox) objB;
                        Return = Aabb(a, b);
                    }
                        break;
                    case PhysicShape.Triangle:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return Return;
        }

        /// <summary>
        ///     Axis-Aligned Bounding Box collision
        /// </summary>
        /// <param name="rectA">Box 1</param>
        /// <param name="rectB">Box 2</param>
        /// <returns></returns>
        private static bool Aabb(SquareCollisionBox rectA, SquareCollisionBox rectB)
        {
            return !(rectA.Right < rectB.Left) && !(rectA.Bottem < rectB.Top) && !(rectA.Left > rectB.Right) &&
                   !(rectA.Top > rectB.Bottem);
        }

        private static bool Cc(Circle circle1, Circle circle2)
        {
            var Return = false;

            var ca = circle1.Center;
            var cb = circle2.Center;

            var dx = (float) (ca.X - cb.X);
            var dy = (float) (cb.Y - cb.Y);

            var dist = Math.Sqrt(dx * dx + dy * dy);

            if (dist < circle1.Radius + circle2.Radius)
                Return = true;

            return Return;
        }

        private static void Collision(BasePhysObject ia, BasePhysObject ib, out CollisionResolutionData dataA,
            out CollisionResolutionData dataB)
        {
            dataA = new CollisionResolutionData();
            dataB = new CollisionResolutionData();

            var abox = ia as SquareCollisionBox;
            var bbox = ib as SquareCollisionBox;

            if (abox != null && bbox != null)
                SolidSquareCollision(abox, bbox, out dataA, out dataB);
        }

        private static void SolidSquareCollision(SquareCollisionBox ia, SquareCollisionBox ib,
            out CollisionResolutionData dataA, out CollisionResolutionData dataB)
        {
            dataA = new CollisionResolutionData();
            dataB = new CollisionResolutionData();

            dataA.obj = ib;
            dataB.obj = ia;

            BasePhysObject objectA = ia;
            BasePhysObject objectB = ib;

            var av = objectA.GetVelocity();
            var bv = objectB.GetVelocity();

            //Get inverse vectors
            var inverseA = Vector.Multiply(-1, av);
            var inverseB = Vector.Multiply(-1, bv);

            //Combined magnitude
            var tXMag = Math.Abs(inverseA.X - inverseB.X);
            var tYMag = Math.Abs(inverseA.Y - inverseB.Y);

            var absXMag = Math.Abs(inverseA.X) + Math.Abs(inverseB.X);
            var absYMag = Math.Abs(inverseA.Y) + Math.Abs(inverseB.Y);

            double deltaX;
            double deltaY;

            if (av.X < 0 && bv.X < 0)
                deltaX = av.X < bv.X ? Math.Abs(ib.Right - ia.Left) : Math.Abs(ib.Left - ia.Right);
            else if (av.X > 0 && bv.X > 0)
                deltaX = av.X > bv.X ? Math.Abs(ib.Left - ia.Right) : Math.Abs(ib.Right - ia.Left);
            else
                deltaX = av.X > 0 ? Math.Abs(ib.Left - ia.Right) : Math.Abs(ib.Right - ia.Left);

            var aMount = false;
            var bMount = false;

            if (av.Y < 0 && bv.Y < 0)
            {
                if (av.Y > bv.Y)
                {
                    deltaY = Math.Abs(ib.Bottem - ia.Top);
                    bMount = true;
                }
                else
                {
                    deltaY = Math.Abs(ib.Top - ia.Bottem);
                    aMount = true;
                }
            }
            else if (av.Y > 0 && bv.Y > 0)
            {
                if (av.Y < bv.Y)
                {
                    deltaY = Math.Abs(ib.Top - ia.Bottem);
                    aMount = true;
                }
                else
                {
                    deltaY = Math.Abs(ib.Bottem - ia.Top);
                    bMount = true;
                }
            }
            else
            {
                if (av.Y > 0)
                {
                    deltaY = Math.Abs(ib.Top - ia.Bottem);
                    aMount = true;
                }
                else
                {
                    deltaY = Math.Abs(ib.Bottem - ia.Top);
                    bMount = true;
                }
            }


            //What we do here is get the ratio of movement to distance 
            //between the closest edges of the two objects 
            double xRatio = 0;
            double yRatio = 0;

            xRatio = deltaX / tXMag;
            yRatio = deltaY / tYMag;


            //The distance between the colliding edges of the objects after collision resolution
            //A buffer of 0.0 would result in the colliding edges still being ontop of eachother after collision resolution
            var collisionBuffer = 0.01;

            //The lower ratio indicates which side hit first
            if (xRatio < yRatio)
            {
                var xa = 0.0;
                var xb = 0.0;

                //apply correction
                if (ia.CollisionObjectType == CollisionObjectType.Static)
                {
                    xb = inverseA.X / absXMag * (deltaX + collisionBuffer) + inverseB.X / absXMag * deltaX;
                }
                else if (ib.CollisionObjectType == CollisionObjectType.Static)
                {
                    xa = inverseA.X / absXMag * (deltaX + collisionBuffer) + inverseB.X / absXMag * deltaX;
                }
                else
                {
                    xa = inverseA.X / absXMag * (deltaX + collisionBuffer);
                    xb = inverseB.X / absXMag * (deltaX + collisionBuffer);
                }

                dataA.PositionChange = new Vector(xa, 0);
                dataB.PositionChange = new Vector(xb, 0);

                dataA.CollisionAxisX = true;
                dataB.CollisionAxisX = true;
            }
            else
            {
                var ya = 0.0;
                var yb = 0.0;

                //apply correction
                if (ia.CollisionObjectType == CollisionObjectType.Static)
                {
                    yb = inverseA.Y / absYMag * (deltaY + collisionBuffer) + inverseB.Y / absYMag * deltaY;
                }
                else if (ib.CollisionObjectType == CollisionObjectType.Static)
                {
                    ya = inverseA.Y / absYMag * (deltaY + collisionBuffer) + inverseB.Y / absYMag * deltaY;
                }
                else
                {
                    ya = inverseA.Y / absYMag * (deltaY + collisionBuffer);
                    yb = inverseB.Y / absYMag * (deltaY + collisionBuffer);
                }

                dataA.PositionChange = new Vector(0, ya);
                dataB.PositionChange = new Vector(0, yb);

                dataA.CollisionAxisY = true;
                dataB.CollisionAxisY = true;

                //Do Mounting
                if (aMount && ia.CollisionObjectType != CollisionObjectType.Static)
                    dataA.Mount = true;
                if (bMount && ib.CollisionObjectType != CollisionObjectType.Static)
                    dataB.Mount = true;
            }
        }

        // ReSharper disable once UnusedMember.Local
        private static float CrossProduct(Vector vec1, Vector vec2)
        {
            return (float) (vec1.X * vec2.Y - vec1.Y * vec2.X);
        }

        // ReSharper disable once UnusedMember.Local
        private static Vector CrossProduct(Vector vec, float s)
        {
            return new Vector(s * vec.Y, -s * vec.X);
        }

        // ReSharper disable once UnusedMember.Local
        private static Vector CrossProduct(float s, Vector vec)
        {
            return new Vector(-s * vec.Y, s * vec.X);
        }
    }
}