using System;
using System.Collections.Generic;
using System.Windows;

namespace Utility
{
    /// <summary>
    ///     Root object for all game/level objects
    /// </summary>
    public abstract class BaseGameObject : object
    {
        //Base Constructor
        protected BaseGameObject()
        {
            Init();
        }

        protected BaseGameObject(Vector P)
            : this()
        {
            LocalPosition = P;
        }

        /// <summary>
        ///     Returns whether this object currently has a parent
        /// </summary>
        public bool HasParent => Parent != null;

        private void Init()
        {
            LocalTransform = new Transform();
            GlobalTransform = new Transform();
            ChildList = new List<BaseGameObject>();
            Parent = null;
        }

        private void SetParent(BaseGameObject p = null)
        {
            Parent = p;

            //_parent?.RemoveChild(this);
            //p?.AddChild(this);

            //if (HasParent)
            //{
            //    UnMapParentTransform();
            //}

            //_parent = p;

            //if (SnapToParent && HasParent)
            //{
            //    MapParentTransform();
            //}
        }

        private void MapParentTransform()
        {
            GlobalTransform = Parent.GlobalTransform;
        }

        private void UnMapParentTransform()
        {
            var p = new Vector(Parent.Position.X, Parent.Position.Y);
            var v = new Vector(Parent.Velocity.X, Parent.Velocity.Y);
            var r = Parent.Rotation;

            var t = new Transform();
            t.SetPosition(Parent.Position);
            t.SetVelocity(Parent.Velocity);
            t.SetRotation(Parent.Rotation);

            GlobalTransform = t;
        }

        private void RemoveChild(BaseGameObject baseGameObject)
        {
            ChildList.Remove(baseGameObject);
        }

        private void AddChild(BaseGameObject baseGameObject)
        {
            ChildList.Add(baseGameObject);
        }

        protected void Attach(BaseGameObject baseGameObject, bool center = false)
        {
            Attach(baseGameObject, center, 0, 0);
        }

        private void Attach(BaseGameObject baseGameObject, bool center, float offsetX, float offsetY)
        {
            baseGameObject.UnAttach();
            baseGameObject.SetParent(this);
            AddChild(baseGameObject);

            if (center)
                baseGameObject.GlobalTransform = GlobalTransform;

            baseGameObject.LocalTransform.SetPosition(offsetX, offsetY);
        }

        private void UnAttach()
        {
            if (!HasParent)
                return;

            Parent.RemoveChild(this);
            UnrefGlobal();
            SetParent();
        }

        private void UnrefGlobal()
        {
            var t = new Transform();
            t.SetPosition(GlobalTransform.Position);
            t.SetVelocity(GlobalTransform.Velocity);
            t.SetRotation(GlobalTransform.Rotation);

            GlobalTransform = t;
        }

        private void GlobalToLocal()
        {
            LocalTransform.SetPosition(GlobalTransform.Position);
            LocalTransform.SetRotation(GlobalTransform.Rotation);
            LocalTransform.SetVelocity(GlobalTransform.Velocity);
        }

        public virtual void Update(object[] args)
        {
        }

        #region props/fields

        private Transform LocalTransform { get; set; }

        protected Transform GlobalTransform { get; private set; }

        private BaseGameObject Parent { get; set; }

        protected List<BaseGameObject> ChildList { get; private set; }

        public Vector Position
        {
            get => GlobalTransform.Position + LocalPosition;
            set
            {
                value.X = Math.Round(value.X, 2);
                value.Y = Math.Round(value.Y, 2);
                GlobalTransform.SetPosition(value);
            }
        }


        //Set the global position while zeroing out any local position
        public void SetPositionAbsolute(Vector P)
        {
            P.X = Math.Round(P.X, 2);
            P.Y = Math.Round(P.Y, 2);
            GlobalTransform.SetPosition(P);
            LocalPosition = new Vector();
        }

        public Vector LocalPosition
        {
            get => LocalTransform.Position;

            set => LocalTransform.SetPosition(value);
        }

        public Vector Velocity
        {
            get => GlobalTransform.Velocity + LocalVelocity;
            set
            {
                value.X = Math.Round(value.X, 2);
                value.Y = Math.Round(value.Y, 2);
                GlobalTransform.SetVelocity(value);
            }
        }

        public void SetVelocityAbsolute(Vector P)
        {
            P.X = Math.Round(P.X, 2);
            P.Y = Math.Round(P.Y, 2);
            GlobalTransform.SetVelocity(P);
            LocalVelocity = new Vector();
        }

        public Vector LocalVelocity
        {
            get => LocalTransform.Velocity;

            set => LocalTransform.SetVelocity(value);
        }

        public float Rotation
        {
            get => GlobalTransform.Rotation + LocalRotation;
            set
            {
                value = (float) Math.Round(value, 2);
                GlobalTransform.SetRotation(value);
            }
        }

        public void SetRotationAbosolute(float r)
        {
            r = (float) Math.Round(r, 2);
            GlobalTransform.SetRotation(r);
            LocalRotation = 0.0f;
        }

        public float LocalRotation
        {
            get => LocalTransform.Rotation;

            set => LocalTransform.SetRotation(value);
        }

        #endregion
    }
}