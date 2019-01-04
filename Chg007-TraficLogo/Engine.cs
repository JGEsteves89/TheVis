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

namespace Chg007_TraficLogo
{

    public class Engine : IEngine
    {
        public static float width = 1000;
        public static float height = 400;
        public static int cols = 10;
        public static int rows = 5;

        public class Particle
        {
            public Vector2 pos;
            public Vector2 vec;
            public Vector2 acc;
            public float radius = 4;
            public Particle(Vector2 pos, Vector2 vec, Vector2 acc)
            {
                this.pos = pos;
                this.vec = vec;
                this.acc = acc;
            }
            public Particle(Vector2 pos) : this(pos, new Vector2(0, 0), new Vector2(0, 0))
            {
            }
            public Particle(Vector2 pos, Vector2 vec) : this(pos, vec, new Vector2(0, 0))
            {
            }
            public virtual void Update()
            {
                pos += vec;
                vec += acc;
            }
            public virtual void Draw()
            {
                SetColor(Color.White);
                DrawCircle(pos.X, pos.Y, radius);
            }
        }
        public class Vehicle : Particle
        {
            public Vector2 target = new Vector2(0, 0);
            public float speed = 2f;
            public Vehicle(Vector2 pos, float speed = 2f) : base(pos)
            {
            }
            public Vehicle(Vector2 pos, Vector2 target, float speed = 2f) : base(pos)
            {
                this.target = target;
                this.acc = Rndv2(1);
            }
            public override void Update()
            {
                base.Update();
                Vector2 heading = target - pos;
                if (Math.Abs(heading.X) > Math.Abs(heading.Y))
                {
                    float dir = (float)heading.X / (float)Math.Abs(heading.X);
                    vec = new Vector2(dir, 0) * speed;
                }
                else
                {
                    float dir = (float)heading.Y / (float)Math.Abs(heading.Y);
                    vec = new Vector2(0, dir) * speed;
                }
                if (heading.Length < speed)
                {
                    pos = target;
                    vec = new Vector2(0, 0);
                }
            }

        }

        public class Cross
        {
            public float size = 10f;
            public Vector2 pos;
            public List<Road> roads = new List<Road>();
            public Cross(Vector2 pos)
            {
                this.pos = pos;
            }

            public void Draw()
            {
                SetColor(Color.FromArgb(125, Color.DarkGray));
                DrawCircle(pos.X, pos.Y, size);
            }
        }
        public class Road
        {
            public List<Vehicle> vehicles = new List<Vehicle>();
            public Cross start;
            public Cross end;
            public float size = 10f;
            public Road(Cross start, Cross end)
            {
                this.start = start;
                this.end = end;
                this.start.roads.Add(this);
                this.end.roads.Add(this);
            }
            public void Draw()
            {
                SetColor(Color.FromArgb(125, Color.DarkGray));
                Vector2 dir = end.pos - start.pos;
                float a = (float)Math.Atan2(dir.Y, dir.X);
                Vector2 su = new Vector2(
                    start.pos.X + size / 2f * (float)Math.Cos(a + Math.PI / 2),
                    start.pos.Y + size / 2f * (float)Math.Sin(a + Math.PI / 2)
                    );
                Vector2 sd = new Vector2(
                    start.pos.X - size / 2f * (float)Math.Cos(a + Math.PI / 2),
                    start.pos.Y - size / 2f * (float)Math.Sin(a + Math.PI / 2)
                    );
                Vector2 eu = new Vector2(
                    end.pos.X + size / 2f * (float)Math.Cos(a + Math.PI / 2),
                    end.pos.Y + size / 2f * (float)Math.Sin(a + Math.PI / 2)
                    );
                Vector2 ed = new Vector2(
                    end.pos.X - size / 2f * (float)Math.Cos(a + Math.PI / 2),
                    end.pos.Y - size / 2f * (float)Math.Sin(a + Math.PI / 2)
                    );
                DrawLine(su.X, su.Y, eu.X, eu.Y);
                DrawLine(sd.X, sd.Y, ed.X, ed.Y);
            }

            internal Vector2 getDir()
            {
                return (end.pos - start.pos).Normalized();
            }
        }
        public class Car
        {
            public float radius = 4;
            public float pace = 2;
            public Cross target;
            public Cross source;
            public Vector2 pos;
            List<Cross> route = new List<Cross>();
            List<Cross> vesited = new List<Cross>();
            public bool arrived = false;
            public int location = 0;
            private bool CalculateRoute(Cross source, Cross target)
            {
                vesited.Add(source);

                if (source == target)
                {
                    route.Add(source);
                    return true;
                }

                foreach (Road item in source.roads)
                {
                    Cross cross = item.end;
                    if (item.end == source)
                        cross = item.start;

                    if (vesited.Contains(cross))
                        continue;

                    if  (CalculateRoute(cross, target)){
                        route.Add(source);
                        return true;
                    }
                }
                return false;
            }


            public Car(Cross source, Cross target)
            {
                this.source = source;
                this.target = target;
                this.pos = source.pos;

                CalculateRoute(source, target);
            }

            public void Update()
            {
                if (arrived) return;

                Cross nextCross = route[location];

                if ((pos - nextCross.pos).Length < radius)
                {
                    location++;
                    pos = nextCross.pos;
                    if (location == route.Count)
                        arrived = true;
                }
                else
                    pos += pace * (nextCross.pos - pos).Normalized();

            }


            public virtual void Draw()
            {
                SetColor(Color.White);
                DrawCircle(pos.X, pos.Y, radius);
            }
        }
        
        List<Cross> crosses = new List<Cross>();
        List<Road> roads = new List<Road>();
        List<Car> cars = new List<Car>();

        public override void SetUp()
        {
            base.SetUp();
            Canvas((int)width, (int)height);
            BackgroundColor(51);
            //float thr = (float)Math.Sqrt(Math.Pow(width / cols, 2) + Math.Pow(height / rows, 2));
            //Vector2 trans = new Vector2(175, height - 100);

            //string text = "Tarina";
            //List<Vector2> stringPoints = GetPointsOfString(text, 200f);
            //List<Vector2> medians = GetMedPoints(text.Length, stringPoints);
            //List<Vector2> nodes = new List<Vector2>();
            //foreach (Vector2 item in medians)
            //{
            //    nodes.Add(item + trans);
            //}
            //for (int i = -1; i < cols + 1; i++)
            //{
            //    float sx = (i) * width / cols;
            //    float ex = (i + 1) * width / cols;
            //    for (int j = -1; j < rows + 1; j++)
            //    {
            //        float sy = (j) * height / rows;
            //        float ey = (j + 1) * height / rows;
            //        nodes.Add(new Vector2(Rndf(sx, ex), Rndf(sy, ey)));
            //    }
            //}
            //for (int i = 0; i < nodes.Count; i++)
            //{
            //    float dBest1 = 9999999;
            //    int iBest1 = -1;
            //    float dBest2 = 9999999;
            //    int iBest2 = -1;
            //    for (int j = i + 1; j < nodes.Count; j++)
            //    {
            //        float dist = (nodes[i] - nodes[j]).LengthFast;
            //        if (dBest1 > dist)
            //        {
            //            dBest1 = dist;
            //            iBest1 = j;
            //        }
            //        else if(dBest2 > dist)
            //        {
            //            dBest2 = dist;
            //            iBest2 = j;
            //        }
            //    }
            //    if (iBest1 != -1)
            //        roads.Add(new Road(nodes[i], nodes[iBest1]));
            //    if (iBest2 != -1)
            //        roads.Add(new Road(nodes[i], nodes[iBest2]));
            //}

            //foreach (Vector2 point in stringPoints)
            //{
            //    pts.Add(new Vehicle(new Vector2(Rndf(width), Rndf(height, height * 2)), point + trans));
            //}
            Cross cross1 = new Cross(new Vector2(300, 300));
            Cross cross2 = new Cross(new Vector2(500, 100));
            Cross cross3 = new Cross(new Vector2(800, 100));

            crosses.Add(cross1);
            crosses.Add(cross2);
            crosses.Add(cross3);

            Road road1 = new Road(cross1, cross2);
            Road road2 = new Road(cross2, cross3);

            roads.Add(road1);
            roads.Add(road2);

            Car car1 = new Car(cross1, cross3);

            cars.Add(car1);
        }



        public override void Draw()
        {
            base.Draw();
            foreach (Car car in cars)
            {
                car.Update();
                car.Draw();
            }
            foreach (Road road in roads)
                road.Draw();


            foreach (Cross cross in crosses)
                cross.Draw();

        }


        List<Vector2> GetPointsOfString(string text, float size)
        {
            int minD = 5;
            List<Vector2> points = new List<Vector2>();
            Vector2 first = new Vector2();
            Bitmap bmp = new Bitmap(Width, Height);
            GraphicsPath gp = new GraphicsPath();
            using (Graphics g = Graphics.FromImage(bmp))
            using (Font f = new Font("Arial", size))
            {
                gp.AddString(text, f.FontFamily, 0, size, new Point(0, 0), StringFormat.GenericDefault);
                first = new Vector2(gp.PathPoints[0].X, -gp.PathPoints[0].Y);
                for (int i = 0; i < gp.PathPoints.Length - 1; i++)
                {
                    if (gp.PathTypes[i + 1] != 0)
                    {
                        Vector2 p1 = new Vector2(gp.PathPoints[i].X, -gp.PathPoints[i].Y);
                        Vector2 p2 = new Vector2(gp.PathPoints[i + 1].X, -gp.PathPoints[i + 1].Y);

                        points.Add(p1);
                        points.AddRange(GetPointsBetween(minD, p1, p2));
                        points.Add(p2);
                    }
                    else
                    {
                        Vector2 last = new Vector2(gp.PathPoints[i].X, -gp.PathPoints[i].Y);
                        points.AddRange(GetPointsBetween(minD, last, first));
                        first = new Vector2(gp.PathPoints[i + 1].X, -gp.PathPoints[i + 1].Y);
                    }
                }

            }
            return points;
        }
        public List<Vector2> GetPointsBetween(int minD, Vector2 p1, Vector2 p2)
        {
            List<Vector2> points = new List<Vector2>();

            Vector2 dir = p2 - p1;
            int segments = (int)dir.Length / minD;
            for (int n = 1; n < segments - 1; n++)
            {
                points.Add(p1 + dir.Normalized() * minD * n);
            }
            return points;
        }
        private List<Vector2> GetMedPoints(int length, List<Vector2> points)
        {
            List<Vector2> avgs = new List<Vector2>();
            float minX = 999999;
            float maxX = 0;
            foreach (Vector2 item in points)
            {
                if (minX > item.X) minX = item.X;
                if (maxX < item.X) maxX = item.X;
            }

            float parc = (maxX - minX) / length;
            for (int i = 0; i < length; i++)
            {
                Vector2 avg = new Vector2(0, 0);
                float count = 0;
                foreach (Vector2 item in points)
                {
                    if (item.X > parc * i && item.X < parc * (i + 1))
                    {
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
