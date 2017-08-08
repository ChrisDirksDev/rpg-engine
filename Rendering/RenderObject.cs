using System.Drawing;
using System.Windows;
using Utility;

namespace Rendering
{
    public abstract class BaseRenderObject
    {
        public bool Debug;
        public Color FillColor;
        public int ObjectIndex;
        public Color OutlineColor;
        public Transform ParentTransform;
        public RenderLayer RenderLayer;
        public RenderShape RenderShape;
        public RenderType RenderType;
        public Vector Size;
        public Transform Transform;
        public int ZLevel;


        protected BaseRenderObject()
        {
            Transform = new Transform();
            Debug = false;
            RenderLayer = RenderLayer.None;
            ZLevel = 1;
            ParentTransform = null;
        }

        public virtual void Render(RenderMachine renderMachine)
        {
        }

        public virtual RenderData GetRenderData()
        {
            var data = new RenderData();
            return data;
        }

        public Vector GetPosition()
        {
            return ParentTransform == null ? Transform.Position : Transform.Position + ParentTransform.Position;
        }
    }
}