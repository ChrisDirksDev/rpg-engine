using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EnginePhysics;
using Rendering;
using Utility;

namespace World
{
    /// <summary>
    ///     Scene objects contain all the properties and methods needed for rendering/physics/update logic
    ///     Any objects created from this class should be registered with the its associated scene at runtime
    /// </summary>
    public abstract class SceneObject : BaseGameObject
    {
        protected SceneObject()
        {
            SceneObjectAnimator = null;
            SceneLayer = SceneLayer.Default;
            Enabled = false;
            RenderObjects = new Dictionary<string, BaseRenderObject>();
            CollisionObjects = new Dictionary<string, BasePhysObject>();
        }

        #region declarations

        protected Dictionary<string, BasePhysObject> CollisionObjects;
        protected Dictionary<string, BaseRenderObject> RenderObjects;

        //The layer where this object resides
        protected SceneLayer SceneLayer;

        protected SceneObjectAnimator SceneObjectAnimator;
        private bool HasAnimation => SceneObjectAnimator != null;

        public bool Enabled;

        public SceneObject MountedObject;
        public bool Mounted => MountedObject != null;

        public ScenePhysics ScenePhysics;
        public bool ApplyScenePhysics;

        #endregion

        #region methods

        public IEnumerable<BasePhysObject> GetCollisionItems()
        {
            return CollisionObjects.Values;
        }

        public override void Update(object[] args)
        {
            ChildList.ForEach(x => x.Update(args));

            CollisionObjects.Values.ToList().ForEach(x => x.Update(args));


            //Currently our animation only consists of movement,
            if (HasAnimation)
                runAnimation();
            else
                DoMovement((float) args[0]);
        }

        protected virtual void DoMovement(float time)
        {
            if (ApplyScenePhysics && ScenePhysics != null)
                Velocity += new Vector(ScenePhysics.AccelerationX, ScenePhysics.AccelerationY) * time;

            Position += Velocity;

            if (Mounted)
            {
                Position += MountedObject.Velocity;
                MountedObject = null;
            }
        }

        private void runAnimation()
        {
            Velocity = SceneObjectAnimator.DoFrameStep();
            Position += Velocity;
        }

        protected void AddRenderObjects(Dictionary<string, BaseRenderObject> renderObjects)
        {
            foreach (var obj in renderObjects)
                AddRenderObject(obj.Key, obj.Value);
        }

        protected void AddRenderObject(string key, BaseRenderObject obj)
        {
            obj.ParentTransform = GlobalTransform;
            RenderObjects.Add(key, obj);
        }

        protected void AddPhysicsObject(string ident, BasePhysObject obj)
        {
            obj.OnCollision += CollisionHandler;
            obj.Parent = this;
            obj.ParentPhysics = GlobalTransform;
            CollisionObjects.Add(ident, obj);
        }

        //This delegate handles collision calls and routes them to the correct methods based on the Event parameters
        private void CollisionHandler(object obj, CollisionEventArgs args)
        {
            var src = obj as SceneObject;
            if (src == null) return;

            switch (args.CType)
            {
                case CollisionType.Trigger:
                    TriggerCollision(src, args);
                    break;
                case CollisionType.Solid:
                    SolidCollision(src, args);
                    break;
                case CollisionType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void TriggerCollision(SceneObject source, CollisionEventArgs args)
        {
        }

        protected virtual void SolidCollision(SceneObject source, CollisionEventArgs args)
        {
            ApplyCollisionResolution(args.CData);

            if (CheckForMount(args.CData))
                MountedObject = source;
        }

        private void ApplyCollisionResolution(CollisionResolutionData data)
        {
            Position += data.PositionChange;

            if (data.CollisionAxisX)
                Velocity = new Vector(0, Velocity.Y);

            if (data.CollisionAxisY)
                Velocity = new Vector(Velocity.X, 0);
        }

        private bool CheckForMount(CollisionResolutionData data)
        {
            if (ScenePhysics == null)
                return false;

            var Return = false;

            if (data.CollisionAxisX)
                if (data.PositionChange.X / ScenePhysics.AccelerationX < 0)
                    Return = true;
            if (data.CollisionAxisY)
                if (data.PositionChange.Y / ScenePhysics.AccelerationY < 0)
                    Return = true;

            return Return;
        }

        public virtual void Render(RenderMachine renderMachine)
        {
            foreach (var obj in RenderObjects.Values)
                obj.Render(renderMachine);
        }

        #endregion
    }

    public enum SceneLayer
    {
        Actor,
        Background,
        SceneStatic,
        Default
    }
}