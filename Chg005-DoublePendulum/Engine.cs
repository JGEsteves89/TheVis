using Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Windows.Forms;
using OpenTK.Input;

namespace Chg005_DoublePendulum {
    public class Engine : IEngine {
        public static float LENGTH = 150;
        public static float G = 1;
        public static float osc = 0;
        public Arm arm;
        public List<Vector2> points = new List<Vector2>();
        public List<Color> colors = new List<Color>();
        public float count = 0f;
        public override void SetUp() {
            base.SetUp();
            Canvas(800, 800);
            BackgroundColor(51);
            SetLineWidth(5f);
            //SetFrameRate(200);
            arm = new Arm(new Vector2(Size.Width / 2, Size.Height / 2));
            arm.AddChild(new Arm(arm.GetEnd(), LENGTH, (float)Math.PI / 4f));
        }
        public override void Draw() {

            base.Draw();
            arm.Render();
            //// Le croissant
            //float m1 = 1f;
            //float m2 = 1f;

            //float a1 = arm.angle;
            //float a2 = arm.child.angle;

            //float a1_v = arm.angle;
            //float a2_v = arm.child.angle;

            //float a1_a = arm.angle;
            //float a2_a = arm.child.angle;

            //float r1 = arm.length;
            //float r2 = arm.length;

            //float num1, num2, num3, num4, den;

            ////      −g   (2   m1 + m2)   sin θ1
            //num1 = (-G * (2 * m1 + m2) * (float)Math.Sin(a1));

            ////      −m2   g   (float)Math.Sin(θ1 − 2   θ2)
            //num2 = (-m2 * G * (float)Math.Sin(a1 - 2 * a2));

            ////      −2   (float)Math.Sin(θ1 − θ2)   m2
            //num3 = (-2 * (float)Math.Sin(a1 - a2) * m2);

            ////      θ2'2          L2 + θ1'2          L1   (float)Math.Cos(θ1 − θ2)
            //num4 = (float)(Math.Sqrt(a2_v) * r2 + Math.Sqrt(a1_v) * r1 * (float)Math.Cos(a1 - a2));

            ////     L1   (2   m1 + m2 − m2   (float)Math.Cos(2   θ1 − 2   θ2))
            //den = (r1 * (2 * m1 + m2 - m2 * (float)Math.Cos(2 * a1 - 2 * a2)));

            //a1_a = ((num1 + num2 + (num3 * num4)) / den);


            ////      2   (float)Math.Sin(θ1 − θ2)
            //num1 = (2 * (float)Math.Sin(a1 - a2));

            ////      θ1'2          L1   (m1 + m2)
            //num2 = (sq(a1_v) * r1 * (m1 + m2));

            ////      g   (m1 + m2)   (float)Math.Cos θ1
            //num3 = (g * (m1 + m2) * (float)Math.Cos(a1));

            ////      θ2'2          L2   m2   (float)Math.Cos(θ1 − θ2))
            //num4 = (sq(a2_v) * r2 * m2 * (float)Math.Cos(a1 - a2));

            ////     L2   (2   m1 + m2 − m2   (float)Math.Cos(2   θ1 − 2   θ2))
            //den = (r2 * (2 * m1 + m2 - m2 * (float)Math.Cos(2 * a1 - 2 * a2)));

            //a2_a = ((num1 * (num2 + num3 + num4)) / den);


            arm.child.angle = (float)Math.Sin(-0.43f * osc) * 2.5f;

            points.Add(arm.child.GetEnd());

            float pi2 = 0.021f;
            if (arm.GetVel() > count) {
                count = arm.GetVel();
                //Console.WriteLine(count);
            }
            int blue = (int)Mapf(arm.GetVel(), -pi2, pi2, 0, 255);
            int green = (int)Mapf(arm.child.GetVel(), -pi2, pi2, 0, 255);
            //int blue = (int)Mapf(Math.Abs(arm.GetVel()), 0, pi2, 0, 255);
            //int green = (int)Mapf(Math.Abs(arm.child.GetVel()), 0, pi2, 0, 255);
            colors.Add(Color.FromArgb(255, 0, green, blue));
            osc += 0.1f;


            SetPointSize(4f);
            for (int i = 0; i < points.Count; i++) {
                SetColor(colors[i]);
                DrawPoint(points[i].X, points[i].Y);
            }


        }

        public class Arm {
            public Vector2 start;
            public float length;
            public float angle;
            public float oldAngle = 0;
            public float parentAngle = 0;
            public Arm child;
            public Arm(Vector2 start, float length = 0, float angle = 0) {
                this.start = start;
                this.angle = angle;
                this.length = length;
                if (length == 0) this.length = LENGTH;
            }
            public Vector2 GetEnd() {
                return new Vector2(
                    start.X + length * (float)Math.Cos(angle + parentAngle),
                    start.Y + length * (float)Math.Sin(angle + parentAngle));
            }
            public float GetVel() {
                return oldAngle - (angle + parentAngle);
            }
            public void AddChild(Arm arm) {
                child = arm;
            }
            public void Render() {
                Vector2 end = GetEnd();

                SetColor(Color.LimeGreen);
                SetPointSize(1f);
                DrawPoint(start.X, start.Y);
                DrawPoint(end.X, end.Y);

                SetColor(Color.Black);
                DrawLine(start.X, start.Y, end.X, end.Y);

                if (child != null) {
                    RenderChild();
                }
                oldAngle = angle + parentAngle;
            }
            public void RenderChild() {
                child.start = GetEnd();
                child.parentAngle = angle;
                child.Render();
            }
        }
    }
}
