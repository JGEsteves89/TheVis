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
using static Chg007_TraficLogo.StringPoints;

namespace Chg007_TraficLogo {

    public class Engine : IEngine {
        public static float width = 800;
        public static float height = 800;
        public static int cols = 6;
        public static int rows = 8;
        public static float delay = 0;
        List<Cross> crosses = new List<Cross>();
        List<Road> roads = new List<Road>();
        List<Car> cars = new List<Car>();

        public override void SetUp() {
            base.SetUp();
            Canvas((int)width, (int)height);
            BackgroundColor(51);
            Vector2 trans = new Vector2(70, 200);

            string text = "Tarina";
            List<Vector2> stringPoints = GetPointsOfString(text, 200f, (int)width, (int)height);
            List<Vector2> medians = GetMedPoints(text.Length, stringPoints);
            List<Vector2> gridPoints = CreateRandomGridPoints(cols, rows, width, height);


            foreach (Vector2 gridPoint in gridPoints) {
                crosses.Add(new Cross(crosses.Count, gridPoint));
            }
            //foreach (Vector2 median in medians) {
            //    crosses.Add(new Cross(crosses.Count, median + trans));
            //}

            Tesselate(ref crosses, ref roads);

            Graph graph = new Graph();
            foreach (Node item in crosses) {
                graph.nodes.Add(item);
            }
            foreach (Road item in roads) {
                graph.edges.Add(item);
            }

            foreach (Vector2 stringPoint in stringPoints) {
                Vector2 targetPoint = stringPoint + trans;
                Vector2 sourcePoint = new Vector2();

                sourcePoint.X = Rndf(-0.1f, 0.1f) * Width + targetPoint.X;
                sourcePoint.Y = height * 2;

                Cross targetCross = FindCrossCloses(ref crosses, targetPoint);
                Cross sourceCross = FindCrossCloses(ref crosses, sourcePoint);

                List<Cross> route = new List<Cross>();
                foreach (Car car in cars) {
                    if (car.source.i == sourceCross.i && car.target.i == targetCross.i) {
                        route = car.route;
                        break;
                    }
                }
                if (route.Count == 0) {
                    List<int> routeInd = Dijkstra.runDijkstra(graph, sourceCross.i, targetCross.i);
                    foreach (int i in routeInd) {
                        route.Add(crosses[i]);
                    }
                }

                cars.Add(new Car(sourceCross, targetCross, targetPoint, route));
                Console.WriteLine(stringPoints.Count + "/" + cars.Count());
            }
        }


        public override void Draw() {
            base.Draw();
            delay+=5f;
            if (delay > cars.Count()) delay = cars.Count();
            for (int i = 0; i < cars.Count(); i++) {
                if (delay>i)
                    cars[i].Update();
                cars[i].Draw();
            }
            //foreach (Road road in roads)
            //    road.Draw();

            //foreach (Cross cross in crosses)
            //    cross.Draw();

        }

    }
}
