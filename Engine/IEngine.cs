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
            GL.Enable(EnableCap.DepthTest);
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
            GL.Ortho(0, The.ClientSize.Width, 0, The.ClientSize.Height, 800, -800);
        }
        public static void Camara(float x, float y, float z, float tx, float ty, float tz)
        {
            Matrix4 modelview = Matrix4.LookAt(new Vector3(x, y, z), new Vector3(0,0,-1), Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
        }

        public static void BackgroundColor(byte color) {
            GL.ClearColor(Color.FromArgb(color, color, color));
        }
        public static void RotateY(float angDeg)
        {
            GL.Rotate(angDeg, 0.0f, 1.0f, 0.0f);
        }
        public static void RotateX(float angDeg)
        {
            GL.Rotate(angDeg, 1.0f, 0.0f, 0.0f);
        }
        public static void Translate(float dx,float dy, float dz)
        {
            GL.Translate(dx, dy, dz);
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
            float size = 100.0f;
            GL.LineWidth(2.5f);
            GL.Begin(PrimitiveType.Lines);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(size, 0, 0);

            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, size, 0);

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, size);
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
        public static void DrawCube(int x, int y, int z, int w, int h, int d) {
            GL.Begin(PrimitiveType.Quads);

            Vector3[] v = new Vector3[]
            {
                new Vector3(x + 0 * w, y + 0 * h, z + 0 * d),
                new Vector3(x + 1 * w, y + 0 * h, z + 0 * d),
                new Vector3(x + 1 * w, y + 0 * h, z + 1 * d),
                new Vector3(x + 0 * w, y + 0 * h, z + 1 * d),
                new Vector3(x + 0 * w, y + 1 * h, z + 0 * d),
                new Vector3(x + 1 * w, y + 1 * h, z + 0 * d),
                new Vector3(x + 1 * w, y + 1 * h, z + 1 * d),
                new Vector3(x + 0 * w, y + 1 * h, z + 1 * d)
            };

            int[] bottom = new int[] { 0, 1, 2, 3 };
            int[] top = new int[] { 4, 5, 6, 7 };
            int[] front = new int[] { 0, 1, 5, 4 };
            int[] right = new int[] { 1, 2, 6, 5 };
            int[] back = new int[] { 2, 3, 7, 6 };
            int[] left = new int[] { 3, 0, 4, 7 };

            List<int[]> faces = new List<int[]>();
            faces.Add(bottom);
            faces.Add(top);
            faces.Add(front);
            faces.Add(right);
            faces.Add(back);
            faces.Add(left);

            float count = 0;
            foreach (int [] face in faces)
            {
                count++;
                GL.Color3(count * 1 / 8f,1-1/8f * count, 0.0f);
                foreach (int i in face)
                {
                    GL.Vertex3(v[i]);
                }
            }

    

            GL.End();
        }
    }
}