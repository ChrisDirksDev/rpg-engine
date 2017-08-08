using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using Utility;

namespace Rendering
{
    public static class Helper
    {
        public static Dictionary<string, BaseRenderObject> GetDefaultActorObjects(Vector v)
        {
            var mDic = new Dictionary<string, BaseRenderObject>();

            //Add color block that shows dimensions of this actor
            var cBlock = new BlockGraphic(v)
            {
                FillColor = Color.Red,
                RenderLayer = RenderLayer.Player
            };
            mDic.Add("main_block", cBlock);

            //Add circle to indicate position/center of object
            var cCircle = new BlockGraphic(new Vector(2, 2))
            {
                FillColor = Color.Coral,
                RenderLayer = RenderLayer.Player,
                ZLevel = 2
            };
            mDic.Add("center", cCircle);

            return mDic;
        }

        public static Dictionary<string, BaseRenderObject> GetDefaultSceneObjects(Vector v)
        {
            var mDic = new Dictionary<string, BaseRenderObject>();
            var block = new BlockGraphic(v)
            {
                FillColor = Color.Purple,
                RenderLayer = RenderLayer.LevelObject
            };
            mDic.Add("main_block", block);

            var cCircle = new BlockGraphic(new Vector(2, 2))
            {
                FillColor = Color.Coral,
                RenderLayer = RenderLayer.LevelObject,
                ZLevel = 2
            };
            mDic.Add("center", cCircle);

            return mDic;
        }

        public static Dictionary<string, BaseRenderObject> GetDebugRenderObjects(BaseGameObject o)
        {
            var mDic = new Dictionary<string, BaseRenderObject>();

            var p = new DynamicTextBlock(0, -15, () => "Pos: " + o.Position) {Debug = true};

            mDic.Add("position_text", p);

            return mDic;
        }
    }
}