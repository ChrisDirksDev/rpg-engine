using System;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Rendering;
using Utility;

namespace EngineName
{
    public sealed class Game : GameWindow
    {
        //Object that handles rendering for our game
        private readonly RenderMachine _renderMachine;

        //Time Keeping
        private readonly Stopwatch _stopwatch;

        private float deltaTime;
        private float lastFrame;
        public World.World World;

        public Game(int w, int h)
            : base(w, h)
        {
            Size = new Size(w, h);

            _renderMachine = new RenderMachine(w, h);
            _stopwatch = new Stopwatch();

            World = new World.World();
        }

        private void Update(object[] args)
        {
            World.Update(args);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _stopwatch.Start();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            _renderMachine.SetSize(ClientRectangle.Width, ClientRectangle.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var currentTicks = (float) _stopwatch.Elapsed.TotalSeconds * 1000;
            deltaTime = currentTicks - lastFrame;
            lastFrame = currentTicks;

            base.OnUpdateFrame(e);

            var input = ProcessInput();

            Update(new object[] {deltaTime, input});
        }

        private Keys ProcessInput()
        {
            if (Keyboard[Key.Escape])
                Exit();

            var keys = Keys.None;
            if (Keyboard[Key.W])
                keys = keys | Keys.W;
            if (Keyboard[Key.S])
                keys = keys | Keys.S;
            if (Keyboard[Key.A])
                keys = keys | Keys.A;
            if (Keyboard[Key.D])
                keys = keys | Keys.D;

            return keys;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Render();
        }

        private void Render()
        {
            _renderMachine.BeginFrame(World.GetActiveCamera());
            World.Render(_renderMachine);
            SwapBuffers();
        }
    }
}