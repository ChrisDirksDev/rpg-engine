using Rendering;
using Utility;

namespace World
{
    public abstract class Actor : SceneObject
    {
        protected Actor()
        {
            actorController = new Controller();
            AddRenderObject("text_positon", new DynamicTextBlock(0, -25, () => $"Pos: {Position}") {Debug = true});
            AddRenderObject("text_velocity", new DynamicTextBlock(0, -15, () => $"Vel: {Velocity}") {Debug = true});
        }

        private Controller actorController { get; }

        public override void Update(object[] args)
        {
            Velocity = actorController.DoStep((float) args[0], (Keys) args[1], Velocity);

            base.Update(args);
        }
    }
}