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
        static Random rnd = new Random();
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
            //ShowAxis();
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
            GL.Color4(
                (float)color.R / 255,
                (float)color.G / 255,
                (float)color.B / 255,
                (float)color.A / 255);
        }
        public static void SetLineWidth(float width) {
            GL.LineWidth(width);
        }
        public static void SetPointSize(float size) {
            GL.PointSize(size);
        }
        public static void SetFrameRate(int frameRate) {
            The.TargetRenderFrequency = frameRate;
        }
        public static void ShowAxis() {
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
        public static void DrawRectangle(float x, float y, float w, float h) {
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(x, y);
            GL.Vertex2(x, y + h);
            GL.Vertex2(x + w, y + h);
            GL.Vertex2(x + w, y);
            GL.End();
        }
        public static void DrawPoint(float x, float y) {
            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(x, y);
            GL.End();
        }
        public static void DrawLine(float x1, float y1, float x2, float y2) {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(x1, y1);
            GL.Vertex2(x2, y2);
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
        public static void DrawCircle(float cx, float cy, float r) {
            int count = 20;
            GL.Begin(PrimitiveType.TriangleFan);

            //Center of the circle
            GL.Vertex2(cx, cy);

            for (int i = 0; i <= count; i++) {
                float theta = 2.0f * (float)Math.PI * (float)i / count;

                float x = r * (float)Math.Cos(theta);//calculate the x component
                float y = r * (float)Math.Sin(theta);//calculate the y component
                GL.Vertex2(x + cx, y + cy);//output vertex
            }
            GL.End();
        }
        public static void SetFill(Color color) {
            GL.Color4(color);
        }
        public static float Rndf(float min, float max) {
            return min + (max - min) * (float)rnd.NextDouble();
        }
        public static float Rndf(float max) {
            return Rndf(0f, max);
        }
        public static int Rndi(int min, int max) {
            return rnd.Next(min, max);
        }
        public static int Rndi(int max) {
            return Rndi(0, max);
        }
        public static Vector2 Rndv2(float mag) {
            Vector2 vec = new Vector2(Rndf(-1,1), Rndf(-1,1));
            vec.Normalize();
            vec = vec * mag;
            return vec;
        }
        public static Color Rndc() {
            return Color.FromArgb(Rndi(255), Rndi(255), Rndi(255));
        }

        public static float Truncf(float value, float min, float max) {
            AssertMaxMinf(ref min, ref max);
            value = Math.Max(min, value);
            value = Math.Min(max, value);
            return value;
        }
        public static int Trunci(int value, int min, int max) {
            return (int)Truncf(value, min, max);
        }

        public static float Mapf(float value, float minS, float maxS, float minT, float maxT) {
            value = Truncf(value, minS, maxS);
            float racio = (value - minS) / (maxS - minS);
            float nValue = minT + (maxT - minT) * racio;

            return nValue;
        }
        public static int Mapi(int value, int minS, int maxS, int minT, int maxT) {
            return (int)Mapf(value,minS,maxS,minT,maxT);
        }

        static void AssertMaxMini(ref int min,ref int max) {
            int tmp = max;
            max = Math.Max(min, max);
            min = Math.Min(min, tmp);
        }
        static void AssertMaxMinf(ref float min, ref float max) {
            float tmp = max;
            max = Math.Max(min, max);
            min = Math.Min(min, tmp);
        }

        public class HSLColor {
            // Private data members below are on scale 0-1
            // They are scaled for use externally based on scale
            private double hue = 1.0;
            private double saturation = 1.0;
            private double luminosity = 1.0;

            private const double scale = 240.0;

            public double Hue {
                get {
                    return hue * scale;
                }
                set {
                    hue = CheckRange(value / scale);
                }
            }
            public double Saturation {
                get {
                    return saturation * scale;
                }
                set {
                    saturation = CheckRange(value / scale);
                }
            }
            public double Luminosity {
                get {
                    return luminosity * scale;
                }
                set {
                    luminosity = CheckRange(value / scale);
                }
            }

            private double CheckRange(double value) {
                if (value < 0.0)
                    value = 0.0;
                else if (value > 1.0)
                    value = 1.0;
                return value;
            }

            public override string ToString() {
                return String.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", Hue, Saturation, Luminosity);
            }

            public string ToRGBString() {
                Color color = (Color)this;
                return String.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", color.R, color.G, color.B);
            }

            #region Casts to/from System.Drawing.Color
            public static implicit operator Color(HSLColor hslColor) {
                double r = 0, g = 0, b = 0;
                if (hslColor.luminosity != 0) {
                    if (hslColor.saturation == 0)
                        r = g = b = hslColor.luminosity;
                    else {
                        double temp2 = GetTemp2(hslColor);
                        double temp1 = 2.0 * hslColor.luminosity - temp2;

                        r = GetColorComponent(temp1, temp2, hslColor.hue + 1.0 / 3.0);
                        g = GetColorComponent(temp1, temp2, hslColor.hue);
                        b = GetColorComponent(temp1, temp2, hslColor.hue - 1.0 / 3.0);
                    }
                }
                return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
            }

            private static double GetColorComponent(double temp1, double temp2, double temp3) {
                temp3 = MoveIntoRange(temp3);
                if (temp3 < 1.0 / 6.0)
                    return temp1 + (temp2 - temp1) * 6.0 * temp3;
                else if (temp3 < 0.5)
                    return temp2;
                else if (temp3 < 2.0 / 3.0)
                    return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
                else
                    return temp1;
            }
            private static double MoveIntoRange(double temp3) {
                if (temp3 < 0.0)
                    temp3 += 1.0;
                else if (temp3 > 1.0)
                    temp3 -= 1.0;
                return temp3;
            }
            private static double GetTemp2(HSLColor hslColor) {
                double temp2;
                if (hslColor.luminosity < 0.5)  //<=??
                    temp2 = hslColor.luminosity * (1.0 + hslColor.saturation);
                else
                    temp2 = hslColor.luminosity + hslColor.saturation - (hslColor.luminosity * hslColor.saturation);
                return temp2;
            }

            public static implicit operator HSLColor(Color color) {
                HSLColor hslColor = new HSLColor();
                hslColor.hue = color.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360 
                hslColor.luminosity = color.GetBrightness();
                hslColor.saturation = color.GetSaturation();
                return hslColor;
            }
            #endregion

            public void SetRGB(int red, int green, int blue) {
                HSLColor hslColor = (HSLColor)Color.FromArgb(red, green, blue);
                this.hue = hslColor.hue;
                this.saturation = hslColor.saturation;
                this.luminosity = hslColor.luminosity;
            }

            public HSLColor() {
            }
            public HSLColor(Color color) {
                SetRGB(color.R, color.G, color.B);
            }
            public HSLColor(int red, int green, int blue) {
                SetRGB(red, green, blue);
            }
            public HSLColor(double hue, double saturation, double luminosity) {
                this.Hue = hue;
                this.Saturation = saturation;
                this.Luminosity = luminosity;
            }

        }
    }

}