using System.Drawing;
using System.Windows;

namespace Rendering
{
    public struct RenderData
    {
        public RenderType RenderType;
        public RenderShape RenderShape;
        public Color OutlineColor;
        public Color FillColor;
        public Vector Size;
        public Vector Position;
        public RenderLayer RenderLayer;
        public string Text;
        public bool debug;
        public int ObjectIndex;
        public int ZIndex;

        public RenderData(double x, double y, Vector size)
            : this(new Vector(x, y), size)
        {
        }

        public RenderData(Vector p, Vector size)
        {
            Position = p;
            Size = size;
            FillColor = Color.Pink;
            OutlineColor = Color.Black;

            RenderType = RenderType.None;
            RenderShape = RenderShape.None;
            RenderLayer = RenderLayer.None;
            ObjectIndex = -1;
            ZIndex = 1;
            Text = "";

            debug = false;
        }
    }
}