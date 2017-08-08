using System;
using System.Windows;
using System.Windows.Forms;
using EngineName;
using EnginePhysics;
using World;


namespace RpgEngine
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {

            using (var game = new Game(800,600))
            {
                Initiallize(game);
                game.Run(60, 60);
            }
        }

        private static void Initiallize(Game myTestGame)
        {
            LoadTest(myTestGame);
        }

        private static void LoadTest(Game myTestGame)
        {

            var mWorld = myTestGame.World;
            //mWorld.UserInterface = new TestLevelUserInterface() { Enabled = true };

            var mLevel = GenerateTestScene(myTestGame.Width, myTestGame.Height);

            mWorld.AddScene("Test", mLevel);
            mWorld.SetActive("Test");
        }

        private static Scene GenerateTestScene(int x, int y)
        {
            var mLevel = new Scene(1500, 800, "Test") {Camera = new Camera(x, y)};
            mLevel.SceneDefaultPhysics = new GravityEffect() {AccelerationY = 0.04f};

            mLevel.Camera.SetStyle(CameraTrakingStyle.Locked);
            
            var mPlat = new MovingPlatform(400, 400, 100, 20) {Enabled = true};
            var mPlayer = new Player(550, 550, 20, 20) { Enabled = true };
            mLevel.AddSceneObject("Player", mPlayer);
            mLevel.AddSceneObject("Plat1", mPlat);

            var floor = new Platform(-5, 750, 1510, 15) { Enabled = true };
            mLevel.AddSceneObject("Floor", floor);

            var roof = new Platform(-5, 0, 1510, 15) { Enabled = true };
            mLevel.AddSceneObject("Roof", roof);

            var leftWall = new Platform(-5, 0, 15, 780) { Enabled = true };
            mLevel.AddSceneObject("LeftWall", leftWall);

            var rightWall = new Platform(1470, 0, 15, 780) { Enabled = true };
            mLevel.AddSceneObject("RightWall", rightWall);

            var T = new TestTrigger(100, 500, 30, 30) { Enabled = true };
            mLevel.AddSceneObject("TriggerB1", T);

            mLevel.Camera.SetTarget(mLevel.GetSceneObject("Player"));
            mLevel.Camera.SetScene(mLevel);


            //var Y = new SquareTrigger(new Vector(700, 400), new Vector(100, 50), TriggerStyle.Enter) { Enabled = true };
            //Y.SceneLayer = SceneLayer.SceneStatic;
            //Y.SetCallback();

            return mLevel;
        }
    }
}