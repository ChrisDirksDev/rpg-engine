using System;
using System.Windows;
using Utility;

//using System.Drawing;


namespace World
{
    public class Camera
    {
        private readonly Transform _transform;

        //stores edge data
        private float _left, _right, _top, _bottem;

        private CameraTrakingStyle _style;
        private SceneObject _targetObj;

        private Vector _translationOrigin;

        public Vector RectSize;

        public Vector SceneSize;

        public Camera(int x, int y)
        {
            RectSize = new Vector(x, y);
            _transform = new Transform();
        }

        public Vector GetPosition()
        {
            return _transform.Position;
        }

        public void SetStyle(CameraTrakingStyle style)
        {
            _style = style;
        }

        public void SetTarget(SceneObject obj)
        {
            _targetObj = obj;
        }

        /// <summary>
        ///     Called when the scene is initally set or if the scene is resized
        /// </summary>
        /// <param name="s">The Scene</param>
        public void SetScene(Scene s)
        {
            SceneSize = s.Size;

            var distX = (float) RectSize.X / 2;
            var distY = (float) RectSize.Y / 2;

            _left = distX;
            _right = (float) SceneSize.X - distX;
            _top = distY;
            _bottem = (float) SceneSize.Y - distY;

            _translationOrigin = new Vector(_left, _top);
        }

        public void Update(object[] args)
        {
            switch (_style)
            {
                case CameraTrakingStyle.Fixed:
                    break;
                case CameraTrakingStyle.Locked:
                    DoLockedStep();
                    //UpdateTranslation();
                    break;
                case CameraTrakingStyle.Float:
                    break;
                case CameraTrakingStyle.Free:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void DoLockedStep()
        {
            if (_targetObj == null) return;

            //Grab the tracked objects position
            var p = _targetObj.Position;

            float newX, newY;

            //Check if our tracked object is too close to a boundy edge
            //If we are too close, we lock the camera to set distances from the edge
            //If not, just follow the target with no smoothing
            if (p.X < _left)
                newX = _left;
            else if (p.X > _right)
                newX = _right;
            else
                newX = (float) p.X;

            if (p.Y < _top)
                newY = _top;
            else if (p.Y > _bottem)
                newY = _bottem;
            else
                newY = (float) p.Y;


            _transform.SetPosition(newX, newY);
        }

        public Vector GetWorldTranslation()
        {
            return Vector.Subtract(_translationOrigin, _transform.Position);
        }
    }

    public enum CameraTrakingStyle
    {
        Fixed,
        Locked,
        Float,
        Free
    }
}