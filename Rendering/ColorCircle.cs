using System;
using System.Drawing;
using System.Windows;

namespace Rendering
{
    [Serializable]
    public class ColorCircle : BaseRenderObject
    {
        public ColorCircle(int x, int y, float radiusX, float radiusY)
        {
            RenderShape = RenderShape.Circle;
            Size = new Vector(radiusX, radiusY);
            FillColor = Color.Red;
            Transform.SetPosition(x, y);
        }
    }
}