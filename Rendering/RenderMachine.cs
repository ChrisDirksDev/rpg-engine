using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Rendering
{
    public sealed class RenderMachine : IDisposable
    {
        private Vector3 cameraPos;
        private readonly Vector3 cameraFront = Vector3.UnitZ;
        private readonly Vector3 cameraUp = Vector3.UnitY;
        
        private TargetGraphics TargetGraphicsMode { get; set; }

        private int QuadVAO, LineVAO;

        //
        private int DefaultShader;

        private float Width, Height;

        public RenderMachine(float width, float height)
        {
            Width = width;
            Height = height;
            
            SetTargetGraphics(TargetGraphics.OpenGL);

            Init();
        }

        private void Init()
        {
            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            //Blocks
            var blockVBO = GL.GenBuffer();
            QuadVAO = GL.GenVertexArray();

            float[] quadVerts =
            {
                0.0f, 1.0f,
                1.0f, 0.0f,
                0.0f, 0.0f,

                0.0f, 1.0f,
                1.0f, 1.0f,
                1.0f, 0.0f
            };

            GL.BindVertexArray(QuadVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, blockVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * quadVerts.Length, quadVerts, BufferUsageHint.StaticDraw);
            
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.BindVertexArray(0);
            //

            //Lines
            //var lineVBO = GL.GenBuffer();
            //LineVAO = GL.GenVertexArray();

            //float[] lineVert =
            //{
            //    0.0f, 0.0f,
            //    1.0f, 1.0f
            //};

            //GL.BindBuffer(BufferTarget.ArrayBuffer, lineVBO);
            //GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * lineVert.Length, lineVert, BufferUsageHint.StaticDraw);

            //GL.BindVertexArray(LineVAO);
            //GL.EnableVertexAttribArray(0);
            //GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.BindVertexArray(0);
            //
        }


        public void BeginFrame(Vector cameraVector)
        {
            SetCameraPosition(cameraVector);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.UseProgram(DefaultShader);
            SetView();
        }

        private void SetView()
        { 
            var viewMatrix = Matrix4.LookAt(
                cameraPos,
                cameraPos - cameraFront,
                cameraUp);
            var view = GL.GetUniformLocation(DefaultShader, "view");
            GL.UniformMatrix4(view, false, ref viewMatrix);
        }

        public void DrawSolidRect(Vector pos, Vector size, Color fColor)
        {
            
            var modelMatrix = Matrix4.Identity;
            Matrix4.CreateTranslation((float)pos.X, (float)pos.Y, 0.0f, out Matrix4 trans);
            Matrix4.CreateScale((float)size.X, (float)size.Y, 1.0f, out Matrix4 scale);
            modelMatrix *= scale;
            modelMatrix *= trans;

            var model = GL.GetUniformLocation(DefaultShader, "model");
            GL.UniformMatrix4(model, false, ref modelMatrix);
            var color = GL.GetUniformLocation(DefaultShader, "ourColor");

            if (fColor != Color.Transparent)
            {
                var colorVec = new Vector4(fColor.R / 255.0f, fColor.G / 255.0f, fColor.B / 255.0f, fColor.A / 255.0f);
                GL.Uniform4(color, colorVec);
                GL.BindVertexArray(QuadVAO);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                GL.End();
            }
        }

        public void DrawLines(Vector[] points, Color color)
        {
            var model = GL.GetUniformLocation(DefaultShader, "model");
            var mPoints = points;
            var colorVec = new Vector4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
            var colorU = GL.GetUniformLocation(DefaultShader, "ourColor");
            GL.Uniform4(colorU, colorVec);

            for (var i = 0; i < mPoints.Length; i+=2)
            {
                Vector mPoint;
                Matrix4 trans;

                mPoint = mPoints[i];
                trans = Matrix4.CreateTranslation((float)mPoint.X, (float)mPoint.Y, 0.0f);
                GL.UniformMatrix4(model, false, ref trans);

                var diff = mPoints[i + 1] - mPoints[i];
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(0.0f, 0.0f);
                GL.Vertex2((float)diff.X, (float)diff.Y);
                GL.End();
            }

            GL.BindVertexArray(0);
        }

        private void OpenGLDrawBlock(RenderData item)
        {
            //GL.BindBuffer(BufferTarget.ArrayBuffer, item.ObjectIndex);
            Matrix4.CreateTranslation((float)item.Position.X, (float)item.Position.Y, 0.0f, out Matrix4 trans);
            var model = GL.GetUniformLocation(DefaultShader, "model");
            GL.UniformMatrix4(model, false, ref trans);

            var color = GL.GetUniformLocation(DefaultShader, "ourColor");

            if (item.FillColor != Color.Transparent)
            {
                var colorVec = new Vector4(item.FillColor.R/255.0f, item.FillColor.G/255.0f, item.FillColor.B/255.0f, item.FillColor.A/255.0f);
                GL.Uniform4(color,colorVec);

                GL.Begin(PrimitiveType.Quads);
            
                GL.Vertex3(item.Size.X, 0.0f, 0.0f);
                GL.Vertex3(item.Size.X, item.Size.Y, 0.0f);
                GL.Vertex3(0.0f,item.Size.Y, 0.0f);
                GL.Vertex3(0.0f, 0.0f, 0.0f);
                GL.End();
                
            }

            if (item.OutlineColor != Color.Transparent)
            {
                var colorVec = new Vector4(item.OutlineColor.R/ 55.0f, item.OutlineColor.G/255.0f, item.OutlineColor.B/255.0f, item.OutlineColor.A/255.0f);
                GL.Uniform4(color, colorVec);
                GL.Begin(PrimitiveType.Lines);

                GL.Vertex3(item.Size.X, 0.0f, 0.0f);
                GL.Vertex3(item.Size.X, item.Size.Y, 0.0f);
                GL.Vertex3(item.Size.X, item.Size.Y, 0.0f);
                GL.Vertex3(0.0f, item.Size.Y, 0.0f);
                GL.Vertex3(0.0f, item.Size.Y, 0.0f);
                GL.Vertex3(0.0f, 0.0f, 0.0f);
                GL.Vertex3(0.0f, 0.0f, 0.0f);
                GL.Vertex3(item.Size.X, 0.0f, 0.0f);
                GL.End();
            }
        }

        private void SetTargetGraphics(TargetGraphics targetGraphics)
        {
            TargetGraphicsMode = targetGraphics;

            switch (targetGraphics)
            {
                case TargetGraphics.Gdi:
                    break;
                case TargetGraphics.Dx9:
                    break;
                case TargetGraphics.Dx11:
                    #region  DX11
                    //{
                    //    // Create swap chain description
                    //    var swapChainDesc = new SwapChainDescription()
                    //    {
                    //        BufferCount = 2, Usage = Usage.RenderTargetOutput, OutputHandle = RenderForm.Handle, IsWindowed = true, ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm), SampleDescription = new SampleDescription(1, 0), Flags = SwapChainFlags.AllowModeSwitch, SwapEffect = SwapEffect.Sequential
                    //    };

                    //    // Create swap chain and Direct3D device
                    //    // The BgraSupport flag is needed for Direct2D compatibility otherwise RenderTarget.FromDXGI will fail!
                    //    Device device;
                    //    SwapChain swapChain;
                    //    Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, swapChainDesc, out device, out swapChain);

                    //    Dx11Device = device;
                    //    _dx11SwapChain = swapChain;
                    //    // Get back buffer in a Direct2D-compatible format (DXGI surface)
                    //    Surface backBuffer = Surface.FromSwapChain(swapChain, 0);
                    //    BackBuffer = backBuffer;
                    //    RenderTarget renderTarget;

                    //    // Create Direct2D factory
                    //    using (var factory = new Factory())
                    //    {
                    //        // Get desktop DPI
                    //        var dpi = factory.DesktopDpi;

                    //        // Create bitmap render target from DXGI surface
                    //        renderTarget = RenderTarget.FromDXGI(factory, backBuffer, new RenderTargetProperties()
                    //        {
                    //            HorizontalDpi = dpi.Width, VerticalDpi = dpi.Height, MinimumFeatureLevel = SlimDX.Direct2D.FeatureLevel.Default, PixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Ignore), Type = RenderTargetType.Default, Usage = RenderTargetUsage.None
                    //        });
                    //    }

                    //    var spritefactory = new SpriteFactory(@"\sprites");
                    //        spritefactory.LoadSpriteSheet("Test.bmp",Dx11Device);

                    //    _renderTarget = renderTarget;
                    //}
                    
                    #endregion
                    break;
                case TargetGraphics.OpenGL:
                    InitDefaultShaders();
                    GL.UseProgram(DefaultShader);
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    //GL.Enable(EnableCap.DepthTest);
                    var projMatrix = Matrix4.CreateOrthographic(Width, Height, -1.0f, 1.0f);

                    //Flip out image right side up
                    projMatrix *= Matrix4.CreateScale(new Vector3(1.0f, -1.0f, 1.0f));

                    var projection = GL.GetUniformLocation(DefaultShader, "projection");
                    GL.UniformMatrix4(projection, false, ref projMatrix);
                    GL.LineWidth(2.0f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetGraphics), targetGraphics, null);
            }
            
        }

        private string[] LoadShaderFromResource(string resource)
        {
            return new string[] { Properties.Resource.ResourceManager.GetString(resource)};
        }

        private void InitDefaultShaders()
        {
            #region VertexShader
            var vShaderData = LoadShaderFromResource("VertexShader");

            var vShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vShader, 1, vShaderData, (int[])null);
            GL.CompileShader(vShader);

            int success;

            GL.GetShader(vShader, ShaderParameter.CompileStatus, out success);

            if (success == 0)
            {
                var log = GL.GetShaderInfoLog(vShader);
            }
            #endregion

            #region FragmentShader
            var fShaderData = LoadShaderFromResource("FragmentShader");

            var fShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fShader, 1, fShaderData, (int[]) null);
            GL.CompileShader(fShader);

            GL.GetShader(fShader, ShaderParameter.CompileStatus, out success);

            if (success == 0)
            {
                var log = GL.GetShaderInfoLog(fShader);
            }
            #endregion

            DefaultShader = GL.CreateProgram();

            GL.AttachShader(DefaultShader, vShader);
            GL.AttachShader(DefaultShader, fShader);
            GL.LinkProgram(DefaultShader);

            GL.GetProgram(DefaultShader, GetProgramParameterName.LinkStatus, out success);

            if (success == 0)
            {
                var log = GL.GetProgramInfoLog(DefaultShader);
            }

            GL.DeleteShader(vShader);
            GL.DeleteShader(fShader);
        }

        //private void Dx11RenderPrimative(RenderData prim)
        //{
        //    try
        //    {
        //        switch (prim.RenderShape)
        //        {

        //            case RenderShape.Block:
        //                {
        //                    var mVec = prim.Position;
        //                    var mPoint = PointF.Add(new PointF((float) mVec.X, (float) mVec.Y),
        //                        new SizeF((float) _translationMod.X, (float) _translationMod.Y));

        //                    var mRect = new RectangleF(mPoint.X, mPoint.Y, (float) prim.Size.X, (float) prim.Size.Y);

        //                    var solid = new SolidColorBrush(_renderTarget, new Color4(prim.FillColor));
        //                    var outline = new SolidColorBrush(_renderTarget, new Color4(prim.OutlineColor));

        //                    _renderTarget.FillRectangle(solid, mRect);
        //                    _renderTarget.DrawRectangle(outline, mRect);

        //                    solid.Dispose();
        //                    outline.Dispose();
        //                }
        //                break;
        //            case RenderShape.Circle:
        //                {
        //                    var mVec = prim.Position;
        //                    var mPoint = PointF.Add(new PointF((float)mVec.X, (float)mVec.Y), new SizeF((float)_translationMod.X, (float)_translationMod.Y));

        //                    var solid = new SolidColorBrush(_renderTarget, new Color4(prim.FillColor));

        //                    var e = new Ellipse() { Center = mPoint, RadiusX = (float)prim.Size.X, RadiusY = (float)prim.Size.Y };
        //                    _renderTarget.FillEllipse(solid, e);

        //                    solid.Dispose();
        //                }
        //                break;
        //            case RenderShape.Triangle:
        //                break;
        //            case RenderShape.None:
        //                if (prim.RenderType == RenderType.Text)
        //                {
        //                    var mVec = prim.Position;
        //                    var mPoint = PointF.Add(new PointF((float)mVec.X, (float)mVec.Y), new SizeF((float)_translationMod.X, (float)_translationMod.Y));


        //                    var solid = new SolidColorBrush(_renderTarget, new Color4(prim.FillColor));

        //                    //SlimDX.DirectWrite.TextFormat tForm = new SlimDX.DirectWrite.TextFormat(new SlimDX.DirectWrite.Factory(), "Arial", SlimDX.DirectWrite.FontWeight.Normal, SlimDX.DirectWrite.FontStyle.Normal, SlimDX.DirectWrite.FontStretch.Normal, 11.0f, ident);
        //                    var mRect = new RectangleF(mPoint.X, mPoint.Y, 500, 10);

        //                    _renderTarget.TextAntialiasMode = TextAntialiasMode.Grayscale;
        //                    _renderTarget.DrawText(prim.Text, _textFormats["Ariel"], mRect, solid);

        //                    solid.Dispose();
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //}
 

        //private void SwapBuffers()
        //{
        //    if (_backRenderList == _bufferRenderListA)
        //    {
        //        _backRenderList = _bufferRenderListB;
        //        _frontRenderList = _bufferRenderListA;
        //    }
        //    else if (_backRenderList == _bufferRenderListB)
        //    {
        //        _backRenderList = _bufferRenderListA;
        //        _frontRenderList = _bufferRenderListB;
        //    }
        //    _backRenderList.Clear();
        //}

        public void SetSize(float w, float h)
        {
            Width = w;
            Height = h;
        }

        public void SetCameraPosition(Vector s)
        {
            cameraPos = new Vector3((float)s.X, (float)s.Y, 0.0f);
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RenderMachine() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}