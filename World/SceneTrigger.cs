using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using EnginePhysics;
using Rendering;
using Utility;

namespace World
{
    public abstract class SceneTrigger : SceneObject
    {
        protected const string TRIGGER_INDEX = "main_trigger";

        protected SceneTrigger()
        {
            SceneLayer = SceneLayer.SceneStatic;
            AddRenderObjects(Helper.GetDebugRenderObjects(this));
        }

        protected override void TriggerCollision(SceneObject obj, CollisionEventArgs args)
        {
            switch (args.TStyle)
            {
                case TriggerStyle.Enter:
                    EnterTriggerAction(obj);
                    break;
                case TriggerStyle.Exit:
                    ExitTriggerAction(obj);
                    break;
                case TriggerStyle.Active:
                    ActiveTriggerAction(obj);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void ActiveTriggerAction(SceneObject Source)
        {
            throw new NotImplementedException();
        }

        protected virtual void ExitTriggerAction(SceneObject Source)
        {
            throw new NotImplementedException();
        }

        protected virtual void EnterTriggerAction(SceneObject Source)
        {
            throw new NotImplementedException();
        }

        protected void SetTriggerStyle(TriggerStyle style)
        {
            CollisionObjects[TRIGGER_INDEX].Style = style;
        }
    }

    public class SquareSceneTrigger : SceneTrigger
    {
        private Vector Size;

        public SquareSceneTrigger(int x, int y, int w, int h)
            : this(x, y, new Vector(w, h))
        {
        }

        public SquareSceneTrigger(int x, int y, Vector size)
            :this(new Vector(x,y), size)
        {
        }

        public SquareSceneTrigger(Vector pos, Vector size)
        {
            Position = size / 2 + pos;
            Size = size;

            var j = new SquareCollisionBox(Size) { Type = CollisionType.Trigger, Style = TriggerStyle.Enter, CollisionLayer = CollisionLayer.Foreground, CollisionObjectType = CollisionObjectType.Static};

            AddPhysicsObject(TRIGGER_INDEX, j);
            
            AddRenderObject("trigger_outline_debug",new BlockGraphic(Size)
            {
                OutlineColor = Color.Green,
                FillColor = Color.Transparent,
                RenderLayer = RenderLayer.LevelObject,
                Debug = true
            });
        }

    }

    public class TriangleSceneTrigger : SceneTrigger
    {
        private Vector Size;

        public TriangleSceneTrigger(Vector a, Vector b, Vector c)
        {

            var j = new TriangleCollisionBox(new Triangle(a, b, c)) { Type = CollisionType.Trigger, Style = TriggerStyle.Enter, CollisionLayer = CollisionLayer.Foreground, CollisionObjectType = CollisionObjectType.Static};

            AddPhysicsObject(TRIGGER_INDEX, j);

            //TODO: Add Triangle Outline

            AddRenderObjects(Helper.GetDebugRenderObjects(this));
        }

    }

    public class CircleSceneTrigger : SceneTrigger
    {
        private int radius;

        public CircleSceneTrigger(int radius)
        {

            var j = new CircleCollisionBox(new Circle(this.Position, radius)) { Type = CollisionType.Trigger, Style = TriggerStyle.Enter, CollisionLayer = CollisionLayer.Foreground, CollisionObjectType = CollisionObjectType.Static};

            AddPhysicsObject(TRIGGER_INDEX, j);

            //TODO: Add Circle Outline

            AddRenderObjects(Helper.GetDebugRenderObjects(this));
        }

    }

}