using System.Collections.Generic;

namespace Rendering
{
    /// <summary>
    ///     An object that describes a collection of sprites to form a single animation
    /// </summary>
    public class DynamicSprite : BaseRenderObject
    {
        public int Framerate;

        public DynamicSprite(Dictionary<int, SpriteAnimationData> dictionary, int x = 0, int y = 0)
        {
            AnimationData = dictionary;
        }

        public Dictionary<int, SpriteAnimationData> AnimationData { get; set; }
    }
}