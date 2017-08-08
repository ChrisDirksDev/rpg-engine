using System;

namespace EnginePhysics
{
    public class CollisionEventArgs : EventArgs
    {
        private readonly string _eventInfo;
        public CollisionResolutionData CData;
        public CollisionType CType;
        public TriggerStyle TStyle;

        public CollisionEventArgs(string text)
        {
            _eventInfo = text;
            CData = new CollisionResolutionData();
        }

        public string GetInfo()
        {
            return _eventInfo;
        }
    }
}