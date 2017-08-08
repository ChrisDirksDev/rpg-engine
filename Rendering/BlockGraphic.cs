using System.Drawing;
using System.Windows;

namespace Rendering
{
    /// <summary>
    ///     A simple renderable object for displaying square color areas
    /// </summary>
    public class BlockGraphic : BaseRenderObject
    {
        public BlockGraphic(double x, double y, Vector size)
            : this(new Vector(x, y), size)
        {
        }

        public BlockGraphic(Vector size)
        {
            RenderShape = RenderShape.Block;
            Size = size;
            OutlineColor = Color.Black;
            Transform.SetPosition(size / -2);
        }

        public BlockGraphic(Vector p, Vector size)
        {
            RenderShape = RenderShape.Block;
            Size = size;
            OutlineColor = Color.Black;
            Transform.SetPosition(p);
        }

        public override RenderData GetRenderData()
        {
            var data = new RenderData
            {
                Position = GetPosition(),
                Size = Size,
                RenderLayer = RenderLayer,
                RenderShape = RenderShape,
                RenderType = RenderType,
                FillColor = FillColor,
                OutlineColor = OutlineColor,
                ZIndex = ZLevel
            };
            return data;
        }

        public override void Render(RenderMachine renderMachine)
        {
            renderMachine.DrawSolidRect(GetPosition(), Size, FillColor);
        }
    }
}