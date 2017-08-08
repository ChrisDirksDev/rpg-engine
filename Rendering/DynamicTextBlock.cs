using System;
using System.Drawing;

namespace Rendering
{
    /// <summary>
    ///     Static Text Block that does not change its displayed text
    /// </summary>
    public class DynamicTextBlock : BaseRenderObject
    {
        private readonly Func<string> _updater;

        public DynamicTextBlock(int x, int y, Func<string> u)
        {
            _updater = u;
            FillColor = Color.White;
            RenderLayer = RenderLayer.Ui;
            RenderType = RenderType.Text;
            Transform.SetPosition(x, y);
        }

        private string Text => _updater();

        public override RenderData GetRenderData()
        {
            var data = base.GetRenderData();
            data.Text = Text;
            data.RenderType = RenderType.Text;
            return data;
        }
    }
}