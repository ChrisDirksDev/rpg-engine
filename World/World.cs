using System;
using System.Collections.Generic;
using System.Windows;
using Rendering;

namespace World
{
    public class World
    {
        private readonly Dictionary<string, Scene> _sceneList;

        private Scene ActiveScene;

        public World()
        {
            _sceneList = new Dictionary<string, Scene>();
        }

        public void Update(object[] args)
        {
            ActiveScene.Update(args);
        }

        public void AddScene(string name, Scene l)
        {
            _sceneList.Add(name, l);
        }

        public void SetActive(string levelName)
        {
            if (_sceneList.ContainsKey(levelName))
                ActiveScene = _sceneList[levelName];
            else
                Console.WriteLine($@"Invalid active scene specified. Param:{levelName}");
        }

        public void Render(RenderMachine renderMachine)
        {
            ActiveScene.Render(renderMachine);
        }

        public Vector GetActiveCamera()
        {
            return ActiveScene.Camera.GetPosition();
        }
    }
}