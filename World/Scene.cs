using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EnginePhysics;
using Rendering;

namespace World
{
    public class Scene
    {
        private readonly Dictionary<string, SceneObject> _sceneItems;

        public Camera Camera;

        private string Name;

        public ScenePhysics SceneDefaultPhysics;

        public Scene(int x, int y, string name)
        {
            Size = new Vector(x, y);
            Name = name;

            _sceneItems = new Dictionary<string, SceneObject>();
        }

        public Vector Size { get; }

        internal void Update(object[] args)
        {
            //Parents get updated first, then update their children
            //Position of parent affects all children
            var sortedObjs = _sceneItems.Values.Where(x => !x.HasParent).OrderBy(x => x.Mounted).ToList();
            sortedObjs.ForEach(x => x.Update(args));

            RunPhysicsLogic();

            Camera.Update(args);
        }

        private void RunPhysicsLogic()
        {
            var mPhys = new List<BasePhysObject>(GetCollisionItems());

            //Get our static objects such as platforms and other stationary interactables
            var mStatic =
                new List<BasePhysObject>(mPhys.Where(x => x.CollisionObjectType == CollisionObjectType.Static));

            //Get our Active objects such as the player and npcs
            var mActive =
                new List<BasePhysObject>(mPhys.Where(x => x.CollisionObjectType == CollisionObjectType.Active));

            //Check our actors for collision with eachother
            Physics.CheckResolveCollision(mActive);

            //Check each actor against all other static objects
            mActive.ForEach(x => Physics.CheckResolveCollision(x, mStatic));

            //Checks for object exit triggers and such
            mPhys.ForEach(x => x.EndFrameCheck());
        }

        public void AddSceneObject(string identifier, SceneObject obj)
        {
            obj.ScenePhysics = SceneDefaultPhysics;
            _sceneItems.Add(identifier, obj);
        }

        public SceneObject GetSceneObject(string name)
        {
            return _sceneItems[name];
        }

        private IEnumerable<BasePhysObject> GetCollisionItems()
        {
            var mList = new List<BasePhysObject>();

            foreach (var item in _sceneItems.Values)
                mList.AddRange(item.GetCollisionItems());

            return mList;
        }

        public void Render(RenderMachine renderMachine)
        {
            foreach (var item in _sceneItems.Values)
                item.Render(renderMachine);
        }
    }
}