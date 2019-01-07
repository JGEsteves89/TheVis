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
using System.Drawing.Drawing2D;

namespace Chg008_MagicWords {

    public class Engine : IEngine {
        public static float WIDTH = 800;
        public static float HEIGHT = 800;
        public static int OUTPUT_SIZE = 6;
        public static Vector2 GRAVITY = new Vector2(0,-0.05f);
        public static List<Vector2> TARGETS = GetPointsOfString("Tarina", 200f, new Vector2(70, 200));

        public Vector2 target = new Vector2(WIDTH / 2, HEIGHT / 2);
        public Comet comet = new Comet(new Vector2(-20, HEIGHT));

        public override void SetUp() {
            base.SetUp();
            Canvas((int)WIDTH, (int)HEIGHT);
            BackgroundColor(51);

            comet.vec = new Vector2(1f, 5f);
            comet.radius = 5;
            comet.target = target;
            comet.targetActive = true;
        }
        public override void Draw() {
            base.Draw();
            comet.Update();
            comet.Draw();
        }

        public static List<Vector2> GetPointsOfString(string text, float size,Vector2 trans) {
            int minD = 1;
            List<Vector2> points = new List<Vector2>();
            Vector2 first = new Vector2();
            Bitmap bmp = new Bitmap((int)WIDTH,(int) HEIGHT);
            GraphicsPath gp = new GraphicsPath();
            using (Graphics g = Graphics.FromImage(bmp))
            using (Font f = new Font("Arial", size)) {
                gp.AddString(text, f.FontFamily, 0, size, new Point(0, 0), StringFormat.GenericDefault);
                first = new Vector2(gp.PathPoints[0].X, -gp.PathPoints[0].Y);
                for (int i = 0; i < gp.PathPoints.Length - 1; i++) {
                    if (gp.PathTypes[i + 1] != 0) {
                        Vector2 p1 = new Vector2(gp.PathPoints[i].X, -gp.PathPoints[i].Y);
                        Vector2 p2 = new Vector2(gp.PathPoints[i + 1].X, -gp.PathPoints[i + 1].Y);

                        points.Add(p1);
                        points.AddRange(GetPointsBetween(minD, p1, p2));
                        points.Add(p2);
                    } else {
                        Vector2 last = new Vector2(gp.PathPoints[i].X, -gp.PathPoints[i].Y);
                        points.AddRange(GetPointsBetween(minD, last, first));
                        first = new Vector2(gp.PathPoints[i + 1].X, -gp.PathPoints[i + 1].Y);
                    }
                }

            }
            for (int i = 0; i < points.Count; i++) {
                points[i] += trans;
            }
            return points;
        }
        public static List<Vector2> GetPointsBetween(int minD, Vector2 p1, Vector2 p2) {
            List<Vector2> points = new List<Vector2>();

            Vector2 dir = p2 - p1;
            int segments = (int)dir.Length / minD;
            for (int n = 1; n < segments - 1; n++) {
                points.Add(p1 + dir.Normalized() * minD * n);
            }
            return points;
        }
    }
}
