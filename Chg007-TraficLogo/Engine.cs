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

namespace Chg007_TraficLogo {

    public class Engine : IEngine {
        public static float width = 1000;
        public static float height = 400;
        public static int cols = 5;
        public static int rows = 2;
        public class Road {
            public List<Vehicle> vehicles = new List<Vehicle>();
            public Vector2 start;
            public Vector2 end;
            public float size = 10f;
            public Road(Vector2 start, Vector2 end) {
                this.start = start;
                this.end = end;
            }
            public void Draw() {
                SetColor(Color.Gray);
                Vector2 dir = end - start;
                float a = (float)Math.Atan2(dir.Y, dir.X);
                Vector2 su = new Vector2(
                    start.X + size/2f * (float)Math.Cos(a + Math.PI / 2),
                    start.Y + size / 2f * (float)Math.Sin(a + Math.PI / 2)
                    );
                Vector2 sd = new Vector2(
                    start.X - size / 2f * (float)Math.Cos(a + Math.PI / 2),
                    start.Y - size / 2f * (float)Math.Sin(a + Math.PI / 2)
                    );
                Vector2 eu = new Vector2(
                    end.X + size / 2f * (float)Math.Cos(a + Math.PI / 2),
                    end.Y + size / 2f * (float)Math.Sin(a + Math.PI / 2)
                    );
                Vector2 ed = new Vector2(
                    end.X - size / 2f * (float)Math.Cos(a + Math.PI / 2),
                    end.Y - size / 2f * (float)Math.Sin(a + Math.PI / 2)
                    );
                DrawLine(su.X, su.Y, eu.X, eu.Y);
                DrawLine(sd.X, sd.Y, ed.X, ed.Y);
            }
        }
        public class Particle {
            public Vector2 pos;
            public Vector2 vec;
            public Vector2 acc;
            public float radius = 4;
            public Particle(Vector2 pos, Vector2 vec, Vector2 acc) {
                this.pos = pos;
                this.vec = vec;
                this.acc = acc;
            }
            public Particle(Vector2 pos) : this(pos, new Vector2(0, 0), new Vector2(0, 0)) {
            }
            public Particle(Vector2 pos, Vector2 vec) : this(pos, vec, new Vector2(0, 0)) {
            }
            public virtual void Update() {
                pos += vec;
                vec += acc;
            }
            public virtual void Draw() {
                SetColor(Color.White);
                DrawCircle(pos.X, pos.Y, radius);
            }
        }
        public class Vehicle : Particle {
            public Vector2 target = new Vector2(0, 0);
            public float speed = 2f;
            public Vehicle(Vector2 pos, float speed = 2f) : base(pos) {
            }
            public Vehicle(Vector2 pos, Vector2 target, float speed = 2f) : base(pos) {
                this.target = target;
                this.acc = Rndv2(1);
            }
            public override void Update() {
                base.Update();
                Vector2 heading = target - pos;
                if (Math.Abs(heading.X) > Math.Abs(heading.Y)) {
                    float dir = (float)heading.X / (float)Math.Abs(heading.X);
                    vec = new Vector2(dir, 0) * speed;
                } else {
                    float dir = (float)heading.Y / (float)Math.Abs(heading.Y);
                    vec = new Vector2(0, dir) * speed;
                }
                if (heading.Length < speed) {
                    pos = target;
                    vec = new Vector2(0, 0);
                }
            }

        }
        List<Vehicle> pts = new List<Vehicle>();
        List<Road> roads = new List<Road>();
        public override void SetUp() {
            base.SetUp();
            Canvas((int)width, (int)height);
            BackgroundColor(51);
            float thr = (float)Math.Sqrt(Math.Pow( width / cols,2) + Math.Pow(height / rows, 2));
            Vector2 trans = new Vector2(175, height - 100);

            string text = "Tarina";
            List<Vector2> stringPoints = GetPointsOfString(text, 200f);
            List<Vector2> medians = GetMedPoints(text.Length, stringPoints);
            List<Vector2> nodes = new List<Vector2>();
            foreach (Vector2 item in medians) {
                nodes.Add(item + trans);
            }
            for (int i = -1; i < cols+1; i++) {
                float sx = (i) * width / cols;
                float ex = (i+1) * width / cols;
                for (int j = -1; j < rows+1; j++) {
                    float sy = (j) * height / rows;
                    float ey = (j + 1) * height / rows;
                    nodes.Add(new Vector2(Rndf(sx, ex), Rndf(sy, ey)));
                }
            }
            for (int i = 0; i < nodes.Count; i++) {
                List<Vector2> near = new List<Vector2>();
                for (int j = i+1; j < nodes.Count; j++) {
                    float dist = (nodes[i] - nodes[j]).Length;
                    if (dist < thr) {

                        roads.Add(new Road(nodes[i], nodes[j]));

                    }
                }
            }
            
            foreach (Vector2 point in stringPoints) {
                pts.Add(new Vehicle(
                    new Vector2(Rndf(width), Rndf(height, height * 2)), 
                    point + trans));
            }


        }

        

        public override void Draw() {
            base.Draw();

            foreach (Vehicle item in pts) {
                item.Update();
                item.Draw();
            }
            foreach (Road item in roads) {
                item.Draw();
            }
        }




        List<Vector2> GetPointsOfString(string text, float size) {
            int minD = 5;
            List<Vector2> points = new List<Vector2>();
            Vector2 first = new Vector2();
            Bitmap bmp = new Bitmap(Width, Height);
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
            return points;
        }
        public List<Vector2> GetPointsBetween(int minD, Vector2 p1, Vector2 p2) {
            List<Vector2> points = new List<Vector2>();

            Vector2 dir = p2 - p1;
            int segments = (int)dir.Length / minD;
            for (int n = 1; n < segments - 1; n++) {
                points.Add(p1 + dir.Normalized() * minD * n);
            }
            return points;
        }
        private List<Vector2> GetMedPoints(int length, List<Vector2> points) {
            List<Vector2> avgs = new List<Vector2>();
            float minX = 999999;
            float maxX = 0;
            foreach (Vector2 item in points) {
                if (minX > item.X) minX = item.X;
                if (maxX < item.X) maxX = item.X;
            }

            float parc = (maxX - minX) / length;
            for (int i = 0; i < length; i++) {
                Vector2 avg = new Vector2(0, 0);
                float count = 0;
                foreach (Vector2 item in points) {
                    if (item.X > parc * i && item.X < parc * (i + 1)) {
                        avg.X += item.X;
                        avg.Y += item.Y;
                        count++;
                    }
                }
                avg.X /= count;
                avg.Y /= count;
                avgs.Add(avg);
            }
            return avgs;
        }
    }
}
