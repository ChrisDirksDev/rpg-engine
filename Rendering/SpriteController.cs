using System.Collections.Generic;

namespace Rendering
{
    public class SpriteController
    {
        public Dictionary<string, DynamicSprite> Animations;
        private bool running;

        public string CurrentAnimation { get; private set; }

        public void RunAnimation(string identifier)
        {
            CurrentAnimation = identifier;
        }

        public void AdvanceFrame()
        {
        }
    }
}