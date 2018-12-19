using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine {
    public class IEngine : GameWindow {
        static IEngine instance = null;
        protected IEngine() {
            instance = this;
            Title = "Visualization Engine";
            GL.ClearColor(Color.CornflowerBlue);
            SetUp();
            int screenX = Screen.PrimaryScreen.Bounds.Width;
            int screenY = Screen.PrimaryScreen.Bounds.Height;
            Location = new Point(screenX / 2 - Size.Width / 2, screenY / 2 - Size.Height / 2);

        }
        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Draw();
            SwapBuffers();

        }

        public static IEngine The {
            get {
                if (instance == null) {
                    instance = new IEngine();
                }
                return instance;
            }
        }

        public virtual void SetUp() {

        }
        public virtual void Draw() {
            ShowAxis();
        }
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
        }

        public static void Canvas(int width, int height) {
            The.ClientSize = new Size(width, height);
            GL.Viewport(0,0,The.ClientSize.Width,The.ClientSize.Height);
            GL.Ortho(0, The.ClientSize.Width, 0, The.ClientSize.Height, -1, 1);
        }
        public static void BackgroundColor(byte color) {
            GL.ClearColor(Color.FromArgb(color, color, color));
        }
        public static void SetColor(Color color) {
            GL.Color3(
                (float)color.R / 255,
                (float)color.G / 255,
                (float)color.B / 255);
        }
        public static void SetFrameRate(int frameRate) {
            The.TargetRenderFrequency = frameRate;
        }
        public  void ShowAxis() {
            float currentLineWidth = GL.GetFloat(GetPName.LineWidth);
            GL.LineWidth(2.5f);
            currentLineWidth = GL.GetFloat(GetPName.LineWidth);
            GL.Begin(PrimitiveType.Lines);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1, 0, 0);

            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 1, 0);

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 1);
            GL.End();
        }
        public static void DrawRectangle(int x, int y, int w, int h) {
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(x, y);
            GL.Vertex2(x, y + h);
            GL.Vertex2(x + w, y + h);
            GL.Vertex2(x + w, y);
            GL.End();
        }
        public static void DrawPrism(int x, int y,int z, double w, double h, double d) {
            GL.Begin(PrimitiveType.Polygon);
            // Front
            GL.Vertex3(x    , y    , z);
            GL.Vertex3(x + w, y    , z);
            GL.Vertex3(x + w, y + h, z);
            GL.Vertex3(x    , y + h, z);

            // Back
            GL.Vertex3(x    , y    , z + d);
            GL.Vertex3(x + w, y    , z + d);
            GL.Vertex3(x + w, y + h, z + d);
            GL.Vertex3(x    , y + h, z + d);

            GL.End();
        }
    }
}