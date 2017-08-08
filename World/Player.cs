using System.Drawing;
using System.Windows;
using EnginePhysics;
using Rendering;

namespace World
{
    public class Player : Actor
    {
        public Player(float posX, float posY, int width, int height)
        {
            Position = new Vector(width, height) / 2 + new Vector(posX, posY);
            Size = new Vector(width, height);

            ApplyScenePhysics = true;
            LoadCollision();
            LoadGraphics();
        }

        private Vector Size { get; }

        private void LoadCollision()
        {
            var j = new SquareCollisionBox(Size)
            {
                Type = CollisionType.Solid,
                CollisionLayer = CollisionLayer.Foreground,
                CollisionObjectType = CollisionObjectType.Active
            };
            AddPhysicsObject("main", j);
        }

        private void LoadGraphics()
        {
            var block = new BlockGraphic(Size)
            {
                FillColor = Color.Red,
                RenderLayer = RenderLayer.Player
            };

            AddRenderObject("main", block);

            var center = new BlockGraphic(new Vector(2, 2))
            {
                FillColor = Color.Coral,
                RenderLayer = RenderLayer.Player,
                ZLevel = 2
            };

            AddRenderObject("center", center);
        }


        private void Bump(SceneObject source)
        {
        }

        public override void Update(object[] args)
        {
            base.Update(args);
        }

        public void ChangeColor(Color c)
        {
            RenderObjects["main"].FillColor = c;
        }
    }
}