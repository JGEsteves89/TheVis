using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.IEngine;

namespace Chg007_TraficLogo {
    class StringPoints {

        public static List<Vector2> GetPointsOfString(string text, float size, int Width, int Height) {
            int minD = 1;
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
        public static List<Vector2> GetPointsBetween(int minD, Vector2 p1, Vector2 p2) {
            List<Vector2> points = new List<Vector2>();

            Vector2 dir = p2 - p1;
            int segments = (int)dir.Length / minD;
            for (int n = 1; n < segments - 1; n++) {
                points.Add(p1 + dir.Normalized() * minD * n);
            }
            return points;
        }
        public static List<Vector2> GetMedPoints(int length, List<Vector2> points) {
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
        public static List<Vector2> CreateRandomGridPoints(int cols,int rows,float width,float height) {
            List<Vector2> gridPoints = new List<Vector2>();
            float yOffset = 200;

            for (int i = 0; i < cols + 1; i++) {
                float sx = (i) * width / cols;
                float ex = (i) * width / cols;
                for (int j = -2; j < rows + 2; j++) {
                    float sy = yOffset + (j) * (height - yOffset) / rows;
                    float ey = yOffset + (j) * (height - yOffset) / rows;
                    gridPoints.Add(new Vector2(Rndf(sx, ex), Rndf(sy, ey)));
                }
            }
            return gridPoints;
        }
        public static void Tesselate(ref List<Cross> crosses, ref List<Road> roads) {
            int connections = 3;
            List<string> visited = new List<string>();

            for (int i = 0; i < crosses.Count- connections; i++) {
                List<int> sortedByDist = new List<int>();
                for (int j = 0; j < crosses.Count; j++) {
                    if (i == j) continue;
                    int index;
                    float cur = (crosses[i].pos - crosses[j].pos).LengthFast;
                    for (index = 0; index < sortedByDist.Count; index++) {
                        float dist = (crosses[i].pos - crosses[sortedByDist[index]].pos).LengthFast;
                        if (dist > cur)
                            break;
                    }
                    sortedByDist.Insert(index,j);
                }
                int count = 0;
                int iter = 0;
                while (count!= connections) {
                    int index = sortedByDist[iter];
                    string key = index + " " + i;
                    if (index < i) {
                        key = i + " " + index;
                    }

                    if (!visited.Contains(key)) {
                        visited.Add(key);
                        roads.Add(new Road(crosses[i], crosses[index]));
                        count++;
                        //Console.WriteLine(key);
                    }
                    iter++;
                }

            }
        }
        public static Cross FindCrossCloses(ref List<Cross> crosses, Vector2 pos) {
            Cross bestCross = crosses.First() ;
            float bestDist = 10000;
            foreach (Cross item in crosses) {
                float dist = (item.pos - pos).LengthFast;
                if (dist < bestDist) {
                    bestDist = dist;
                    bestCross = item;
                }
            }
            return bestCross;
        }
    }
}
