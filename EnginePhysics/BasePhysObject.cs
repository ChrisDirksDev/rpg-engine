using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Utility;

namespace EnginePhysics
{
    public abstract class BasePhysObject
    {
        private List<BasePhysObject> _currentCollisions, _prevCollisions;

        public CollisionObjectType CollisionObjectType;

        public BasePhysObject MountedObject;

        //a ref to our parent object
        public object Parent;

        public Transform ParentPhysics;
        public Vector Position;
        public float Rotation;
        public TriggerStyle Style;

        public CollisionType Type;

        protected BasePhysObject()
        {
            Shape = PhysicShape.None;
            Type = CollisionType.None;
            _currentCollisions = new List<BasePhysObject>();
            _prevCollisions = new List<BasePhysObject>();
            Style = TriggerStyle.None;
            CollisionObjectType = CollisionObjectType.Static;
            ParentPhysics = null;
        }

        public PhysicShape Shape { get; protected set; }
        public CollisionLayer CollisionLayer { get; set; }
        public bool Mounted => MountedObject != null;

        public event CollisionEventHandler OnCollision;

        public virtual void Update(object[] args)
        {
            MountedObject = null;
        }

        public Vector GetPosition()
        {
            return ParentPhysics == null ? Position : Position + ParentPhysics.Position;
        }

        public float GetRotation()
        {
            return ParentPhysics == null ? Rotation : Rotation + ParentPhysics.Rotation;
        }

        public Vector GetVelocity()
        {
            return ParentPhysics == null ? new Vector() : ParentPhysics.Velocity;
        }

        //Called by the physics collision check if a collision is detected
        public void Collision(BasePhysObject obj, CollisionType type, CollisionResolutionData data)
        {
            addCurrentCollisions(obj);

            switch (type)
            {
                case CollisionType.Trigger:
                    RunTriggerLogic(obj.Parent);
                    break;
                case CollisionType.Solid:
                    RunSolidLogic(obj.Parent, type, data);
                    break;
                case CollisionType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void addCurrentCollisions(BasePhysObject obj)
        {
            if (!_currentCollisions.Contains(obj))
                _currentCollisions.Add(obj);
        }

        private void RunSolidLogic(object objParent, CollisionType type, CollisionResolutionData data)
        {
            OnCollision?.Invoke(objParent,
                new CollisionEventArgs("") {CType = type, TStyle = TriggerStyle.None, CData = data});
        }

        private void RunTriggerLogic(object obj)
        {
            switch (Style)
            {
                case TriggerStyle.Enter:
                case TriggerStyle.EnterExit:
                    RunEnterTriggers(obj);
                    break;
                case TriggerStyle.Active:
                    RunActiveTriggers(obj);
                    break;
                case TriggerStyle.None:
                    break;
                case TriggerStyle.Exit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RunActiveTriggers(object obj)
        {
            EventLogic(obj, Type, TriggerStyle.Active);
        }

        private void RunEnterTriggers(object obj)
        {
            if (!_prevCollisions.Contains(obj))
                EventLogic(obj, Type, TriggerStyle.Enter);
        }

        private void RunExitTriggers()
        {
            var exits = _prevCollisions.Except(_currentCollisions);

            foreach (var obj in exits)
                //Send an exit event to the physics object that is no longer colliding with us
                EventLogic(obj.Parent, Type, TriggerStyle.Exit);
        }

        public void EndFrameCheck()
        {
            if (Style == TriggerStyle.Exit || Style == TriggerStyle.EnterExit)
                RunExitTriggers();

            _prevCollisions = _currentCollisions;
            _currentCollisions = new List<BasePhysObject>();
        }

        private void EventLogic(object obj, CollisionType type, TriggerStyle style,
            CollisionResolutionData data = new CollisionResolutionData())
        {
            OnCollision?.Invoke(obj, new CollisionEventArgs("") {CType = type, TStyle = style, CData = data});
        }
    }
}