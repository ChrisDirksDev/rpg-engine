using Utility;

namespace World
{
    public abstract class ScenePhysics
    {
        #region Gravity

        public float AccelerationX;
        public float AccelerationY;

        #endregion
    }

    /// <summary>
    /// A scene affect that applies gravity to all controllers within the scene
    /// </summary>
    public class GravityEffect : ScenePhysics
    {
        public GravityEffect()
        {
            AccelerationX = 0.0f;
            AccelerationY = 0.9f;
        }
    }

}
