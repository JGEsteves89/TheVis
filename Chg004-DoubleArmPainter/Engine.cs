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

namespace Chg004_DoubleArmPainter {

    public class Engine : IEngine {
        public static float LENGTH = 200;
        public static float osc = 0;
        public Arm arm;
        public List<Vector2> points = new List<Vector2>();
        public List<Color> colors = new List<Color>();
        public int count = 0;
        public override void SetUp() {
            base.SetUp();
            Canvas(800, 800);
            BackgroundColor(255);
            SetLineWidth(5f);
            //SetFrameRate(200);
            arm = new Arm(new Vector2(Size.Width / 2, Size.Height / 2));
            arm.AddChild(new Arm(arm.GetEnd(), LENGTH, (float)Math.PI / 4f));
        }
        public override void Draw() {
            count++;
            int total = 1;
            if (count > 200)
                total = 2;
            if (count > 400)
                total = 5;
            if (count > 800)
                total = 10;
            for (int n = 0; n < total; n++) {
                base.Draw();
                arm.Render();
                // Le croissant
                arm.angle = (float)Math.Sin(osc) * 1.5f;
                arm.child.angle = (float)Math.Sin(0.43f * osc) * 2.5f;

                points.Add(arm.child.GetEnd());

                float pi2 = (float)Math.PI / 8;
                int blue = (int)Mapf(arm.GetVel(), -pi2, pi2, 0, 255);
                int green = (int)Mapf(arm.child.GetVel(), -pi2, pi2, 0, 255);
                colors.Add(Color.FromArgb(255, 0, green, blue));
                SetPointSize(4f);
                for (int i = 0; i < points.Count; i++) {
                    SetColor(colors[i]);
                    DrawPoint(points[i].X, points[i].Y);
                }
                osc += 0.075f;
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
