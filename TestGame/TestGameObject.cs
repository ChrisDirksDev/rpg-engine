using System.Drawing;
using System.Windows;
using Rendering;
using EnginePhysics;
using World;

namespace RpgEngine
{
    public class Platform : SceneObject
    {
        private Vector Size;

        public Platform(float x, float y, float width, float height)   
        {
            Position = new Vector(width, height)/2 + new Vector(x,y);
            Size = new Vector(width, height);

            SceneLayer = SceneLayer.SceneStatic;

            LoadDefaultGraphics();
            LoadDefaultCollision();
        }

        private void LoadDefaultCollision()
        {
            var j = new SquareCollisionBox(Size) { Type = CollisionType.Solid, CollisionLayer = CollisionLayer.Foreground, CollisionObjectType = CollisionObjectType.Static };
            AddPhysicsObject("main", j);
        }
        
        private void LoadDefaultGraphics()
        {
            var block = new BlockGraphic(this.Size)
            {
                FillColor = Color.Purple,
                RenderLayer = RenderLayer.LevelObject
            };

            AddRenderObject("main", block);

            var center = new BlockGraphic(new Vector(2, 2))
            {
                FillColor = Color.Coral,
                RenderLayer = RenderLayer.LevelObject,
                ZLevel = 2
            };

            AddRenderObject("center", center);
        }
    }

    public class MovingPlatform : Platform
    { 
        public MovingPlatform(int x, int y, int width, int height)
            :base(x,y,width,height)
        {
            this.SceneObjectAnimator = new SceneObjectAnimator(this);
            var a = new SceneAnimation() {AnimationType = SceneAnimationType.FullRepeat, RepeatStyle = RepeatProgressionStyle.Circular};
            a.KeyFrames.Add(new SceneAnimationKeyFrame(this) { FrameType = KeyFrameType.Movement, TargetPos = this.Position + new Vector(100, 0), TotalFrames = 100 });
            a.KeyFrames.Add(new SceneAnimationKeyFrame(this) { FrameType = KeyFrameType.Movement, TargetPos = this.Position, TotalFrames = 100 });
            this.SceneObjectAnimator.AddAnimation("BackForth", a);
            this.SceneObjectAnimator.Play("BackForth");
        }

    }

    public class TestTrigger : SquareSceneTrigger
    {

        public TestTrigger(int x, int y, int w, int h)
            :base(x,y,w,h)
        {
            SetTriggerStyle(TriggerStyle.EnterExit);
        }
        
        protected override void EnterTriggerAction(SceneObject Source)
        {
            
            if (!(Source is Player))
                return;
        
            var p = (Player) Source;
            p.ChangeColor(Color.Brown);

            RenderObjects["trigger_outline_debug"].OutlineColor = Color.Red;
        }

        protected override void ExitTriggerAction(SceneObject Source)
        {
            if (!(Source is Player))
                return;

            var p = (Player)Source;
            p.ChangeColor(Color.CornflowerBlue);

            RenderObjects["trigger_outline_debug"].OutlineColor = Color.Aquamarine;
        } 
    }
}